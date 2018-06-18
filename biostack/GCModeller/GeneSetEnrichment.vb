Imports System.IO.Compression
Imports biostack.GCModellerApps
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Serialization.JSON

<AppEntry(GCModeller.Apps.Enrichment)>
Public Class GeneSetEnrichment : Implements IBiostackApp

    Public ReadOnly Property AppID As GCModeller.Apps Implements IBiostackApp.AppID
        Get
            Return GCModeller.Apps.Enrichment
        End Get
    End Property

    ReadOnly profiler As Profiler
    ReadOnly eggHTS As eggHTS
    ReadOnly repository$

    Sub New()
        profiler = New Profiler(AppContainer.GetGCModeller & "/Profiler.exe")
        eggHTS = New eggHTS(AppContainer.GetGCModeller & "/eggHTS.exe")
        repository = App.GetVariable("repository") & "/backgrounds/"
    End Sub

    Public Function RunApp(argumentJSON$, workspace$) As Exception Implements IBiostackApp.RunApp
        Dim args = argumentJSON.LoadObject(Of Dictionary(Of String, String))
        Dim organism$ = args!organism
        Dim orgName$ = args!organismName
        Dim geneSet$ = workspace & "/geneSet.txt"
        Dim background$ = $"{repository}/{organism}"
        Dim uniprot$ = $"{background}/uniprot.XML"
        Dim KEGGOut$ = workspace & "/enrichment_KO.csv"
        Dim GOOut$ = workspace & "/enrichment_GO.csv"
        Dim converts$ = workspace & "/converts_UniProt.txt"

        ' 先进行编号的类型的统一转换
        ' 然后再使用转换过后的编号进行富集计算分析
        Call profiler.IDconverts(uniprot, geneSet, out:=converts)

        ' kegg
        Call profiler.EnrichmentTest(background & "/KO_background.XML", converts, KEGGOut, hide_progress:=True)
        ' go
        Call profiler.EnrichmentTest(background & "/GO_background.XML", converts, GOOut, hide_progress:=True)

        ' 合并，方便显示于结果页面之上
        Dim all$ = workspace & "/enrichment[1].csv"
        Dim zip$ = workspace & "/result.zip"

        Call DocumentExtensions.DirectAppends(
            {KEGGOut, GOOut},
            all,
            orderBy:=Function(r)
                         Return Val(r!pvalue)
                     End Function)

        ' 分别进行绘图
        ' ko
        ' go

        ' zip 打包下载备用
        Dim packageFiles$() = workspace _
            .EnumerateFiles("*.*") _
            .Where(Function(file)
                       Return Not file.ExtensionSuffix.TextEquals("zip")
                   End Function) _
            .ToArray

        Call GZip.AddToArchive(
            packageFiles, zip,
            ArchiveAction.Replace,
            Overwrite.Always,
            CompressionLevel.Optimal
        )

        Return Nothing
    End Function
End Class
