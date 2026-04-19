Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemoinformatics.Metabolite
Imports BioNovoGene.BioDeep.Chemoinformatics.Metabolite.CrossReference
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_data.Exports

Public Module ExportMetaboliteData

    <Extension>
    Public Iterator Function ExportMetabolites(registry As biocad_registry,
                                               dbname As String,
                                               ontology_id As UInteger,
                                               Optional filterMass As Boolean = True,
                                               Optional zh_class As Boolean = False) As IEnumerable(Of MetaLib)
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
                    Yield registry.BuildMetabolite(m, ontology_id, zh_class:=zh_class)
                End If
            Next
        Next
    End Function

    <Extension>
    Public Iterator Function ExportMetabolites(registry As biocad_registry, meta_ids As IEnumerable(Of UInteger), ontology_id As UInteger,
                                               Optional filterMass As Boolean = True,
                                               Optional zh_class As Boolean = False) As IEnumerable(Of MetaLib)
        Dim page_size As Integer = 100
        Dim page As metabolites()

        For Each page_id As UInteger() In meta_ids.SplitIterator(page_size)
            page = registry.metabolites _
                .where(field("id").in(page_id)) _
                .select(Of metabolites)

            If page.IsNullOrEmpty Then
                Exit For
            End If

            For Each m As metabolites In page
                If filterMass AndAlso m.exact_mass <= 1 Then
                    Continue For
                Else
                    Yield registry.BuildMetabolite(m, ontology_id, zh_class:=zh_class)
                End If
            Next
        Next
    End Function

    <Extension>
    Public Function BuildMetabolite(registry As biocad_registry, m As metabolites, ontology_id As UInteger, zh_class As Boolean) As MetaLib
        Dim class_info = registry.GetClass(m.id, ontology_id, lang_zh:=zh_class)
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
            .molecular_framework = class_info?.molecular_framework,
            .zh_name = m.name_zh
        }

        If m.main_id > 0 Then
            model.xref.extras = New Dictionary(Of String, String()) From {
                {"main_id", {"BioCAD" & m.main_id.ToString.PadLeft(11, "0"c)}}
            }
        End If

        Return model
    End Function

    <Extension>
    Private Function GetClass(registry As biocad_registry, metabolite_id As UInteger, ontology_id As UInteger, lang_zh As Boolean) As CompoundClass
        Dim class_data As New CompoundClass
        Dim lineage As ontology() = registry.GetClassLineage(metabolite_id, ontology_id)

        If lineage Is Nothing Then
            Return Nothing
        Else
            Dim super = If(lineage.ElementAtOrDefault(0), New ontology)
            Dim main = If(lineage.ElementAtOrDefault(1), New ontology)
            Dim subclass = If(lineage.ElementAtOrDefault(2), New ontology)

            class_data.super_class = If(lang_zh, super.term_zh, super.term)
            class_data.class = If(lang_zh, main.term_zh, main.term)
            class_data.sub_class = If(lang_zh, subclass.term_zh, subclass.term)

            Return class_data
        End If
    End Function
End Module
