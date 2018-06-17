Public Interface IBiostackApp

    ReadOnly Property AppID As GCModeller.Apps

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="argumentJSON$"></param>
    ''' <param name="workspace$"></param>
    ''' <returns></returns>
    Function RunApp(argumentJSON$, workspace$) As Exception

End Interface

<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public Class AppEntry : Inherits Attribute

    Public ReadOnly Property AppID As GCModeller.Apps

    Sub New(appID As GCModeller.Apps)
        Me.AppID = appID
    End Sub

    Public Overrides Function ToString() As String
        Return AppID.Description
    End Function
End Class