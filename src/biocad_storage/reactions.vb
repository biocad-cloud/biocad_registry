Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module reactions

    <Extension>
    Public Sub BuildUniqeHashCode(registry As biocad_registry, Optional page_size As Integer = 1000)
        Dim page_data As biocad_registryModel.reaction()
        Dim left_key = registry.getVocabulary("substrate", "Compound Role")
        Dim right_key = registry.getVocabulary("product", "Compound Role")
        Dim enzyme_role = registry.getVocabulary("Enzymatic Catalysis", "Regulation Type")
        Dim reaction_type = registry.getVocabulary("Reaction", "Entity Type")

        For i As Integer = 0 To 100000
            page_data = registry.reaction _
                .limit(i * page_size, page_size) _
                .select(Of biocad_registryModel.reaction)

            For Each rxn As biocad_registryModel.reaction In TqdmWrapper.Wrap(page_data)
                Dim left = registry.reaction_graph.where(field("reaction") = rxn.id, field("role") = left_key).select(Of biocad_registryModel.reaction_graph)
                Dim right = registry.reaction_graph.where(field("reaction") = rxn.id, field("role") = right_key).select(Of biocad_registryModel.reaction_graph)
                Dim left_hash = CheckSum(left)
                Dim right_hash = CheckSum(right)
                Dim enzymes = registry.regulation_graph _
                    .where(field("reaction_id") = rxn.id,
                           field("role") = enzyme_role) _
                    .select(Of biocad_registryModel.regulation_graph)
                Dim uniqecode As String

                ' clear unique hashcode of current object
                registry.hashcode.where(field("type_id") = reaction_type, field("obj_id") = rxn.id).delete()

                If enzymes.IsNullOrEmpty Then
                    uniqecode = {left_hash, right_hash}.OrderBy(Function(s) s).JoinBy("").MD5
                    registry.hashcode.add(field("type_id") = reaction_type, field("obj_id") = rxn.id, field("hashcode") = uniqecode)
                Else
                    For Each enzyme As biocad_registryModel.regulation_graph In enzymes
                        uniqecode = {left_hash, right_hash}.OrderBy(Function(s) s).JoinBy(enzyme.term).MD5
                        registry.hashcode.add(field("type_id") = reaction_type, field("obj_id") = rxn.id, field("hashcode") = uniqecode)
                    Next
                End If

                Call registry.reaction _
                    .where(field("id") = rxn.id) _
                    .save(field("hashcode") = New String() {left_hash, right_hash} _
                                                .OrderBy(Function(s) s) _
                                                .JoinBy("") _
                                                .MD5)
            Next
        Next
    End Sub

    Private Function CheckSum(compounds As biocad_registryModel.reaction_graph()) As String
        Dim sort = compounds.SafeQuery.OrderBy(Function(a) a.molecule_id).ToArray
        Dim data = sort.Select(Function(a) If(a.molecule_id = 0, a.db_xref, a.molecule_id.ToString)).JoinBy("+")
        Dim checksum_result = data.MD5
        Return checksum_result
    End Function
End Module
