Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.Uniprot.XML
Imports SMRUCC.genomics.SequenceModel

Public Class UniProtImporter

    ReadOnly registry As biocad_registry

    Sub New(registry As biocad_registry)
        Me.registry = registry
    End Sub

    Public Sub importsData(pagedata As entry())
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim terms = registry.vocabulary_terms

        For Each prot As entry In pagedata
            Dim mol As biocad_registryModel.molecule = check_protein(prot)

            If mol Is Nothing Then
                Dim seq As String = prot.ProteinSequence
                Dim formula = MolecularWeightCalculator.PolypeptideFormula(seq)
                Dim mass = MolecularWeightCalculator.CalcMW_Polypeptide(seq)
                Dim desc As String = prot.GetCommentText("function")

                Call trans.add(
                    field("xref_id") = prot.accessions(0),
                    field("name") = prot.name,
                    field("mass") = mass,
                    field("type") = terms.protein_term,
                    field("formula") = formula,
                    field("parent") = 0,
                    field("tax_id") = Val(prot.NCBITaxonomyId),
                    field("note") = desc
                )
            End If
        Next

        Call trans.commit()

        trans = registry.sequence_graph.open_transaction.ignore

        For Each prot As entry In pagedata
            Dim mol As biocad_registryModel.molecule = check_protein(prot)

            If mol Is Nothing Then
                Continue For
            End If

            Dim seq As String = prot.ProteinSequence
            Dim hashcode As String = BatchDataCommit.SequenceHashcode(seq)
            Dim check = registry.sequence_graph _
                .where(field("molecule_id") = mol.id,
                       field("hashcode") = hashcode) _
                .find(Of biocad_registryModel.sequence_graph)

            If check IsNot Nothing Then
                Continue For
            End If

            Call trans.add(
                field("molecule_id") = mol.id,
                field("sequence") = seq,
                field("hashcode") = hashcode,
                field("morgan") = ""
            )
        Next

        Call trans.commit()

        ' add keywords
        trans = registry.molecule_tags.open_transaction.ignore

        For Each prot As entry In pagedata
            Dim mol As biocad_registryModel.molecule = check_protein(prot)

            If mol Is Nothing Then
                Continue For
            End If


        Next
    End Sub

    Private Function check_protein(prot As entry) As biocad_registryModel.molecule
        Dim tax_id = CUInt(Val(prot.NCBITaxonomyId))
        Dim check = registry.molecule _
            .where(field("xref_id") = prot.accessions(0)) _
            .find(Of biocad_registryModel.molecule)

        If Not check Is Nothing Then
            Return check
        End If

        check = registry.db_xrefs _
            .left_join("molecule") _
            .on(field("molecule.id") = field("obj_id")) _
            .where(field("db_key") = registry.vocabulary_terms.uniprot_term,
                   field("xref") = prot.accessions(0)) _
            .find(Of biocad_registryModel.molecule)("`molecule`.*")

        If Not check Is Nothing Then
            Return check
        End If

        Dim seq As String = prot.ProteinSequence
        Dim hashcode As String = BatchDataCommit.SequenceHashcode(seq)

        check = registry.molecule _
            .left_join("sequence_graph") _
            .on(field("`sequence_graph`.molecule_id") = field("`molecule`.id")) _
            .where(field("tax_id") = tax_id,
                   field("`sequence_graph`.hashcode") = hashcode) _
            .find(Of biocad_registryModel.molecule)("`molecule`.*")

        If Not check Is Nothing Then
            Return check
        End If

        Return Nothing
    End Function

End Class

Public Class UniProtPageLoader

    ReadOnly repo As String

    Sub New(repo As String)
        Me.repo = repo
    End Sub

    Public Iterator Function LoadPageData() As IEnumerable(Of entry())
        Dim tmp As New List(Of entry)

        For Each prot As entry In UniProtXML.EnumerateEntries({repo}, ignoreError:=True, tqdm:=True)
            Call tmp.Add(prot)

            If tmp.Count > 3000 Then
                Yield tmp.ToArray
                Call tmp.Clear()
            End If
        Next
    End Function

End Class