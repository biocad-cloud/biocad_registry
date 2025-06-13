Imports BioNovoGene.BioDeep.Chemistry.ChEBI
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.foundation.OBO_Foundry.IO.Models

''' <summary>
''' imports chebi metabolite and ontology information
''' </summary>
Public Class ChEBIOntologyImports

    ReadOnly registry As biocad_registry
    ReadOnly terms As BioCadVocabulary
    ReadOnly chebi As OBOFile

    Sub New(registry As biocad_registry, chebi_obo As String)
        Me.registry = registry
        Me.terms = registry.vocabulary_terms
        Me.chebi = New OBOFile(chebi_obo)
    End Sub

    Public Sub ImportsOntology()
        Dim ontology_id = terms.chebi_term
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
                Dim parent = registry.ontology.where(field("db_source") = ontology_id, field("db_xref") = parent_id).find(Of biocad_registryModel.ontology)

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

    Public Sub RunImports()
        Dim metadata As MetaInfo() = ChEBIObo.ImportsMetabolites(chebi).ToArray

        For Each page As MetaInfo() In metadata.Split(10000)
            Call RunImports(page)
        Next
    End Sub

    Private Sub RunImports(page As MetaInfo())
        For Each meta As MetaInfo In TqdmWrapper.Wrap(page)
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
            End If
        Next

        Dim trans = registry.db_xrefs.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(page)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

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

        For Each meta As MetaInfo In TqdmWrapper.Wrap(page)
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            Dim synonyms As String() = {meta.name}.JoinIterates(meta.synonym).Distinct.ToArray

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

        trans = registry.sequence_graph.open_transaction.ignore

        For Each meta As MetaInfo In TqdmWrapper.Wrap(page)
            Dim smiles As String = meta.xref.SMILES

            If smiles.StringEmpty(, True) Then
                Continue For
            End If

            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta, Function(s) s.ID)

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
    End Sub
End Class
