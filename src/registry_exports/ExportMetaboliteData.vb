Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_data.Exports

Public Module ExportMetaboliteData

    <Extension>
    Public Iterator Function ExportMetabolites(registry As biocad_registry,
                                               dbname As String,
                                               ontology_id As UInteger,
                                               Optional filterMass As Boolean = True) As IEnumerable(Of MetaLib)
        Dim page_size As Integer = 100
        Dim offset As UInteger
        Dim page As metabolites()
        Dim q As FieldAssert() = If(dbname.StringEmpty, Nothing, {Not field(dbname).is_nothing, field(dbname) <> ""})

        For i As Integer = 1 To Integer.MaxValue
            offset = (i - 1) * page_size

            If q Is Nothing Then
                page = registry.metabolites _
                    .limit(offset, page_size) _
                    .select(Of metabolites)
            Else
                page = registry.metabolites _
                    .where(q) _
                    .limit(offset, page_size) _
                    .select(Of metabolites)
            End If

            If page.IsNullOrEmpty Then
                Exit For
            End If

            For Each m As metabolites In page
                If filterMass AndAlso m.exact_mass <= 1 Then
                    Continue For
                Else
                    Yield registry.BuildMetabolite(m, ontology_id)
                End If
            Next
        Next
    End Function

    <Extension>
    Private Function BuildMetabolite(registry As biocad_registry, m As metabolites, ontology_id As UInteger) As MetaLib
        Dim class_info = registry.GetClass(m.id, ontology_id)
        Dim struct = registry.struct_data _
            .where(field("metabolite_id") = m.id) _
            .find(Of struct_data)
        Dim model As New MetaLib With {
            .ID = "BioCAD" & m.id.ToString.PadLeft(11, "0"c),
            .name = m.name,
            .formula = m.formula,
            .description = m.note,
            .exact_mass = m.exact_mass,
            .IUPACName = m.name,
            .xref = New xref With {
                .CAS = If(m.cas_id.StringEmpty, Nothing, {m.cas_id}),
                .chebi = If(m.chebi_id > 0, $"ChEBI:{m.chebi_id}", ""),
                .DrugBank = m.drugbank_id,
                .HMDB = m.hmdb_id,
                .KEGG = m.kegg_id,
                .lipidmaps = m.lipidmaps_id,
                .MetaCyc = m.biocyc,
                .Wikipedia = m.wikipedia,
                .MeSH = m.mesh_id,
                .pubchem = m.pubchem_cid,
                .SMILES = If(struct Is Nothing, Nothing, struct.smiles)
            },
            .[class] = class_info?.class,
            .kingdom = class_info?.kingdom,
            .sub_class = class_info?.sub_class,
            .super_class = class_info?.super_class,
            .molecular_framework = class_info?.molecular_framework
        }

        Return model
    End Function

    <Extension>
    Private Function GetClass(registry As biocad_registry, metabolite_id As UInteger, ontology_id As UInteger) As ClassyfireInfoTable
        Dim class_data As New ClassyfireInfoTable
        Dim lineage As ontology() = registry.GetClassLineage(metabolite_id, ontology_id)

        If lineage Is Nothing Then
            Return Nothing
        Else
            ' class_data.kingdom = lineage.ElementAtOrDefault(0)?.term
            class_data.super_class = lineage.ElementAtOrDefault(0)?.term
            class_data.class = lineage.ElementAtOrDefault(1)?.term
            class_data.sub_class = lineage.ElementAtOrDefault(2)?.term
            ' class_data.molecular_framework = lineage.ElementAtOrDefault(3)?.term

            Return class_data
        End If
    End Function
End Module
