Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.GCModeller.ModellingEngine.Model
Imports SMRUCC.genomics.SequenceModel.FASTA

''' <summary>
''' Make exports of the component models for run virtual cell annotations
''' </summary>
Public Class ExportVirtualCellModels

    ReadOnly registry As biocad_registry
    ReadOnly repo As String
    ReadOnly vocabulary As biocad_vocabulary

    Sub New(registry As biocad_registry, repo As String)
        Me.vocabulary = registry.biocad_vocabulary
        Me.repo = repo
        Me.registry = registry
    End Sub

    Public Sub ExportLocations()
        Dim locs = registry.registry_resolver.where(field("type") = vocabulary.GetRegistryEntity(biocad_vocabulary.EntitySubcellularLocation).id).select(Of registry_resolver)
        Dim models As WebJSON.CellularLocation() = locs _
            .SafeQuery _
            .Select(Function(cc)
                        Return New WebJSON.CellularLocation With {
                            .id = cc.symbol_id,
                            .symbol = cc.register_name
                        }
                    End Function) _
            .ToArray

        Call models.GetJson.SaveTo($"{repo}/subcellular.json")
    End Sub

    Public Sub ExportEnzymeDb()
        Using text As New StreamWriter($"{repo}/ec_numbers.fasta".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Call text.Add(registry.ExportEnzyme, filterEmpty:=True)
        End Using
    End Sub

    Public Sub ExportSubcellularLocationDb()
        Using text As New StreamWriter($"{repo}/subcellular.fasta".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Call text.Add(registry.ExportCellularLocation, filterEmpty:=True)
        End Using
    End Sub

    Public Sub ExportMoleculeData()
        Using jsonl As New System.IO.StreamReader($"{repo}/metabolic_network.jsonl".Open(System.IO.FileMode.Open, doClear:=False, [readOnly]:=True))
            Dim json_str As Value(Of String) = ""
            Dim mols As New Dictionary(Of String, metabolites)

            Using molecules As New System.IO.StreamWriter($"{repo}/molecules.jsonl".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
                Do While (json_str = jsonl.ReadLine) IsNot Nothing
                    Dim model As WebJSON.Reaction = json_str.LoadJSON(Of WebJSON.Reaction)

                    For Each cid As UInteger In From c As WebJSON.Substrate
                                                In model.left.JoinIterates(model.right)
                                                Select c.molecule_id
                                                Distinct

                        Call mols.ComputeIfAbsent(cid.ToString, Function(id) MakeQueryMetabolite(id, molecules))
                    Next
                Loop
            End Using

            ' export table
            Call mols.Values.SaveTo($"{repo}/molecules.csv")
        End Using
    End Sub

    Private Function MakeQueryMetabolite(id As UInteger, jsonl As System.IO.StreamWriter) As metabolites
        Dim metabolite_type As UInteger = vocabulary.metabolite_type
        Dim meta As metabolites = registry.metabolites.where(field("id") = id).find(Of metabolites)

        If meta Is Nothing Then
            Return Nothing
        End If

        Dim xrefs As New List(Of WebJSON.DBXref)
        Dim symbol As registry_resolver = registry.registry_resolver _
            .where(field("symbol_id") = meta.id,
                   field("type") = metabolite_type) _
            .find(Of registry_resolver)

        If meta.pubchem_cid > 0 Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "PubChem", .xref_id = "PubChem:" & meta.pubchem_cid})
        If meta.chebi_id > 0 Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "ChEBI", .xref_id = "ChEBI:" & meta.chebi_id})

        If Not meta.cas_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "CAS", .xref_id = meta.cas_id})
        If Not meta.kegg_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "KEGG", .xref_id = meta.kegg_id})
        If Not meta.hmdb_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "HMDB", .xref_id = meta.hmdb_id})
        If Not meta.lipidmaps_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "LipidMaps", .xref_id = meta.lipidmaps_id})
        If Not meta.biocyc.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "BioCyc", .xref_id = meta.biocyc})
        If Not meta.mesh_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "MeSH", .xref_id = meta.mesh_id})
        If Not meta.wikipedia.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "Wikipedia", .xref_id = meta.wikipedia})

        Dim model As New WebJSON.Molecule With {
            .id = "BioCAD" & id.ToString.PadLeft(11, "0"c),
            .name = meta.name,
            .formula = meta.formula,
            .db_xrefs = xrefs.ToArray,
            .symbol = If(symbol Is Nothing, Nothing, symbol.register_name)
        }

        meta.note = model.symbol

        Call jsonl.WriteLine(model.GetJson)

        Return meta
    End Function

    Public Sub ExportReactionPool()
        Dim hashset As String() = registry.reaction _
            .where(field("hashcode").char_length > 0) _
            .distinct _
            .project(Of String)("hashcode")
        Dim role_left As UInteger = registry.MetabolicSubstrateRole.id
        Dim role_right As UInteger = registry.MetabolicProductRole.id
        Dim ec_type As UInteger = vocabulary.db_ECNumber
        Dim reaction_type As UInteger = vocabulary.reaction_type

        Using json As New System.IO.StreamWriter($"{repo}/metabolic_network.jsonl".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            For Each hashcode As String In TqdmWrapper.Wrap(hashset)
                Dim rxn As reaction = registry.reaction _
                    .where(field("hashcode") = hashcode) _
                    .order_by("id") _
                    .find(Of reaction)
                Dim species = registry.metabolic_network _
                    .where(field("reaction_id") = rxn.id,
                           field("role").in({role_left, role_right})) _
                    .select(Of metabolic_network)
                Dim left = castModel(From c As metabolic_network In species Where c.role = role_left).ToArray
                Dim right = castModel(From c As metabolic_network In species Where c.role = role_right).ToArray

                If left.IsNullOrEmpty OrElse
                    left.Any(Function(a) a.molecule_id = 0) OrElse
                    right.IsNullOrEmpty OrElse
                    right.Any(Function(a) a.molecule_id = 0) Then

                    Continue For
                End If

                Dim ec_number As String() = registry.db_xrefs _
                    .where(field("type") = reaction_type,
                           field("db_name") = ec_type,
                           field("obj_id") = rxn.id) _
                    .distinct _
                    .project(Of String)("db_xref")

                Dim model As New WebJSON.Reaction With {
                    .guid = hashcode,
                    .name = rxn.name,
                    .reaction = rxn.equation,
                    .left = left,
                    .right = right,
                    .law = laws(ec_number, .left).ToArray
                }

                Call json.WriteLine(model.GetJson)
            Next
        End Using
    End Sub

    Private Iterator Function laws(ec_number As IEnumerable(Of String), left As WebJSON.Substrate()) As IEnumerable(Of WebJSON.LawData)
        For Each ec As String In ec_number.SafeQuery
            Yield New WebJSON.LawData With {
                .metabolite_id = "BioCAD" & left(0).molecule_id.ToString.PadLeft(11, "0"c),
                .lambda = "(Vmax * S)/(Km + S)",
                .params = New Dictionary(Of String, String) From {
                    {"Km", 1},
                    {"Vmax", 100},
                    {"S", .metabolite_id}
                },
                .ec_number = ec
            }
        Next
    End Function

    Private Iterator Function castModel(nodes As IEnumerable(Of metabolic_network)) As IEnumerable(Of WebJSON.Substrate)
        For Each c As metabolic_network In nodes
            Yield New WebJSON.Substrate With {
                .factor = c.factor,
                .location = c.compartment_id,
                .molecule_id = c.species_id
            }
        Next
    End Function

End Class
