Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.foundation.OBO_Foundry.IO.Models

Public Class GeneOntologyImports

    ReadOnly registry As biocad_registry
    ReadOnly terms As BioCadVocabulary
    ReadOnly chebi As OBOFile

    Sub New(registry As biocad_registry, go_obo As String)
        Me.registry = registry
        Me.terms = registry.vocabulary_terms
        Me.chebi = New OBOFile(go_obo)
    End Sub

    Public Sub ImportsOntology()
        Dim ontology_id = terms.go_term
        Dim trans As CommitTransaction = registry.ontology.open_transaction.ignore
        Dim term_dataset = chebi.GetRawTerms _
            .AsParallel _
            .Where(Function(t) t.type = "[Term]") _
            .Select(Function(t)
                        Dim obo_data = t.GetData
                        Dim id As String = obo_data(RawTerm.Key_id).First
                        Return New NamedValue(Of Dictionary(Of String, String()))(id, obo_data)
                    End Function) _
            .ToArray

        For Each term As NamedValue(Of Dictionary(Of String, String())) In TqdmWrapper.Wrap(term_dataset)
            Dim obo_data = term.Value
            Dim id As String = obo_data(RawTerm.Key_id).First
            Dim name As String = obo_data(RawTerm.Key_name).First
            Dim def As String = obo_data.TryGetValue(RawTerm.Key_def).JoinBy("; ")

            If registry.ontology.where(field("db_source") = ontology_id, field("db_xref") = id).find(Of biocad_registryModel.ontology) Is Nothing Then
                Call trans.add(
                    field("db_xref") = id,
                    field("db_source") = ontology_id,
                    field("rank") = 0,
                    field("name") = name,
                    field("description") = def
                )
            End If
        Next

        Call trans.commit()

        trans = registry.ontology_tree.open_transaction.ignore

        ' build tree
        For Each term As NamedValue(Of Dictionary(Of String, String())) In TqdmWrapper.Wrap(term_dataset)
            Dim obo_data = term.Value
            Dim id As String = obo_data(RawTerm.Key_id).First
            Dim is_a As String() = obo_data.TryGetValue(RawTerm.Key_is_a)
            Dim term_id = registry.ontology.where(field("db_source") = ontology_id, field("db_xref") = id).find(Of biocad_registryModel.ontology)

            If term_id Is Nothing OrElse is_a.IsNullOrEmpty Then
                Continue For
            End If

            For Each parent_id As String In is_a
                Dim parent_term = parent_id.GetTagValue("!")
                Dim parent = registry.ontology.where(field("db_source") = ontology_id, field("db_xref") = parent_term.Name).find(Of biocad_registryModel.ontology)

                If Not parent Is Nothing Then
                    Dim check = registry.ontology_tree.where(field("ontology_id") = term_id.id, field("is_a") = parent.id).find(Of biocad_registryModel.ontology_tree)

                    If check Is Nothing Then
                        Call trans.add(
                            field("ontology_id") = term_id.id, field("is_a") = parent.id
                        )
                    End If
                End If
            Next
        Next

        Call trans.commit()

        ' link metabolite with ontology
        trans = registry.molecule_ontology.open_transaction.ignore

        For Each term As NamedValue(Of Dictionary(Of String, String())) In TqdmWrapper.Wrap(term_dataset)
            Dim id As String = term.Name
            Dim term_id = registry.ontology.where(field("db_source") = ontology_id, field("db_xref") = id).find(Of biocad_registryModel.ontology)

            If term_id Is Nothing Then
                Continue For
            End If

            Dim mol As biocad_registryModel.db_xrefs = registry.db_xrefs.where(field("db_key") = ontology_id, field("xref") = id).find(Of biocad_registryModel.db_xrefs)

            If Not mol Is Nothing Then
                If registry.molecule_ontology.where(field("molecule_id") = mol.obj_id, field("ontology_id") = term_id.id).find(Of biocad_registryModel.molecule_ontology) Is Nothing Then
                    Call trans.add(
                       field("molecule_id") = mol.obj_id,
                       field("ontology_id") = term_id.id,
                       field("evidence") = term.Name
                    )
                End If
            End If
        Next

        Call trans.commit()
    End Sub

End Class
