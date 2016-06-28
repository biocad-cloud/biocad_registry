#Region "Microsoft.VisualBasic::d2db7347b21fbdb69fc6b20ae7897a76, ..\GCModeller\GCModeller\Drawing\Drawing.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports System.IO
Imports GCModeller.WebAPP.Framework
Imports LANS.SystemsBiology.GCModeller.Workbench
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Net.Mailto
Imports SMRUCC.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.HTTPInternal.AppEngine.POSTParser
Imports SMRUCC.HTTPInternal.Platform

Namespace Drawing

    ''' <summary>
    ''' GCModeller 绘图服务
    ''' </summary>
    <[Namespace]("drawing")>
    Public Class API : Inherits WebAPP.Framework.WebAPP

        Sub New(main As PlatformEngine)
            Call MyBase.New(main)
            Call Settings.Initialize(GetType(API))
            Call PlatformEngine.AddMappings(Settings.DataCache, "/drawing/cache/")
        End Sub

        <ExportAPI("/drawing/chr_map.run_task.vb"), [POST](GetType(String))>
        Public Function ChrMapRunTask(args As String, inputs As PostReader) As String
            Dim tmp As String = App.GetAppSysTempFile
            Dim ptt As String = tmp & "/g.ptt"
            Dim rnt As String = tmp & "/g.rnt"
            Dim cog As String = tmp & "/g_cogs.csv"

            '   Call data.GetFile("ptt")?.content.SaveTo(ptt)
            ' Call data.GetFile("rnt")?.content.SaveTo(rnt)
            '  Call data.GetFile("cog")?.content.SaveTo(cog)

            Dim uid As String = Now.ToBinary
            Dim out As String = Settings.DataCache & "/" & uid

            args = $"--Drawing.ChromosomeMap /ptt {ptt.CliPath} /conf {(App.HOME & "/GCModeller/Settings/chr_map.inf").CliPath} /out {out.CliPath} /COG {cog.CliPath}"

            Dim task As New ChrMapTask(App.HOME & "/GCModeller/GCModeller.exe", args) With {
                .outDIR = out,
                .EMail = inputs.Form("email"),
                .JobTitle = inputs.Form("job_title"),
                .ResultPage = $"/drawing/chr_map.show_map.vb?uid={uid}",
                .uid = uid
            }

            Return __joinTask(task)
        End Function

        <ExportAPI("/drawing/chr_map.show_map.vb"), [GET](GetType(String))>
        Public Function ChrMapShowMap(args As String) As String
            Dim arg As Dictionary(Of String, String) = args.postRequestParser
            Dim uid As String = arg("uid")

        End Function

        <ExportAPI("/drawing/seq_logo.run_task.vb"), POST(GetType(String))>
        Public Function SeqLogoSubmits(args As String, inputs As PostReader) As String
            Dim tmp As String = App.GetAppSysTempFile
            Dim File As HttpPostedFile = inputs.Files("memeText")
            Dim fileData As String '= (From x As Content In data
            'Where String.Equals(x.Name, "memeText", StringComparison.OrdinalIgnoreCase) AndAlso
            'String.IsNullOrEmpty(x.FileName)
            'Select Case x.content).FirstOrDefault
            If String.IsNullOrEmpty(fileData) OrElse String.Equals(fileData, vbLf) Then
                '   fileData = File.content
            End If

            Dim name As String = getMEMEQueryName(fileData)
            Dim uid As String = Now.ToBinary & "." & name
            Dim out As String = Settings.DataCache & "/" & uid

            tmp = $"{tmp}/{name}.txt"
            args = $"/seq.logo /in {tmp.CliPath} /out {out.CliPath}"

            Dim task As New SeqLogoTask(App.HOME & "/GCModeller/meme.exe", args) With {
                .EMail = inputs.Form("email"),
                .JobTitle = inputs.Form("job_title"),
                .outDIR = out,
                .uid = uid,
                .ResultPage = $"/drawing/seq_logo.show.vb?uid=" & uid
            }

            Return __joinTask(task)
        End Function

        <ExportAPI("/drawing/seq_logo.show.vb"), [GET](GetType(String))>
        Public Function ShowLogo(args As String) As String
            Dim params As Dictionary(Of String, String) = args.requestParser
            Dim uid As String = params("uid")
        End Function
    End Class
End Namespace
