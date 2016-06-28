#Region "Microsoft.VisualBasic::8e7701343ce87a0e1f4a75c71296e970, ..\GCModeller\MEME\TomQuery\SWTomTask.vb"

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

Imports GCModeller.WebAPP.Framework
Imports LANS.SystemsBiology.GCModeller.Workbench
Imports Microsoft.VisualBasic

Namespace TomQuery

    Public Class SWTomTask : Inherits TaskCallback

        Public out As String

        Sub New(app As String, args As String)
            Call MyBase.New(app, args)
        End Sub

        Public Overrides Sub Callback()
            If String.IsNullOrEmpty(JobTitle) Then
                JobTitle = uid
            End If

            Dim result As List(Of String) = FileIO.FileSystem.GetFiles(out, FileIO.SearchOption.SearchAllSubDirectories, "*.zip").ToList
            Dim msg As New Net.Mailto.MailContents With {
                 .Attatchments = result,
                 .Subject = "Motif TomQuery Job Complete!",
                 .Body = ReportBuilder.EMailMsg.GetMessage(
                     "TomQuery Job Complete",
                    $"Your Motif TomQuery submit task ""<strong>{JobTitle}</strong>"" completed! The result data archive has been send to you as the attachments in this E-Mail, and you can click the link in this mail message to make the further analysis.<br /> 
                                                             Your result data will store on the server cache temporary in next 7 days.",
                     EMail.Split("@"c).First.Replace(".", " "),
                     $"http://services.gcmodeller.org/tom.query/swtom.query.show_result.vb?uid={uid}",
                     "Click View Result")
            }
            Dim mail As Net.Mailto.EMailClient = MailServices.GetClient
            Call mail.SendEMail(msg, "analysis-services@gcmodeller.org", EMail)
            Call mail.ErrMessage.__DEBUG_ECHO
        End Sub
    End Class
End Namespace
