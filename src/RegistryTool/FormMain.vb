Imports biocad_registry
Imports RegistryTool.My

Public Class FormMain

    Private Sub VocabularyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VocabularyToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.LoadTableView(Function() MyApplication.biocad_registry.vocabulary.select(Of biocad_registryModel.vocabulary)("*"))
        view.Show(Me)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        App.Exit(0)
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MyApplication.Load Then

        End If
    End Sub
End Class
