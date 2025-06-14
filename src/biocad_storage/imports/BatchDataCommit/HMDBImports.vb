Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.TMIC
Imports BioNovoGene.BioDeep.Chemistry.TMIC.HMDB
Imports Microsoft.VisualBasic.Linq
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
        Dim hmdb As UInteger = terms.hmdb_term

        Call MetaboliteCommit.CommitMetabolites(metadata, registry)
        Call MetaboliteCommit.CommitDbXrefs(metadata, registry)
        Call MetaboliteCommit.CommitSynonyms(metadata, registry)
        Call MetaboliteCommit.CommitStructData(metadata, registry)

        Dim trans_tag_link = registry.molecule_tags.open_transaction.ignore
        Dim trans_loc_link = registry.subcellular_location.open_transaction.ignore

        For Each metabolite As HMDB.metabolite In page
            Dim mol = registry.db_xrefs _
                .left_join("molecule").on(field("`molecule`.id") = field("obj_id")) _
                .where(field("db_key") = hmdb,
                       field("xref") = metabolite.accession) _
                .find(Of biocad_registryModel.molecule)("molecule.*")

            If mol Is Nothing Then
                Continue For
            End If

            If metabolite.biological_properties IsNot Nothing Then
                For Each tag As String In metabolite.biological_properties.biospecimen_locations.biospecimen.SafeQuery
                    Dim tag_id As UInteger = terms.GetVocabularyTerm(tag, "Bio Specimen")

                    If registry.molecule_tags.where(field("tag_id") = tag_id, field("molecule_id") = mol.id).find(Of biocad_registryModel.molecule_tags) Is Nothing Then
                        Call trans_tag_link.add(
                           field("tag_id") = tag_id, field("molecule_id") = mol.id,
                           field("description") = tag
                        )
                    End If
                Next

                For Each loc As String In metabolite.biological_properties.cellular_locations.cellular.SafeQuery
                    Dim locData = registry.subcellular_compartments.where(field("compartment_name") = loc).find(Of biocad_registryModel.subcellular_compartments)
                    If locData Is Nothing Then
                        registry.subcellular_compartments.add(
                            field("compartment_name") = loc,
                            field("topology") = "",
                            field("note") = "hmdb source"
                        )
                        locData = registry.subcellular_compartments _
                            .where(field("compartment_name") = loc) _
                            .order_by("id", desc:=True) _
                            .find(Of biocad_registryModel.subcellular_compartments)
                    End If
                    If locData Is Nothing Then
                        Continue For
                    End If

                    If registry.subcellular_location _
                        .where(field("compartment_id") = locData.id, field("obj_id") = mol.id, field("entity") = terms.molecule_entity) _
                        .find(Of biocad_registryModel.subcellular_location) Is Nothing Then

                        Call trans_loc_link.add(
                             field("compartment_id") = locData.id, field("obj_id") = mol.id, field("entity") = terms.molecule_entity
                        )
                    End If
                Next
            End If
        Next

        Call trans_tag_link.commit()
        Call trans_loc_link.commit()
    End Sub

End Class
