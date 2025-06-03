Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
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

    <ExportAPI("export_mona_metabolites")>
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

        For i As Integer = 0 To Integer.MaxValue
            Dim pagedata = registry.db_xrefs _
                .where(field("db_key") = mona) _
                .limit(i * page_size, page_size) _
                .select(Of biocad_registryModel.db_xrefs)

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            For Each db_xref As biocad_registryModel.db_xrefs In pagedata
                Dim cad_id As String = "BioCAD" & db_xref.obj_id.ToString.PadLeft(16, "0"c)

                If Not list.hasName(cad_id) Then
                    Dim metabolite As biocad_registryModel.molecule = registry.molecule.where(field("id") = db_xref.obj_id).find(Of biocad_registryModel.molecule)

                    If metabolite Is Nothing Then
                        Call $"missing metabolite of db_xref mapping: {cad_id} -> {db_xref.ToString}".Warning
                        Continue For
                    End If

                    Dim xrefs = registry.db_xrefs _
                        .where(field("db_key") <> mona, field("obj_id") = db_xref.obj_id) _
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
                        .synonym = registry.synonym.where(field("obj_id") = db_xref.obj_id).project(Of String)("synonym"),
                        .xref = New xref With {
                            .CAS = xrefs.TryGetValue(cas),
                            .chebi = xrefs.TryGetValue(chebi).DefaultFirst,
                            .ChEMBL = xrefs.TryGetValue()
                        }
                    }
                End If
            Next
        Next

        ' mapping the spectrum reference id to the metabolite reference id
        Call list.setAttribute("mapping", New list(mapping))

        Return list
    End Function
End Module
