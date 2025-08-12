Imports BioNovoGene.BioDeep.Chemistry.Coconut
Imports BioNovoGene.BioDeep.Chemistry.LOTUS
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class LotusNPImports

    ReadOnly registry As biocad_registry

    Sub New(registry As biocad_registry)
        Me.registry = registry
    End Sub

    Public Sub ImportsNP(np As IEnumerable(Of NaturalProduct))
        For Each page As NaturalProduct() In np.SplitIterator(2500)
            Call importsPage(page)
        Next
    End Sub

    Private Sub importsPage(page As NaturalProduct())
        Dim meta = page.Select(Function(a) DirectCast(a.CreateMetabolite, MetaInfo)).ToArray

        Call MetaboliteImports.RunDataCommit(registry, meta, uniref:=Function(m) m.ID, lazyMol:=False)
        Call MetaboliteCommit.CommitTags(registry, meta, "natural products")
        Call MetaboliteCommit.CommitStructClass(meta, registry, "LOTUS NPclass")

        Dim links = registry.taxonomy_source.open_transaction.ignore

        For i As Integer = 0 To page.Length - 1
            Dim mol = registry.findMolecule(meta(i), Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            For Each tax As NamedValue(Of Taxonomy) In page(i).GetNCBITaxonomyReference
                If registry.taxonomy_source _
                    .where(field("molecule_id") = mol.id,
                           field("ncbi_taxid") = tax.Value.cleaned_organism_id,
                           field("doi") = tax.Name) _
                    .find(Of biocad_registryModel.taxonomy_source) Is Nothing Then

                    Call links.add(
                          field("molecule_id") = mol.id,
                           field("ncbi_taxid") = tax.Value.cleaned_organism_id,
                           field("doi") = tax.Name
                    )
                End If
            Next
        Next

        Call links.commit()
    End Sub

End Class

Public Class CoconutNPImports

    ReadOnly registry As biocad_registry

    Sub New(registry As biocad_registry)
        Me.registry = registry
    End Sub

    Public Sub ImportsNP(np As IEnumerable(Of CoconutNPTable))
        For Each page As CoconutNPTable() In np.SplitIterator(2500)
            Call importsPage(page)
        Next
    End Sub

    Private Sub importsPage(page As CoconutNPTable())
        Dim meta = page.Select(Function(a) DirectCast(a.GetMetaboliteData, MetaInfo)).ToArray

        Call MetaboliteImports.RunDataCommit(registry, meta, uniref:=Function(m) m.ID, lazyMol:=False)
        Call MetaboliteCommit.CommitTags(registry, meta, "natural products")
        Call MetaboliteCommit.CommitStructClass(meta, registry, "Coconut NPclass")

        Dim links = registry.taxonomy_source.open_transaction.ignore
        Dim tag_links = registry.molecule_tags.open_transaction.ignore

        For i As Integer = 0 To page.Length - 1
            Dim mol = registry.findMolecule(meta(i), Function(a) a.ID)

            If mol Is Nothing Then
                Continue For
            End If

            For Each tax As String In page(i).organisms
                Dim ncbi_tax = registry.ncbi_taxonomy.where(field("taxname") = tax).find(Of biocad_registryModel.ncbi_taxonomy)

                If ncbi_tax Is Nothing Then
                    ' use molecule tags
                    Dim tag_id = registry.getVocabulary($"tax:{tax}".ToLower, "Topic")

                    Call tag_links.add(
                        field("tag_id") = tag_id,
                        field("molecule_id") = mol.id,
                        field("description") = tax
                    )
                Else
                    If registry.taxonomy_source _
                        .where(field("molecule_id") = mol.id,
                               field("ncbi_taxid") = ncbi_tax.id,
                               field("doi") = $"Coconut-{page(i).identifier}") _
                        .find(Of biocad_registryModel.taxonomy_source) Is Nothing Then

                        Call links.add(
                              field("molecule_id") = mol.id,
                               field("ncbi_taxid") = ncbi_tax.id,
                               field("doi") = $"Coconut-{page(i).identifier}"
                        )
                    End If
                End If
            Next
        Next

        Call links.commit()
        Call tag_links.commit()
    End Sub
End Class