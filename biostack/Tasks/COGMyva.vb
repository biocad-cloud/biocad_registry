Imports Microsoft.VisualBasic.Language
Imports SMRUCC.genomics.Interops.NCBI.Extensions.LocalBLAST
Imports SMRUCC.WebCloud.HTTPInternal.Platform

Public Class COGMyva : Inherits TaskModel

    Dim query$

    Sub New(fasta$)

    End Sub

    Public Overrides Function GetTask() As Action
        Dim localblast As New Programs.BLASTPlus(bin:=GCModeller.FileSystem.GetLocalblast)
        Dim myva$ = GCModeller.FileSystem.COGs & "/Myva/myva"
        Dim qvs$ = query.TrimSuffix & "_vs_myva.txt"
        Dim svq$ = query.TrimSuffix & "_vs_myva_reverse.txt"
        Dim i As int = Scan0

        current = ++i

        Call localblast.Blastp(query, myva, qvs).Run()
        Call localblast.Blastp(myva, query, svq).Run()

        current = ++i



    End Function

    Protected Overrides Function contents() As String()
        Return {
            "Blast+ myva database search",
            "Export blastp table",
            "COG annotation",
            "Data Plot"
        }
    End Function
End Class
