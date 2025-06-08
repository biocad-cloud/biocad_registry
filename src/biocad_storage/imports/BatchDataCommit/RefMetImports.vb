Imports BioNovoGene.BioDeep.Chemistry.MetaLib
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
        For Each pagedata As RefMet() In refmet.Split(5000)
            Call ImportsPageData(pagedata)
        Next
    End Sub

    Private Sub ImportsPageData(pagedata As RefMet())
        Dim metadata As MetaLib() = pagedata.Select(Function(r) r.CastModel).ToArray

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

        Dim trans_tree = registry.ontology_tree.open_transaction
        Dim trans_links = registry.molecule_ontology.open_transaction
        Dim refmet_ontology As UInteger = vocabulary.GetDatabaseKey(NameOf(BioNovoGene.BioDeep.Chemistry.MetaLib.RefMet))

        For Each meta As MetaInfo In TqdmWrapper.Wrap(metadata)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim l1 = registry.ontology _
                .where(field("db_xref") = meta.super_class,
                       field("db_source") = refmet_ontology) _
                .find(Of biocad_registryModel.ontology)

            If l1 Is Nothing Then
                registry.ontology.add(
                    field("db_xref") = meta.super_class,
                    field("db_source") = refmet_ontology,
                    field("name") = meta.super_class
                )
                l1 = registry.ontology _
                    .where(field("db_xref") = meta.super_class,
                           field("db_source") = refmet_ontology) _
                    .order_by("id", desc:=True) _
                    .find(Of biocad_registryModel.ontology)
            End If

            Dim l2 = registry.ontology
        Next
    End Sub
End Class
