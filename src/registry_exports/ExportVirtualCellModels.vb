Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.GCModeller.ModellingEngine.Model
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Class ExportVirtualCellModels

    ReadOnly registry As biocad_registry
    ReadOnly repo As String
    ReadOnly vocabulary As biocad_vocabulary

    Sub New(registry As biocad_registry, repo As String)
        Me.vocabulary = registry.biocad_vocabulary
        Me.repo = repo
        Me.registry = registry
    End Sub

    Public Sub ExportEnzymeDb()
        Using text As New StreamWriter($"{repo}/ec_numbers.fasta".Open(IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Call text.Add(registry.ExportEnzyme, filterEmpty:=True)
        End Using
    End Sub

    Public Sub ExportSubcellularLocationDb()
        Using text As New StreamWriter($"{repo}/subcellular.fasta".Open(IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Call text.Add(registry.ExportCellularLocation, filterEmpty:=True)
        End Using
    End Sub

    Public Sub ExportReactionPool()
        Dim hashset As String() = registry.reaction _
            .where(field("hashcode").char_length > 0) _
            .distinct _
            .project(Of String)("hashcode")
        Dim role_left As UInteger = vocabulary.GetVocabulary("Metabolic Role", "Substrate").id
        Dim role_right As UInteger = vocabulary.GetVocabulary("Metabolic Role", "Product").id
        Dim ec_type As UInteger = vocabulary.db_ECNumber
        Dim reaction_type As UInteger = vocabulary.reaction_type

        Using json As New IO.StreamWriter($"{repo}/metabolic_network.jsonl".Open(IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
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
