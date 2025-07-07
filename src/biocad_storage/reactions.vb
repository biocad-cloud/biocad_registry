Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module reactions

    <Extension>
    Public Sub BuildUniqeHashCode(registry As biocad_registry, Optional page_size As Integer = 1000)
        Dim page_data As biocad_registryModel.reaction()
        Dim left_key = registry.getVocabulary("substrate", "Compound Role")
        Dim right_key = registry.getVocabulary("product", "Compound Role")

        For i As Integer = 0 To 100000
            page_data = registry.reaction _
                .limit(i * page_size, page_size) _
                .select(Of biocad_registryModel.reaction)

            For Each rxn As biocad_registryModel.reaction In TqdmWrapper.Wrap(page_data)
                Dim left = registry.reaction_graph.where(field("reaction") = rxn.id, field("role") = left_key).project(Of UInteger)("molecule_id")
                Dim right = registry.reaction_graph.where(field("reaction") = rxn.id, field("role") = right_key).project(Of UInteger)("molecule_id")
                Dim left_hash = left.OrderBy(Function(id) id).JoinBy("+").MD5
                Dim right_hash = right.OrderBy(Function(id) id).JoinBy("+").MD5
                Dim uniqecode = left_hash & right_hash

                registry.reaction.where(field("id") = rxn.id).save(field("hashcode") = uniqecode)
            Next
        Next
    End Sub
End Module
