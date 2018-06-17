<AppEntry(GCModeller.Apps.Enrichment)>
Public Class GeneSetEnrichment : Implements IBiostackApp

    Public ReadOnly Property AppID As GCModeller.Apps Implements IBiostackApp.AppID
        Get
            Return GCModeller.Apps.Enrichment
        End Get
    End Property

    Public Function RunApp(argumentJSON As String, workspace As String) As Exception Implements IBiostackApp.RunApp

    End Function
End Class
