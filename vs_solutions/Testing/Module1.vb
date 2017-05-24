Module Module1

    Sub Main()
        Call TestCOGMyva()
    End Sub

    Sub TestCOGMyva()
        Dim task As New biostack.COGMyva("G:\biostack\vs_solutions\test_data\cog_myva\pXOCgx01-protein.fasta", Nothing)
        Call task .RunTask 
    End Sub
End Module
