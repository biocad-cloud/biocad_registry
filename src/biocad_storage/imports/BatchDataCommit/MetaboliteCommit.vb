﻿Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module MetaboliteCommit

    <Extension>
    Public Sub CommitMetabolites(Of T As MetaInfo)(metabolites As IEnumerable(Of T), registry As biocad_registry)
        Dim terms As BioCadVocabulary = registry.vocabulary_terms

        For Each meta As T In TqdmWrapper.Wrap(metabolites.ToArray)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Dim mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)
                Dim molData = {
                    field("xref_id") = meta.ID,
                    field("name") = meta.name,
                    field("mass") = If(mass < 0, 0, mass),
                    field("type") = terms.metabolite_term,
                    field("formula") = meta.formula,
                    field("parent") = 0,
                    field("tax_id") = 0,
                    field("note") = meta.description
                }

                ' add new 
                Call registry.molecule.add(molData)
            ElseIf mol.note.StringEmpty(, True) AndAlso Not meta.description.StringEmpty(, True) Then
                Call registry.molecule.where(field("id") = mol.id).save(field("note") = meta.description)
            End If
        Next
    End Sub

    Public Sub CommitTags(Of T As MetaInfo)(registry As biocad_registry, metabolites As IEnumerable(Of T), topic As String)
        Dim topic_id As String = registry.getVocabulary(topic, "Topic")
        Dim links = registry.molecule_tags.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metabolites.ToArray)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            If registry.molecule_tags _
                .where(field("molecule_id") = mol.id,
                       field("tag_id") = topic_id) _
                .find(Of biocad_registryModel.molecule_tags) Is Nothing Then

                Call links.add(
                    field("molecule_id") = mol.id,
                    field("tag_id") = topic_id,
                    field("description") = meta.name
                )
            End If
        Next

        Call links.commit()
    End Sub

    Public Sub CommitOdors(Of T As MetaInfo)(metabolites As IEnumerable(Of T), registry As biocad_registry)
        Dim trans = registry.odor.open_transaction.ignore
        Dim terms As BioCadVocabulary = registry.vocabulary_terms
        Dim odor_term As UInteger = terms.GetVocabularyTerm("Odor", "Odor Category")
        Dim taste_term As UInteger = terms.GetVocabularyTerm("Taste", "Odor Category")
        Dim color_term As UInteger = terms.GetVocabularyTerm("Color", "Odor Category")

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metabolites.ToArray)
            If Not TypeOf meta Is MetaLib Then
                Continue For
            ElseIf DirectCast(meta, MetaLib).chemical Is Nothing Then
                Continue For
            End If

            Dim odors As NamedValue(Of String)() = DirectCast(meta, MetaLib).chemical _
                .EnumerateOdorTerms _
                .ToArray

            If odors.IsNullOrEmpty Then
                Continue For
            End If

            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            For Each group As IGrouping(Of String, NamedValue(Of String)) In odors.GroupBy(Function(a) a.Name)
                Dim term_id As UInteger

                Select Case group.Key
                    Case "odor" : term_id = odor_term
                    Case "taste" : term_id = taste_term
                    Case "color" : term_id = color_term
                    Case Else
                        Throw New NotImplementedException(group.Key)
                End Select

                For Each odor As NamedValue(Of String) In group
                    Dim check = registry.odor _
                        .where(field("molecule_id") = mol.id,
                               field("category") = term_id,
                               field("odor") = odor.Value) _
                        .find(Of biocad_registryModel.odor)

                    If check Is Nothing Then
                        Call trans.add(
                            field("molecule_id") = mol.id,
                            field("category") = term_id,
                            field("odor") = odor.Value,
                            field("hashcode") = odor.Value.ToLower.MD5,
                            field("value") = 0,
                            field("unit") = 0,
                            field("text") = odor.Description
                        )
                    End If
                Next
            Next
        Next

        Call trans.commit()
    End Sub

    Public Sub CommitDbXrefs(Of T As MetaInfo)(metabolites As IEnumerable(Of T), registry As biocad_registry)
        Dim trans = registry.db_xrefs.open_transaction.ignore
        Dim vocabulary = registry.vocabulary_terms

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metabolites.ToArray)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim xrefs As NamedValue(Of String)() = meta.xref.PopulateXrefs(True, True).ToArray

            For Each xref As NamedValue(Of String) In xrefs
                Dim check = registry.db_xrefs _
                    .where(field("obj_id") = mol.id,
                           field("db_key") = vocabulary.GetDatabaseKey(xref.Name),
                           field("xref") = xref.Value).find(Of biocad_registryModel.db_xrefs)

                If check Is Nothing Then
                    Call trans.add(
                        field("obj_id") = mol.id,
                        field("db_key") = vocabulary.GetDatabaseKey(xref.Name),
                        field("xref") = xref.Value,
                        field("type") = vocabulary.metabolite_term
                    )
                End If
            Next
        Next

        Call trans.commit()
    End Sub

    Public Sub CommitSynonyms(Of T As MetaInfo)(metabolites As IEnumerable(Of T), registry As biocad_registry)
        Dim trans = registry.synonym.open_transaction.ignore
        Dim terms As BioCadVocabulary = registry.vocabulary_terms

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metabolites.ToArray)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim synonyms As String() = {meta.name, meta.IUPACName} _
                .JoinIterates(meta.synonym) _
                .Where(Function(s) Not s.StringEmpty(, True)) _
                .Distinct _
                .ToArray

            For Each name As String In synonyms
                Dim hash As String = name.ToLower.MD5
                Dim check = registry.synonym _
                    .where(field("obj_id") = mol.id, field("hashcode") = hash) _
                    .find(Of biocad_registryModel.synonym)

                If check Is Nothing Then
                    Call trans.add(
                        field("obj_id") = mol.id,
                        field("type_id") = terms.metabolite_term,
                        field("hashcode") = hash,
                        field("synonym") = name,
                        field("lang") = "en"
                    )
                End If
            Next
        Next

        Call trans.commit()
    End Sub

    Public Sub CommitTaxLink(Of T As MetaInfo)(metabolites As IEnumerable(Of T), registry As biocad_registry, ncbi_taxid As UInteger, Optional doi As String = "-")
        Dim links = registry.taxonomy_source.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metabolites.ToArray)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            If registry.taxonomy_source _
                .where(field("molecule_id") = mol.id,
                       field("ncbi_taxid") = ncbi_taxid) _
                .find(Of biocad_registryModel.taxonomy_source) Is Nothing Then

                Call links.add(
                    field("molecule_id") = mol.id,
                    field("ncbi_taxid") = ncbi_taxid,
                    field("doi") = doi
                )
            End If
        Next

        Call links.commit()
    End Sub

    Public Sub CommitStructData(Of T As MetaInfo)(metabolites As IEnumerable(Of T), registry As biocad_registry)
        Dim trans = registry.sequence_graph.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metabolites.ToArray)
            Dim smiles As String = meta.xref.SMILES

            If smiles.StringEmpty(, True) Then
                Continue For
            End If

            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim seq = registry.sequence_graph.where(field("molecule_id") = mol.id).find(Of biocad_registryModel.sequence_graph)

            If seq Is Nothing OrElse seq.sequence.StringEmpty(, True) Then
                trans.add(
                    field("molecule_id") = mol.id,
                    field("sequence") = smiles,
                    field("hashcode") = smiles.MD5,
                    field("morgan") = ""
                )
            End If
        Next

        Call trans.commit()
    End Sub

    Public Sub CommitStructClass(Of T As MetaInfo)(metabolites As IEnumerable(Of T), registry As biocad_registry, ontology_name As String)
        Dim trans_links = registry.molecule_ontology.open_transaction.ignore
        Dim vocabulary = registry.vocabulary_terms

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metabolites.ToArray)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim l1 = (term:=vocabulary.GetOntologyTerm(meta.kingdom, "kingdom", ontology_name, meta.kingdom), tag:=meta.kingdom)
            Dim l2 = (term:=vocabulary.GetOntologyTerm(meta.super_class, "super_class", ontology_name, meta.super_class), tag:=meta.super_class)
            Dim l3 = (term:=vocabulary.GetOntologyTerm(meta.class, "class", ontology_name, meta.class), tag:=meta.class)
            Dim l4 = (term:=vocabulary.GetOntologyTerm(meta.sub_class, "sub_class", ontology_name, meta.sub_class), tag:=meta.sub_class)
            Dim l5 = (term:=vocabulary.GetOntologyTerm(meta.molecular_framework, "molecular_framework", ontology_name, meta.molecular_framework), tag:=meta.molecular_framework)

            ' build tree

            If Not (l1.tag.StringEmpty(, True) OrElse l2.tag.StringEmpty(, True)) Then
                If registry.ontology_tree.where(field("ontology_id") = l2.term.id, field("is_a") = l1.term.id).find(Of biocad_registryModel.ontology_tree) Is Nothing Then
                    registry.ontology_tree.add(
                        field("ontology_id") = l2.term.id, field("is_a") = l1.term.id
                    )
                End If
            End If
            If Not (l2.tag.StringEmpty(, True) OrElse l3.tag.StringEmpty(, True)) Then
                If registry.ontology_tree.where(field("ontology_id") = l3.term.id, field("is_a") = l2.term.id).find(Of biocad_registryModel.ontology_tree) Is Nothing Then
                    registry.ontology_tree.add(
                        field("ontology_id") = l3.term.id, field("is_a") = l2.term.id
                    )
                End If
            End If
            If Not (l3.tag.StringEmpty(, True) OrElse l4.tag.StringEmpty(, True)) Then
                If registry.ontology_tree.where(field("ontology_id") = l4.term.id, field("is_a") = l3.term.id).find(Of biocad_registryModel.ontology_tree) Is Nothing Then
                    registry.ontology_tree.add(
                        field("ontology_id") = l4.term.id, field("is_a") = l3.term.id
                    )
                End If
            End If
            If Not (l4.tag.StringEmpty(, True) OrElse l5.tag.StringEmpty(, True)) Then
                If registry.ontology_tree.where(field("ontology_id") = l5.term.id, field("is_a") = l4.term.id).find(Of biocad_registryModel.ontology_tree) Is Nothing Then
                    registry.ontology_tree.add(
                        field("ontology_id") = l5.term.id, field("is_a") = l4.term.id
                    )
                End If
            End If

            For Each rank In {l5, l4, l3, l2, l1}
                If Not rank.tag.StringEmpty(, True) Then
                    If registry.molecule_ontology _
                        .where(field("molecule_id") = mol.id, field("ontology_id") = rank.term.id) _
                        .find(Of biocad_registryModel.molecule_ontology) Is Nothing Then

                        Call trans_links.add(
                            field("molecule_id") = mol.id, field("ontology_id") = rank.term.id,
                            field("evidence") = meta.ID
                        )
                        Exit For
                    End If
                End If
            Next
        Next

        Call trans_links.commit()
    End Sub
End Module
