﻿Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Framework
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class RefMetImports

    ReadOnly refmet As RefMet()
    ReadOnly registry As biocad_registry
    ReadOnly vocabulary As BioCadVocabulary

    Sub New(repo As String, registry As biocad_registry)
        Me.refmet = repo.LoadCsv(Of RefMet)(mute:=True).ToArray
        Me.registry = registry
        Me.vocabulary = registry.vocabulary_terms
    End Sub

    Public Sub [Imports]()
        For Each pagedata As RefMet() In refmet.Split(50)
            Call ImportsPageData(pagedata)
        Next
    End Sub

    Private Sub ImportsPageData(pagedata As RefMet())
        Dim metadata As MetaInfo() = pagedata.Select(Function(r) r.CastModel).ToArray

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Dim mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)
                Dim molData = {
                    field("xref_id") = meta.ID,
                    field("name") = meta.name,
                    field("mass") = If(mass < 0, 0, mass),
                    field("type") = vocabulary.metabolite_term,
                    field("formula") = meta.formula,
                    field("parent") = 0,
                    field("tax_id") = 0,
                    field("note") = meta.description
                }

                ' add new 
                Call registry.molecule.add(molData)
            End If
        Next

        Dim trans = registry.db_xrefs.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
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

        trans = registry.synonym.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim synonyms As String() = {meta.name}

            For Each name As String In synonyms
                Dim hash As String = name.ToLower.MD5
                Dim check = registry.synonym _
                    .where(field("obj_id") = mol.id, field("hashcode") = hash) _
                    .find(Of biocad_registryModel.synonym)

                If check Is Nothing Then
                    Call trans.add(
                        field("obj_id") = mol.id,
                        field("type_id") = vocabulary.metabolite_term,
                        field("hashcode") = hash,
                        field("synonym") = name,
                        field("lang") = "en"
                    )
                End If
            Next
        Next

        Call trans.commit()

        Dim trans_tree = registry.ontology_tree.open_transaction.ignore
        Dim trans_links = registry.molecule_ontology.open_transaction.ignore
        Dim refmet_ontology As UInteger = vocabulary.GetDatabaseKey(NameOf(BioNovoGene.BioDeep.Chemistry.MetaLib.RefMet))

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim l1 = vocabulary.GetOntologyTerm(meta.super_class, "super_class", NameOf(BioNovoGene.BioDeep.Chemistry.MetaLib.RefMet), meta.super_class)
            Dim l2 = vocabulary.GetOntologyTerm(meta.class, "class", NameOf(BioNovoGene.BioDeep.Chemistry.MetaLib.RefMet), meta.class)
            Dim l3 = vocabulary.GetOntologyTerm(meta.sub_class, "sub_class", NameOf(BioNovoGene.BioDeep.Chemistry.MetaLib.RefMet), meta.sub_class)

            ' build tree
            If registry.ontology_tree.where(field("ontology_id") = l2.id, field("is_a") = l1.id).find(Of biocad_registryModel.ontology_tree) Is Nothing Then
                trans_tree.add(
                    field("ontology_id") = l2.id, field("is_a") = l1.id
                )
            End If
            If registry.ontology_tree.where(field("ontology_id") = l3.id, field("is_a") = l2.id).find(Of biocad_registryModel.ontology_tree) Is Nothing Then
                trans_tree.add(
                   field("ontology_id") = l3.id, field("is_a") = l2.id
               )
            End If

            If registry.molecule_ontology.where(field("molecule_id") = mol.id, field("ontology_id") = l3.id).find(Of biocad_registryModel.molecule_ontology) Is Nothing Then
                trans_links.add(
                     field("molecule_id") = mol.id, field("ontology_id") = l3.id,
                     field("evidence") = "RefMet:" & mol.id
                )
            End If
        Next

        Call trans_tree.commit()
        Call trans_links.commit()
    End Sub
End Class
