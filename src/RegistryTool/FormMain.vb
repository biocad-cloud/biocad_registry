Imports biocad_registry
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports Microsoft.VisualBasic.Language
Imports RegistryTool.My
Imports Metadata = BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaLib

Public Class FormMain

    Private Sub VocabularyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VocabularyToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.LoadTableView(Function() MyApplication.biocad_registry.vocabulary.select(Of biocad_registryModel.vocabulary)("*"))
        view.MdiParent = Me
        view.Text = "`biocad_registry`.`vocabulary`"
        view.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        App.Exit(0)
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MyApplication.Load Then

        End If
    End Sub

    Private Sub SubCellularCompartmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SubCellularCompartmentsToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.LoadTableView(Function() MyApplication.biocad_registry.subcellular_compartments.select(Of biocad_registryModel.subcellular_compartments)("*"))
        view.MdiParent = Me
        view.Text = "`biocad_registry`.`subcellular_compartments`"
        view.Show()
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Call New FormSettings().ShowDialog()
    End Sub

    Private Sub MoleculesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoleculesToolStripMenuItem.Click
        Dim view As New FormMoleculeTable
        view.MdiParent = Me
        view.Show()
    End Sub

    Private Sub ExportMetabolitesDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportMetabolitesDatabaseToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "Metabolite Annotation Database(*.dat)|*.dat"}
            If file.ShowDialog = DialogResult.OK Then
                Call MyApplication.Loading(
                    Function(println)
                        Return ExportLocal(println, file.FileName)
                    End Function)
                Call MessageBox.Show("Export metabolite local annotation repository database success!", "Task Finish", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Shared Function ExportLocal(println As Action(Of String), filename As String) As Boolean
        Dim i As Integer = 0
        Dim repo As New RepositoryWriter(filename.Open(IO.FileMode.OpenOrCreate, doClear:=True))
        Dim block As i32 = 1

        Call println("Export metabolite annotation into local repository...")

        For Each mol As Metadata In MetaboliteAnnotations.ExportAnnotation
            If i > 2000 Then
                i = 0
                repo.CommitBlock()
                println($"commit block data: {++block}")
            Else
                i += 1
                repo.Add(mol)
            End If
        Next

        Call repo.Dispose()

        Return True
    End Function
End Class
