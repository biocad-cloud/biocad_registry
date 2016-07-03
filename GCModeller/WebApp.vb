Imports System.Text
Imports System.Text.RegularExpressions
Imports SMRUCC.HTTPInternal
Imports SMRUCC.HTTPInternal.Platform

''' <summary>
''' 在这里执行WebApp的初始化工作
''' </summary>
Public Module WebApp

    Sub Main(engine As PlatformEngine)
        Call engine.SetGetRequest(
            AddressOf New __mapHelper With {
                .engine = engine
            }.__requestStream
        )
    End Sub

    ReadOnly Template As String = My.Resources.index

    Private Structure __mapHelper

        Public engine As PlatformEngine

        Public Function __requestStream(ByRef res As String) As Byte()
            If Regex.Match(res, "\.html?$", RegexICMul).Success Then
                Dim url As String = engine.MapPath(res)
                Dim page As HtmlPage = HtmlPage.LoadPage(url, engine.HOME.FullName)
                Dim html As String = page.BuildPage(WebApp.Template)
                Dim bufs As Byte() = Encoding.UTF8.GetBytes(html)

                Return bufs
            Else
                Return engine.GetResource(res)
            End If
        End Function
    End Structure
End Module
