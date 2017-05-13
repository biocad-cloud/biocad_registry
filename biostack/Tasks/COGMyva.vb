Imports Microsoft.VisualBasic.Language
Imports SMRUCC.genomics.Interops.NCBI.Extensions.LocalBLAST
Imports SMRUCC.WebCloud.HTTPInternal.Platform

Public Class COGMyva : Inherits TaskModel

    Dim query$

    Sub New(fasta$)
        query = fasta
    End Sub

    ''' <summary>
    ''' myva数据库是事先已经格式化好了的
    ''' </summary>
    Public Overrides Sub RunTask()
        Dim localblast As New Programs.BLASTPlus(bin:=GCModeller.FileSystem.GetLocalblast)
        Dim myva$ = GCModeller.FileSystem.COGs & "/Myva/myva"
        Dim qvs$ = query.TrimSuffix & "_vs_myva.txt"
        Dim svq$ = query.TrimSuffix & "_vs_myva_reverse.txt"
        Dim i As int = Scan0

        ' 数据库搜索
        current = ++i

        Call localblast.FormatDb(query, localblast.MolTypeProtein).Run()
        Call localblast.Blastp(query, myva, qvs).Run()
        Call localblast.Blastp(myva, query, svq).Run()

        ' 导出sbh数据
        current = ++i

        Dim qvsTable$ = qvs.TrimSuffix & ".sbh.csv"
        Dim svqTable$ = svq.TrimSuffix & ".sbh.csv"

        Call Apps.localblast.SBH_Export_Large(qvs, qvsTable)
        Call Apps.localblast.SBH_Export_Large(svq, svqTable)

        ' 使用bbh方法进行COG注释
        current = ++i

        Dim bbh$ = qvs.TrimSuffix & "__vs_" & svq.BaseName & ".bbh.csv"
        Call Apps.localblast.sbh2bbh(qvsTable, svqTable, _out:=bbh)

        Dim MyvaCOG$ =  bbh .TrimSuffix & ".myvaCOG.csv"

    End Sub

    Protected Overrides Function contents() As String()
        Return {
            "Blast+ myva database search",
            "Export blastp table",
            "COG annotation",
            "Data Plot"
        }
    End Function
End Class
