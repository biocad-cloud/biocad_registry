Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.TMIC
Imports BioNovoGene.BioDeep.Chemistry.TMIC.HMDB
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class HMDBImports

    ReadOnly registry As biocad_registry
    ReadOnly hmdbfile As String
    ReadOnly terms As BioCadVocabulary

    Sub New(registry As biocad_registry, hmdb As String)
        Me.registry = registry
        Me.hmdbfile = hmdb
        Me.terms = registry.vocabulary_terms
    End Sub

    Public Sub [Imports]()
        For Each page As HMDB.metabolite() In HMDB.metabolite.Load(hmdbfile).SplitIterator(100)
            Call [Imports](page)
        Next
    End Sub

    Private Sub [Imports](page As HMDB.metabolite())
        Dim metadata As MetaLib() = page.ConvertInternal.ToArray

        Call MetaboliteCommit.CommitMetabolites(metadata, registry)
        Call MetaboliteCommit.CommitDbXrefs(metadata, registry)
        Call MetaboliteCommit.CommitSynonyms(metadata, registry)
        Call MetaboliteCommit.CommitStructData(metadata, registry)

    End Sub

End Class
