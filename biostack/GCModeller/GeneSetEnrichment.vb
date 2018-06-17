Imports biostack.GCModellerApps
Imports Microsoft.VisualBasic.Serialization.JSON

<AppEntry(GCModeller.Apps.Enrichment)>
Public Class GeneSetEnrichment : Implements IBiostackApp

    Public ReadOnly Property AppID As GCModeller.Apps Implements IBiostackApp.AppID
        Get
            Return GCModeller.Apps.Enrichment
        End Get
    End Property

    ReadOnly eggHTS As eggHTS

    Sub New()
        eggHTS = New eggHTS(AppContainer.GetGCModeller & "/eggHTS.exe")
    End Sub

    Public Function RunApp(argumentJSON As String, workspace As String) As Exception Implements IBiostackApp.RunApp
        Dim args = argumentJSON.LoadObject(Of Dictionary(Of String, String))
        Dim organism$ = args!organism
        Dim orgName$ = args!organismName
        Dim geneSet$ = workspace & "/geneSet.txt"


    End Function
End Class
