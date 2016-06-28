#Region "Microsoft.VisualBasic::adf0696ec96e34e4bb6311a646b8fccd, ..\GCModeller\GCModeller\Drawing\SeqLogoTask.vb"

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

Namespace Drawing

    Public Class SeqLogoTask : Inherits TaskCallback

        Public Property outDIR As String

        Sub New(app As String, args As String)
            Call MyBase.New(app, args)
        End Sub

        Public Overrides Sub Callback()

        End Sub
    End Class
End Namespace
