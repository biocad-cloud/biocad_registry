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

        edit.MdiParent = MyApplication.host
        edit.Text = $"BioCAD{id.PadLeft(11, "0"c)} - {name}"
        edit.Show()
    End Sub
End Module
