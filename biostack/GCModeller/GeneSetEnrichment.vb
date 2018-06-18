Imports biostack.GCModellerApps
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

        ' kegg
        Call profiler.EnrichmentTest(background & "/KO_background.XML", geneSet, uniprot, KEGGOut)
        ' go
        Call profiler.EnrichmentTest(background & "/GO_background.XML", geneSet, uniprot, GOOut)

        ' 分别进行绘图
        ' ko
        ' go

        ' zip 打包下载备用


        Return Nothing
    End Function
End Class
