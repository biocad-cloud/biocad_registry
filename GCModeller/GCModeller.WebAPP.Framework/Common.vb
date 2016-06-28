#Region "Microsoft.VisualBasic::12d4c7cc33215f66feb1a284d3423f7a, ..\GCModeller\GCModeller.WebAPP.Framework\Common.vb"

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

Imports System.Text.RegularExpressions

Public Module Common

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="memeText">The text file content, not the path</param>
    ''' <returns></returns>
    Public Function getMEMEQueryName(memeText As String) As String
        Dim file As String = Regex.Match(memeText, "DATAFILE=.+?ALPHABET=", RegexOptions.IgnoreCase Or RegexOptions.Singleline).Value
        file = Mid(file, 10).Trim
        file = Mid(file, 1, Len(file) - 9).Trim
        file = IO.Path.GetFileNameWithoutExtension(file)
        Return file
    End Function
End Module

