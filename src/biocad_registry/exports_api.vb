Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports rdataframe = SMRUCC.Rsharp.Runtime.Internal.[Object].dataframe

<Package("data_exports")>
Module exports_api

    <ExportAPI("export_fingerprints")>
    Public Function export_fingerprint(registry As biocad_registry) As Object
        Dim mat As NamedCollection(Of Double)() = Embedding.ExportGenomicsFingerprint(registry).ToArray
        Dim df As New rdataframe With {
            .columns = New Dictionary(Of String, Array),
            .rownames = mat _
                .Select(Function(i) i.name) _
                .ToArray
        }
        Dim len As Integer = mat(0).Length
        Dim offset As Integer

        For i As Integer = 0 To len - 1
            offset = i
            df.add($"v{i + 1}", From nt As NamedCollection(Of Double)
                                In mat
                                Select nt(offset))
        Next

        df.setAttribute("names", mat.Select(Function(i) i.description).ToArray)

        Return df
    End Function

    <ExportAPI("export_metabolites")>
    Public Function export_metabolites(registry As biocad_registry) As Object
        Dim mapping As New Dictionary(Of String, Object)
        Dim list As list = list.empty
        Dim page_size As Integer = 10000
        Dim mona As UInteger = registry.vocabulary_terms.GetDatabaseKey("MoNA")
        Dim cas As UInteger = registry.vocabulary_terms.GetDatabaseKey("CAS")
        Dim kegg As UInteger = registry.vocabulary_terms.GetDatabaseKey("KEGG")
        Dim kegg_drug As UInteger = registry.vocabulary_terms.GetDatabaseKey("KEGGdrug")
        Dim hmdb As UInteger = registry.vocabulary_terms.GetDatabaseKey("HMDB")
        Dim lipidmaps As UInteger = registry.vocabulary_terms.GetDatabaseKey("lipidmaps")
        Dim mesh As UInteger = registry.vocabulary_terms.GetDatabaseKey("MeSH")
        Dim biocyc As UInteger = registry.vocabulary_terms.GetDatabaseKey("biocyc")
        Dim chebi As UInteger = registry.vocabulary_terms.GetDatabaseKey("chebi")
        Dim chembl As UInteger = registry.vocabulary_terms.GetDatabaseKey("ChEMBL")
        Dim ChemIDplus As UInteger = registry.vocabulary_terms.GetDatabaseKey("ChemIDplus")
        Dim chemspider As UInteger = registry.vocabulary_terms.GetDatabaseKey("chemspider")
        Dim DrugBank As UInteger = registry.vocabulary_terms.GetDatabaseKey("DrugBank")
        Dim foodb As UInteger = registry.vocabulary_terms.GetDatabaseKey("foodb")
        Dim KNApSAcK As UInteger = registry.vocabulary_terms.GetDatabaseKey("KNApSAcK")
        Dim Wikipedia As UInteger = registry.vocabulary_terms.GetDatabaseKey("Wikipedia")
        Dim PubChem As UInteger = registry.vocabulary_terms.GetDatabaseKey("PubChem")
        Dim metlin As UInteger = registry.vocabulary_terms.GetDatabaseKey("metlin")

        For i As Integer = 0 To Integer.MaxValue
            Dim pagedata = registry.molecule _
                .where(field("type") = registry.vocabulary_terms.metabolite_term) _
                .limit(i * page_size, page_size) _
                .select(Of biocad_registryModel.molecule)

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            Dim bar As Tqdm.ProgressBar = Nothing

            For Each metabolite As biocad_registryModel.molecule In TqdmWrapper.Wrap(pagedata, bar:=bar)
                Dim cad_id As String = "BioCAD" & metabolite.id.ToString.PadLeft(11, "0"c)
                Dim mona_xrefs As biocad_registryModel.db_xrefs() = registry.db_xrefs _
                    .where(field("db_key") = mona,
                           field("obj_id") = metabolite.id) _
                    .select(Of biocad_registryModel.db_xrefs)

                For Each id As biocad_registryModel.db_xrefs In mona_xrefs
                    mapping(id.xref) = cad_id
                Next

                Call bar.SetLabel(cad_id & " -> " & metabolite.name)

                Dim xrefs = registry.db_xrefs _
                    .where(field("db_key") <> mona,
                           field("obj_id") = metabolite.id) _
                    .select(Of biocad_registryModel.db_xrefs)() _
                    .GroupBy(Function(a) a.db_key) _
                    .ToDictionary(Function(a) a.Key,
                                  Function(a)
                                      Return a _
                                          .Select(Function(x) x.xref) _
                                          .Distinct _
                                          .ToArray
                                  End Function)
                Dim metab As New MetaInfo With {
                    .ID = cad_id,
                    .description = metabolite.note,
                    .exact_mass = metabolite.mass,
                    .formula = metabolite.formula,
                    .IUPACName = metabolite.name,
                    .name = metabolite.name,
                    .synonym = registry.synonym.where(field("obj_id") = metabolite.id).project(Of String)("synonym"),
                    .xref = New xref With {
                        .CAS = xrefs.TryGetValue(cas),
                        .chebi = xrefs.TryGetValue(chebi).DefaultFirst,
                        .ChEMBL = xrefs.TryGetValue(chembl).DefaultFirst,
                        .ChemIDplus = xrefs.TryGetValue(ChemIDplus).DefaultFirst,
                        .chemspider = xrefs.TryGetValue(chemspider).DefaultFirst,
                        .DrugBank = xrefs.TryGetValue(DrugBank).DefaultFirst,
                        .foodb = xrefs.TryGetValue(foodb).DefaultFirst,
                        .HMDB = xrefs.TryGetValue(hmdb).DefaultFirst,
                        .KEGG = xrefs.TryGetValue(kegg).DefaultFirst,
                        .KEGGdrug = xrefs.TryGetValue(kegg_drug).DefaultFirst,
                        .KNApSAcK = xrefs.TryGetValue(KNApSAcK).DefaultFirst,
                        .lipidmaps = xrefs.TryGetValue(lipidmaps).DefaultFirst,
                        .MeSH = xrefs.TryGetValue(mesh).DefaultFirst,
                        .Wikipedia = xrefs.TryGetValue(Wikipedia).DefaultFirst,
                        .pubchem = xrefs.TryGetValue(PubChem).DefaultFirst,
                        .metlin = xrefs.TryGetValue(metlin).DefaultFirst,
                        .MetaCyc = xrefs.TryGetValue(biocyc).DefaultFirst,
                        .SMILES = registry.sequence_graph _
                            .where(field("molecule_id") = metabolite.id) _
                            .find(Of biocad_registryModel.sequence_graph) _
                           ?.sequence
                    }
                }

                Call list.add(cad_id, metab)
            Next
        Next

        ' mapping the spectrum reference id to the metabolite reference id
        Call list.setAttribute("mapping", New list(mapping))

        Return list
    End Function
End Module
