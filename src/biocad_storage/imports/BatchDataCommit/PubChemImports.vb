﻿Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module MetaboliteImports

    <Extension>
    Public Sub RunDataCommit(registry As biocad_registry, metadata As MetaInfo(), uniref As Func(Of MetaInfo, String), Optional lazyMol As Boolean = True)
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim terms = registry.vocabulary_terms

        If lazyMol Then
            Call VBDebugger.EchoLine("the metabolute molecule will be created in transaction batch mode!")
        Else
            Call VBDebugger.EchoLine("the metabolute molecule will be created in one by one sequence mode!")
        End If

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, uniref)

            If mol Is Nothing Then
                Dim mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)
                Dim molData = {
                    field("xref_id") = uniref(meta),
                    field("name") = meta.name,
                    field("mass") = If(mass < 0, 0, mass),
                    field("type") = terms.metabolite_term,
                    field("formula") = meta.formula,
                    field("parent") = 0,
                    field("tax_id") = 0,
                    field("note") = meta.description
                }

                ' add new 
                If lazyMol Then
                    Call trans.add(molData)
                Else
                    Call registry.molecule.add(molData)
                End If
            End If
        Next

        Call trans.commit()
        Call CommitOdors(metadata, registry)

        trans = registry.sequence_graph.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim smiles As String = meta.xref.SMILES

            If smiles.StringEmpty(, True) Then
                Continue For
            End If

            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, uniref)

            If mol Is Nothing Then
                Continue For
            End If

            Dim seq = registry.sequence_graph.where(field("molecule_id") = mol.id).find(Of biocad_registryModel.sequence_graph)

            If seq Is Nothing Then
                trans.add(
                    field("molecule_id") = mol.id,
                    field("sequence") = smiles,
                    field("hashcode") = smiles.MD5,
                    field("morgan") = ""
                )
            End If
        Next

        Call trans.commit()

        trans = registry.db_xrefs.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, uniref)

            If mol Is Nothing Then
                Continue For
            End If

            Dim xrefs As NamedValue(Of String)() = meta.xref.PopulateXrefs(True, True).ToArray

            For Each xref As NamedValue(Of String) In xrefs
                Dim check = registry.db_xrefs _
                    .where(field("obj_id") = mol.id,
                           field("db_key") = terms.GetDatabaseKey(xref.Name),
                           field("xref") = xref.Value).find(Of biocad_registryModel.db_xrefs)

                If check Is Nothing Then
                    Call trans.add(
                        field("obj_id") = mol.id,
                        field("db_key") = terms.GetDatabaseKey(xref.Name),
                        field("xref") = xref.Value,
                        field("type") = terms.metabolite_term
                    )
                End If
            Next
        Next

        Call trans.commit()

        trans = registry.synonym.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, uniref)

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

    <Extension>
    Public Function findMolecule(registry As biocad_registry, meta As MetaInfo, uniref As Func(Of MetaInfo, String)) As biocad_registryModel.molecule
        Dim cid As String = uniref(meta)

        ' find molecule table via xref_id directly at first
        Dim q = registry.molecule _
            .where(field("xref_id") = cid, field("type") = registry.vocabulary_terms.metabolite_term) _
            .find(Of biocad_registryModel.molecule)

        If Not q Is Nothing Then
            Return q
        End If

        ' find molecule via db_xrefs
        Dim filter As FieldAssert = FieldAssert.empty
        Dim xrefs As NamedValue(Of String)() = meta.xref.PopulateXrefs(True, True).ToArray
        Dim mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)

        For Each xref As NamedValue(Of String) In xrefs
            filter = filter Or (field("db_key") = registry.vocabulary_terms.GetDatabaseKey(xref.Name) And field("xref") = xref.Value)
        Next

        q = registry.molecule _
            .left_join("db_xrefs") _
            .on(field("`db_xrefs`.obj_id") = field("`molecule`.id")) _
            .where(field("mass").between(mass - 1, mass + 1), filter) _
            .find(Of biocad_registryModel.molecule)("`molecule`.*")

        If Not q Is Nothing Then
            Return q
        End If

        ' find molecule table via name
        q = registry.molecule _
            .where(field("mass").between(mass - 1, mass + 1),
                   field("name") = meta.name) _
            .find(Of biocad_registryModel.molecule)

        If Not q Is Nothing Then
            Return q
        End If

        Dim hashset As String() = {meta.name, meta.IUPACName} _
            .JoinIterates(meta.synonym) _
            .Where(Function(s) Not s.StringEmpty(, True)) _
            .Select(Function(s) s.ToLower.MD5) _
            .ToArray

        If hashset.Any Then
            ' find molecule via the synonym
            q = registry.molecule _
                .left_join("synonym") _
                .on(field("`synonym`.obj_id") = field("`molecule`.id")) _
                .where(field("mass").between(mass - 1, mass + 1),
                       field("hashcode").in(hashset)) _
                .find(Of biocad_registryModel.molecule)("`molecule`.*")
        End If

        If Not q Is Nothing Then
            Return q
        End If

        Return Nothing
    End Function
End Module

Public Module PubChemImports

    <Extension>
    Public Sub RunDataCommit(registry As biocad_registry, pagedata As PugViewRecord())
        Dim metadata As MetaInfo() = pagedata.AsParallel _
            .Select(Function(m)
                        Try
                            Return DirectCast(m.GetMetaInfo, MetaInfo)
                        Catch ex As Exception
                            Call App.LogException(ex)
                        End Try

                        Return Nothing
                    End Function) _
            .Where(Function(m) Not m Is Nothing) _
            .ToArray

        Call MetaboliteImports.RunDataCommit(registry, metadata, Function(meta) $"PubChem:{meta.ID}")
    End Sub

End Module
