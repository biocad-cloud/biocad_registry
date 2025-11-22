Imports RegistryTool.My

Module Workbench

    Public Sub StatusMessage(msg As String)
        Call MyApplication.host.Invoke(
            Sub()
                MyApplication.host.ToolStripStatusLabel1.Text = msg
            End Sub)
    End Sub

    Public Sub OpenMoleculeEditor(id As String, name As String)
        Dim edit As New FormMoleculeEditor With {.id = id}

        If id.IsPattern("\d+") OrElse id.IsPattern("BioCAD\d+") Then
            edit.MdiParent = MyApplication.host
            edit.Text = $"BioCAD{id.PadLeft(11, "0"c)} - {name}"
            edit.Show()
        Else
            Call MessageBox.Show($"the requested id '{id}' is not a valid registry number!",
                                 "Invalid input",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning)
        End If
    End Sub
End Module
