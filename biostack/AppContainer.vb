Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Language

Module AppContainer

    ReadOnly defines As New Dictionary(Of GCModeller.Apps, Type)

    Sub New()
        Call LoadApps()
    End Sub

    Private Sub LoadApps()
        Dim types = GetType(AppContainer) _
            .Assembly _
            .GetTypes _
            .Where(Function(type)
                       Return type.ImplementInterface(GetType(IBiostackApp))
                   End Function) _
            .ToArray

        For Each type As Type In types
            Dim entry As AppEntry = type.GetAttribute(Of AppEntry)

            If Not entry Is Nothing Then
                Call defines.Add(entry.AppID, type)
            End If
        Next
    End Sub

    Public Function GetApp(app As Long) As IBiostackApp
        With DirectCast(app, GCModeller.Apps)
            If defines.ContainsKey(.ByRef) Then
                Return Activator.CreateInstance(defines(.ByRef))
            Else
                Throw New NotImplementedException(.ByRef)
            End If
        End With
    End Function
End Module
