Imports Galaxy.Workbench
Imports Microsoft.VisualStudio.WinForms.Docking

Module Workbench

    Public Async Sub OpenMoleculeEditor(id As String, name As String)
        Dim edit As New FormMoleculeEditor

        If id.IsPattern("\d+") OrElse id.IsPattern("BioCAD\d+") Then
            If Await edit.LoadModel(id) Then
                edit.Text = $"BioCAD{id.PadLeft(11, "0"c)} - {name}"
                edit.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
            End If
        Else
            Call MessageBox.Show($"the requested id '{id}' is not a valid registry number!",
                                 "Invalid input",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning)
        End If
    End Sub
End Module
