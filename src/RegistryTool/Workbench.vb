Imports RegistryTool.My

Module Workbench

    Public Sub StatusMessage(msg As String)
        Call MyApplication.host.Invoke(
            Sub()
                MyApplication.host.ToolStripStatusLabel1.Text = msg
            End Sub)
    End Sub
End Module
