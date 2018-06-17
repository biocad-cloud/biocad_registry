Public Interface IBiostackApp

    ReadOnly Property AppID As GCModeller.Apps

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="argumentJSON$"></param>
    ''' <param name="workspace$"></param>
    ''' <returns></returns>
    Function RunApp(argumentJSON$, workspace$) As String

End Interface
