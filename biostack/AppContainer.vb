Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Language
Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Expressions

Module AppContainer

    Public ReadOnly defines As New Dictionary(Of GCModeller.Apps, Type)
    Public ReadOnly repository$

    Sub New()
        Call LoadApps()

        repository = App.GetVariable("repository")
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

    Public Function GetGCModeller() As String
        Dim appPath$ = App.GetVariable("GCModeller")

        If appPath.DirectoryExists Then
            Return appPath
        Else
            Throw New EntryPointNotFoundException("Missing GCModeller tools!")
        End If
    End Function

    <Extension>
    Public Function GetWorkspace(mysqli As MySqli, taskID$) As String
        Dim task = New Table(Of MySql.bioCAD.task)(mysqli) _
            .Where($"`id` = '{taskID}' OR `sha1` = '{taskID}'") _
            .Find

        If task Is Nothing Then
            Return Nothing
        Else
            Return $"/upload/{task.user_id}/{task.app_id}/{task.id}/"
        End If
    End Function
End Module
