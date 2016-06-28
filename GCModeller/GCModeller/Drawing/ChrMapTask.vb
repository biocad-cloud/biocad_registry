#Region "Microsoft.VisualBasic::cc5e98068f02d0881caf72637eefbc47, ..\GCModeller\GCModeller\Drawing\ChrMapTask.vb"

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
Imports Microsoft.VisualBasic.Net.Mailto
Imports Microsoft.VisualBasic

Namespace Drawing

    Public Class ChrMapTask : Inherits TaskCallback

        Public Property outDIR As String

        Sub New(app As String, args As String)
            Call MyBase.New(app, args)
        End Sub

        Public Overrides Sub Callback()
            If String.IsNullOrEmpty(JobTitle) Then
                JobTitle = uid
            End If

            Call GZip.DirectoryArchive(outDIR, outDIR & "/chr_Map.zip", ArchiveAction.Replace, Overwrite.Always, Compression.CompressionLevel.Fastest)

            Dim result As List(Of String) = FileIO.FileSystem.GetFiles(outDIR, FileIO.SearchOption.SearchAllSubDirectories, "*.zip").ToList
            Dim msg As New Net.Mailto.MailContents With {
                .Attatchments = result,
                .Subject = "Map Drawing Complete!",
                .Body = ReportBuilder.EMailMsg.GetMessage(
                    "Map Drawing Complete",
                    $"Your chromosomes map drawing task submit ""<strong>{JobTitle}</strong>"" completed! 
                    The result data archive has been send to you as the attachments in this E-Mail.<br /> 
   Your result data will store on the server cache temporary in next 7 days.",
                    EMail.Split("@"c).First.Replace(".", " "),
                    $"http://services.gcmodeller.org/tom.query/swtom.query.show_result.vb?uid={uid}",
                    "Click View Result")
            }
            Dim mail As EMailClient = MailServices.GetClient
            Call mail.SendEMail(msg, "analysis-services@gcmodeller.org", EMail)
            Call mail.ErrMessage.__DEBUG_ECHO
        End Sub
    End Class
End Namespace
