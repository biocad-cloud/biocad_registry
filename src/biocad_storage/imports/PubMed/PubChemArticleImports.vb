Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Public Class PubChemArticleImports

    ReadOnly registry As biocad_registry
    ReadOnly terms As BioCadVocabulary
    ReadOnly wrap_tqdm As Boolean = False

    Sub New(registry As biocad_registry)
        Me.terms = registry.vocabulary_terms
        Me.registry = registry
    End Sub

    Public Sub MakePageImports(articles As IEnumerable(Of PubMedTextTable), topic As String)
        Dim topic_id As UInteger = terms.GetVocabularyTerm(topic.ToLower, "Topic")

        For Each pagedata As PubMedTextTable() In articles.Where(Function(a) a.pmid > 0).SplitIterator(10000)
            Call MakeImports(pagedata, topic_id)
        Next
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub MakeImports(articles As PubMedTextTable(), topic As String)
        Call MakeImports(articles, topic_id:=terms.GetVocabularyTerm(topic.ToLower, "Topic"))
    End Sub

    Private Sub MakeImports(articles As PubMedTextTable(), topic_id As UInteger)
        Dim trans As CommitTransaction = registry.pubmed.open_transaction.ignore
        Dim pubchem_id As UInteger = terms.pubchem_term

        For Each article As PubMedTextTable In TqdmWrapper.Wrap(articles, wrap_console:=wrap_tqdm)
            If registry.pubmed.find_object(field("id") = article.pmid) Is Nothing Then
                Call trans.add(
                    field("id") = article.pmid,
                    field("authors") = article.articleauth,
                    field("title") = article.articletitle,
                    field("journal") = article.articlejourname,
                    field("year") = CUInt(Val(article.citation.StringSplit("[;,\s]").FirstOrDefault)),
                    field("citation") = article.citation,
                    field("doi") = article.doi,
                    field("affil") = article.articleaffil,
                    field("abstract") = article.articleabstract
                )
            End If
        Next

        Call trans.commit()

        Dim tag_trans = registry.molecule_tags.open_transaction.ignore

        trans = registry.pubmed_source.open_transaction.ignore

        For Each article As PubMedTextTable In TqdmWrapper.Wrap(articles, wrap_console:=wrap_tqdm)
            If article.cids.IsNullOrEmpty Then
                Continue For
            End If

            For Each cid As String In article.cids.Select(Function(s) Strings.Trim(s).Trim(""""c, "'"c, " "c))
                If Val(cid) = 0 Then
                    Continue For
                End If

                Dim mol = registry.db_xrefs _
                    .where(field("db_key") = pubchem_id,
                           field("xref") = cid,
                           field("type") = terms.metabolite_term) _
                    .project(Of UInteger)("obj_id")

                If mol.IsNullOrEmpty Then
                    Continue For
                End If

                For Each mod_id As UInteger In mol
                    Call trans.add(
                        field("molecule_id") = mod_id,
                        field("pubmed_id") = article.pmid,
                        field("note") = article.articletitle
                    )
                    Call tag_trans.add(
                        field("tag_id") = topic_id,
                        field("molecule_id") = mod_id,
                        field("description") = article.articletitle
                    )
                Next
            Next
        Next

        Call tag_trans.commit()
        Call trans.commit()

        trans = registry.mesh_link.open_transaction.ignore

        For Each article As PubMedTextTable In TqdmWrapper.Wrap(articles, wrap_console:=wrap_tqdm)
            If article.meshheadings.IsNullOrEmpty Then
                Continue For
            End If

            For Each mesh_name As String In article.meshheadings.Select(Function(s) Strings.Trim(s).Trim(""""c, "'"c, " "c))
                If mesh_name = "" Then
                    Continue For
                End If

                Dim term = registry.mesh.find_object(field("mesh_term") = mesh_name)
                If term Is Nothing Then
                    registry.mesh.add(field("mesh_term") = mesh_name)
                    term = registry.mesh.find_object(field("mesh_term") = mesh_name)
                End If
                If term Is Nothing Then
                    Continue For
                End If

                Call trans.add(
                    field("mesh_id") = term.id,
                    field("pubmed_id") = article.pmid
                )
            Next
        Next

        Call trans.commit()
    End Sub
End Class
