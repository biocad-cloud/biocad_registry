Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
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
                Dim left = registry.reaction_graph.where(field("reaction") = rxn.id, field("role") = left_key).project(Of UInteger)("molecule_id")
                Dim right = registry.reaction_graph.where(field("reaction") = rxn.id, field("role") = right_key).project(Of UInteger)("molecule_id")
                Dim left_hash = left.OrderBy(Function(id) id).JoinBy("+").MD5
                Dim right_hash = right.OrderBy(Function(id) id).JoinBy("+").MD5
                Dim enzymes = registry.regulation_graph.where(field("reaction_id") = rxn.id, field("role") = enzyme_role).select(Of biocad_registryModel.regulation_graph)
                Dim uniqecode As String

                ' clear unique hashcode of current object
                registry.hashcode.where(field("type_id") = reaction_type, field("obj_id") = rxn.id).delete()

                If enzymes.IsNullOrEmpty Then
                    uniqecode = {left_hash, right_hash}.OrderBy(Function(s) s).JoinBy("").MD5
                    registry.hashcode.add(field("type_id") = reaction_type, field("obj_id") = rxn.id, field("hashcode") = uniqecode)
                Else
                    For Each enzyme In enzymes
                        uniqecode = {left_hash, right_hash}.OrderBy(Function(s) s).JoinBy(enzyme.term).MD5
                        registry.hashcode.add(field("type_id") = reaction_type, field("obj_id") = rxn.id, field("hashcode") = uniqecode)
                    Next
                End If

                registry.reaction.where(field("id") = rxn.id).save(field("hashcode") = {left_hash, right_hash}.OrderBy(Function(s) s).JoinBy("").MD5)
            Next
        Next
    End Sub
End Module
