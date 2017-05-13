Imports System.IO.Compression
Imports Microsoft.VisualBasic.Language
Imports SMRUCC.genomics.Interops.NCBI.Extensions.LocalBLAST
Imports SMRUCC.WebCloud.HTTPInternal.Platform

Public Class COGMyva : Inherits TaskModel

    Dim query$, outZIP$

    Sub New(fasta$)
        query = fasta
        outZIP = query.TrimSuffix & "_myva.zip"
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

        Call Apps.localblast.ExportBBHLarge(qvs, qvsTable)
        Call Apps.localblast.ExportBBHLarge(svq, svqTable)

        ' 使用bbh方法进行COG注释
        current = ++i

        Dim bbh$ = qvs.TrimSuffix & "__vs_" & svq.BaseName & ".bbh.csv"
        Call Apps.localblast.BBHExport2(qvsTable, svqTable, _out:=bbh)

        Dim MyvaCOG$ = bbh.TrimSuffix & ".myvaCOG.csv"
        Call Apps.localblast.COG_myva(bbh, GCModeller.FileSystem.COGs & "/Myva/Whog.XML", MyvaCOG)

        ' 进行一些简单的统计分析以及绘图可视化
        current = ++i

        Dim statices = MyvaCOG.TrimSuffix & ".profiling.csv"
        Dim plot$ = MyvaCOG.TrimSuffix & ".plot.png"
        Call Apps.localblast.COGStatics(MyvaCOG, _out:=statices)
        Call Apps.eggHTS.COGCatalogProfilingPlot(MyvaCOG, _out:=plot)

        ' 进行注释报告的结果的打包操作
        current = ++i

        Dim out$() = {
            svq, qvs, qvsTable, svqTable, bbh, MyvaCOG, statices, plot
        }
        Call GZip.AddToArchive(outZIP, out, ArchiveAction.Replace, Overwrite.Always, CompressionLevel.Fastest)
    End Sub

    Protected Overrides Function contents() As String()
        Return {
            "Blast+ myva database search",
            "Export blastp table",
            "COG annotation",
            "Data Plot",
            "Report packing"
        }
    End Function
End Class
