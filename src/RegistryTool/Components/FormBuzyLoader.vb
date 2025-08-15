Imports System.Runtime.CompilerServices
Imports System.Threading

Public Class FormBuzyLoader

    Public Class BuzyTask(Of T)

        Public ReadOnly loader As New FormBuzyLoader
        Public task As Func(Of Action(Of String), T)
        Public result As T

        Public Sub Run()
            Call Thread.Sleep(300)

            ' run background task
            result = task(AddressOf loader.Println)
            loader.Invoke(Sub() loader.Close())
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub ShowDialog()
            Call loader.ShowDialog()
        End Sub
    End Class

    Private Sub Println(s As String)
        Call Me.Invoke(Sub() TextBox1.Text = s)
    End Sub

    Public Shared Function Loading(Of T)(getter As Func(Of Action(Of String), T)) As T
        Dim loader As New BuzyTask(Of T) With {.task = getter}
        Dim runner As New Thread(AddressOf loader.Run)

        Call runner.Start()
        Call loader.ShowDialog()

        Return loader.result
    End Function

    Public Shared Sub Loading(task As Action(Of Action(Of String)))
        Dim getter As Func(Of Action(Of String), Boolean) =
            Function(println) As Boolean
                Call task(println)
                Return True
            End Function
        Dim loader As New BuzyTask(Of Boolean) With {.task = getter}
        Dim runner As New Thread(AddressOf loader.Run)

        Call runner.Start()
        Call loader.ShowDialog()
    End Sub

End Class