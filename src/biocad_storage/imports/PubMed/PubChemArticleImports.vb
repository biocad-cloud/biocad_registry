Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Public Class PubChemArticleImports

    ReadOnly registry As biocad_registry
    ReadOnly terms As BioCadVocabulary

    Sub New(registry As biocad_registry)
        Me.terms = registry.vocabulary_terms
        Me.registry = registry
    End Sub

    Public Sub MakeImports(articles As PubChemTextJSON(), topic As String)
        For Each article As PubChemTextJSON In TqdmWrapper.Wrap(articles)

        Next
    End Sub
End Class
