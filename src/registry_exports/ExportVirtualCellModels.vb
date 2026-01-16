Imports registry_data
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

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

        For Each hashcode As String In hashset
            Dim rxn As reaction = registry.reaction _
                .where(field("hashcode") = hashcode) _
                .order_by("id") _
                .find(Of reaction)
            Dim species = registry.metabolic_network _
                .where(field("reaction_id") = rxn.id,
                       field("role").in({role_left, role_right})) _
                .select(Of metabolic_network)
            Dim left = species.Where(Function(c) c.role = role_left).ToArray
            Dim right = species.Where(Function(c) c.role = role_right).ToArray

            If left.IsNullOrEmpty OrElse
                left.Any(Function(a) a.species_id = 0) OrElse
                right.IsNullOrEmpty OrElse
                right.Any(Function(a) a.species_id = 0) Then

                Continue For
            End If


        Next
    End Sub

End Class
