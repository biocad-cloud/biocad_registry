Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel

Public Module ExportMetaboliteData

    <Extension>
    Public Iterator Function ExportMetabolites(registry As biocad_registry, dbname As String) As IEnumerable(Of MetaLib)
        Dim page_size As Integer = 100
        Dim offset As UInteger
        Dim page As metabolites()
        Dim struct As struct_data
        Dim q = {Not field(dbname).is_nothing, field(dbname) <> ""}

        For i As Integer = 1 To Integer.MaxValue
            offset = (i - 1) * page_size

            If dbname.StringEmpty() Then
                page = registry.metabolites _
                    .limit(offset, page_size) _
                    .select(Of metabolites)
            Else
                page = registry.metabolites _
                    .where(q) _
                    .limit(offset, page_size) _
                    .select(Of metabolites)
            End If

            For Each m As metabolites In page
                struct = registry.struct_data _
                    .where(field("metabolite_id") = m.id) _
                    .find(Of struct_data)

                Yield New MetaLib With {
                    .ID = "BioCAD" & m.id.ToString.PadLeft(11, "0"c),
                    .name = m.name,
                    .formula = m.formula,
                    .description = m.note,
                    .exact_mass = m.exact_mass,
                    .IUPACName = m.name,
                    .xref = New xref With {
                        .CAS = If(m.cas_id.StringEmpty, Nothing, {m.cas_id}),
                        .chebi = m.chebi_id,
                        .DrugBank = m.drugbank_id,
                        .HMDB = m.hmdb_id,
                        .KEGG = m.kegg_id,
                        .lipidmaps = m.lipidmaps_id,
                        .MetaCyc = m.biocyc,
                        .Wikipedia = m.wikipedia,
                        .MeSH = m.mesh_id,
                        .pubchem = m.pubchem_cid,
                        .SMILES = If(struct Is Nothing, Nothing, struct.smiles)
                    }
                }
            Next
        Next
    End Function
End Module
