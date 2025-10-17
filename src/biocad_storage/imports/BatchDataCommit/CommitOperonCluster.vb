Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Model.OperonMapper

Public Module CommitOperonCluster

    Public Sub CommitOperonSet(registry As biocad_registry, operons As IEnumerable(Of OperonRow))
        For Each page As OperonRow() In operons.Split(100)
            Call CommitOperonPage(registry, page)
        Next
    End Sub

    Private Sub CommitOperonPage(registry As biocad_registry, page As OperonRow())
        Dim clusters = registry.conserved_cluster
        Dim link = registry.cluster_link.open_transaction.ignore

        For Each tu As OperonRow In TqdmWrapper.Wrap(page)
            Dim check = clusters _
                .where(field("db_xref") = tu.koid,
                       field("tax_id") = tu.org) _
                .find(Of biocad_registryModel.conserved_cluster)

            If check Is Nothing Then
                clusters.add(
                    field("db_xref") = tu.koid,
                    field("tax_id") = tu.org,
                    field("name") = If(tu.name.StringEmpty(), tu.op.JoinBy(","), tu.name),
                    field("size") = tu.op.TryCount,
                    field("description") = tu.definition
                )
                check = clusters _
                    .where(field("db_xref") = tu.koid,
                           field("tax_id") = tu.org) _
                    .find(Of biocad_registryModel.conserved_cluster)
            ElseIf check.name.StringEmpty Then
                clusters.where(field("id") = check.id).save(field("name") = tu.op.JoinBy(","))
            End If
            If check Is Nothing Then
                Continue For
            End If

            For Each gene_id As String In tu.op.SafeQuery
                Dim gene = registry.molecule _
                    .where(field("tax_id") = tu.org,
                           field("xref_id") = $"{tu.org}:{gene_id}") _
                    .find(Of biocad_registryModel.molecule)

                If gene Is Nothing Then
                    Continue For
                End If

                If registry.cluster_link _
                    .where(field("cluster_id") = check.id, field("gene_id") = gene.id) _
                    .find(Of biocad_registryModel.cluster_link) Is Nothing Then

                    Call link.add(field("cluster_id") = check.id,
                                  field("gene_id") = gene.id)
                End If
            Next
        Next

        Call link.commit()
    End Sub

End Module
