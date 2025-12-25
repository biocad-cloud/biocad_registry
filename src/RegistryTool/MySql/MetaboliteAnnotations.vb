Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My
Imports Metadata = BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaLib

Module MetaboliteAnnotations

    Public Iterator Function ExportAnnotation(Optional db_subset As String = Nothing) As IEnumerable(Of Metadata)
        Dim page_size As Integer = 2000
        Dim page As Integer = 1
        Dim offset As UInteger
        Dim pagedata As biocad_registryModel.molecule()
        Dim key As UInteger

        If Not db_subset.StringEmpty Then
            key = MyApplication.biocad_registry.vocabulary.where(field("category") = "External Database", field("term") = db_subset).find(Of biocad_registryModel.vocabulary).id
        End If

        Do While True
            offset = (page - 1) * page_size
            page += 1

            If db_subset.StringEmpty Then
                pagedata = MyApplication.biocad_registry.molecule _
                    .where(field("type") = 213) _
                    .limit(offset, page_size) _
                    .select(Of biocad_registryModel.molecule)
            Else
                pagedata = MyApplication.biocad_registry.molecule _
                    .where(field("type") = 213, field("id").in($"SELECT obj_id FROM db_xrefs WHERE db_key = {key} AND type = 213")) _
                    .limit(offset, page_size) _
                    .select(Of biocad_registryModel.molecule)
            End If

            If pagedata.IsNullOrEmpty Then
                Exit Do
            End If

            For Each mol As biocad_registryModel.molecule In pagedata
                Dim xrefs As db_xref() = MyApplication.biocad_registry.db_xrefs _
                    .left_join("vocabulary") _
                    .on(field("vocabulary.`id`") = field("db_key")) _
                    .where(field("obj_id") = mol.id) _
                    .select(Of db_xref)("term AS db_name", "xref")
                Dim xrefIndex = xrefs.SafeQuery _
                    .GroupBy(Function(a) a.db_name.ToLower) _
                    .ToDictionary(Function(a) a.Key,
                                  Function(a)
                                      Return a.First.xref
                                  End Function)
                Dim db_xrefs As New xref With {
                    .CAS = If(xrefIndex.ContainsKey("cas"), {xrefIndex!cas}, {}),
                    .chebi = xrefIndex.TryGetValue("chebi"),
                    .ChEMBL = xrefIndex.TryGetValue("chembl"),
                    .ChemIDplus = xrefIndex.TryGetValue("chemidplus"),
                    .chemspider = xrefIndex.TryGetValue("chemspider"),
                    .DrugBank = xrefIndex.TryGetValue("drugbank"),
                    .foodb = xrefIndex.TryGetValue("foodb"),
                    .HMDB = xrefIndex.TryGetValue("hmdb"),
                    .KEGG = xrefIndex.TryGetValue("kegg"),
                    .KEGGdrug = xrefIndex.TryGetValue("kegg drug"),
                    .KNApSAcK = xrefIndex.TryGetValue("knapsack"),
                    .lipidmaps = xrefIndex.TryGetValue("lipidmaps"),
                    .MeSH = xrefIndex.TryGetValue("mesh"),
                    .metlin = xrefIndex.TryGetValue("metlin"),
                    .MetaCyc = xrefIndex.TryGetValue("metacyc"),
                    .pubchem = xrefIndex.TryGetValue("pubchem"),
                    .Wikipedia = xrefIndex.TryGetValue("wikipedia")
                }
                Dim smiles = MyApplication.biocad_registry.sequence_graph _
                    .where(field("molecule_id") = mol.id) _
                    .find(Of biocad_registryModel.sequence_graph)

                If Not smiles Is Nothing Then
                    db_xrefs.SMILES = smiles.sequence
                End If

                Yield New Metadata With {
                    .ID = "BioCAD" & mol.id.ToString().PadLeft(11, "0"c),
                    .description = mol.note,
                    .exact_mass = mol.mass,
                    .formula = mol.formula,
                    .IUPACName = mol.name,
                    .name = mol.name,
                    .xref = db_xrefs
                }
            Next
        Loop
    End Function

End Module

Public Class db_xref

    <DatabaseField> Public Property db_name As String
    <DatabaseField> Public Property xref As String

End Class