#Region "Microsoft.VisualBasic::97343f03c7880de85dc0e766053e9e25, ..\GCModeller\MailServices\services.vb"

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

Imports Microsoft.VisualBasic.Net.Mailto

''' <summary>
''' services@gcmodeller.org
''' </summary>
Module MailServices

    Const AppKey As String = "nhchgmjmsqbrdcbc"

    Public Function GetClient() As EMailClient
        Return Net.Mailto.EMailClient.QQMail("analysis-services@gcmodeller.org", AppKey)
    End Function
End Module

