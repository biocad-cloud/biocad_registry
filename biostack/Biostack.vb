Imports biostack.GCModellerApps
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform

''' <summary>
''' 小任务web api
''' </summary>
<[Namespace]("biostack")> Public Class Biostack : Inherits WebApp

    ReadOnly mysqli As New MySqli
    ReadOnly OSS_ROOT$
    ReadOnly eggHTS As eggHTS

    Public Sub New(main As PlatformEngine)
        Call MyBase.New(main)
        Call MySqliHelper.init(mysqli)

        OSS_ROOT = App.GetVariable("OSS_ROOT")
        eggHTS = New eggHTS(AppContainer.GetGCModeller & "/eggHTS.exe")

        If OSS_ROOT.DirectoryExists Then
            Call $"OSS_ROOT mounted at: {OSS_ROOT}".__INFO_ECHO
        Else
            Call $"Invalid OSS_ROOT location: {OSS_ROOT}".PrintException
            Throw New InvalidOperationException(OSS_ROOT)
        End If
    End Sub

    <ExportAPI("/biostack/CustomEnrichmentPlot.vbs")>
    <POST>
    Public Function CustomEnrichmentPlot(request As HttpPOSTRequest, response As HttpResponse) As Boolean
        Dim type$ = request.URLParameters("type")
        Dim id$ = request.URLParameters("id")
        Dim grayScale As Boolean = request.POSTData.Form("grayScale").ParseBoolean
        Dim label_align_right As Boolean = request.POSTData.Form("label_align_right").ParseBoolean
        Dim ticks As Double = request.POSTData.Form("ticks").ParseDouble
        Dim size As Double() = request.POSTData.Form("size").LoadObject(Of Double())
        Dim workspace$ = $"{OSS_ROOT}/{mysqli.GetWorkspace(taskID:=id)}"


        Return True
    End Function
End Class
