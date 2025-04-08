Imports System.ComponentModel
Imports RegistryTool.My

Public Class FormSettings

    Private Sub FormSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        PropertyGrid1.SelectedObject = Settings.Load
    End Sub

    Private Sub PropertyGrid1_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles PropertyGrid1.PropertyValueChanged
        Call DirectCast(PropertyGrid1.SelectedObject, Settings).Save()
    End Sub

    Private Sub FormSettings_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Call DirectCast(PropertyGrid1.SelectedObject, Settings).Save()
        Call MyApplication.Load()
    End Sub
End Class