Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem.ExtensionModels
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module PathwayImports

    <Extension>
    Public Sub ImportsPubChemPathway(registry As biocad_registry, pathways As PathwayGraph(), Optional topic As String = Nothing)
        Dim cid As UInteger = registry.vocabulary_terms.pubchem_term
        Dim topic_id As UInteger

        If topic.StringEmpty(, True) Then
            topic_id = registry.getVocabulary(topic, "Topic")
        End If

        For Each pathway As PathwayGraph In TqdmWrapper.Wrap(pathways)
            Dim db_key As UInteger = registry.getVocabulary(pathway.source, "External Database")
            Dim ref = registry.pathway _
                .where(field("xref_id") = pathway.pwacc,
                       field("source_dbkey") = db_key) _
                .find(Of biocad_registryModel.pathway)

            If ref Is Nothing Then
                Call registry.pathway.add(
                    field("xref_id") = pathway.pwacc,
                    field("source_dbkey") = db_key,
                    field("name") = pathway.name,
                    field("ncbi_taxid") = CUInt(Val(pathway.taxid)),
                    field("note") = pathway.citations.JoinBy("; ")
                )

                ref = registry.pathway _
                   .where(field("xref_id") = pathway.pwacc,
                          field("source_dbkey") = db_key) _
                   .order_by("id", desc:=True) _
                   .find(Of biocad_registryModel.pathway)
            End If

            If pathway.cids.IsNullOrEmpty OrElse ref Is Nothing Then
                Continue For
            End If

            Dim link_cids As biocad_registryModel.db_xrefs() = registry.db_xrefs _
                .where(field("db_key") = cid,
                       field("xref").in(pathway.cids)) _
                .select(Of biocad_registryModel.db_xrefs)
            Dim link_pathways = registry.pathway_graph.open_transaction.ignore
            Dim link_topic = registry.molecule_tags.open_transaction.ignore

            For Each mol As biocad_registryModel.db_xrefs In link_cids
                Call link_pathways.add(
                    field("pathway_id") = ref.id,
                    field("entity_id") = mol.obj_id,
                    field("note") = pathway.name
                )

                If topic_id > 0 Then
                    Call link_topic.add(
                        field("tag_id") = topic_id,
                        field("molecule_id") = mol.obj_id,
                        field("description") = pathway.pwacc
                    )
                End If
            Next

            Call link_topic.commit()
            Call link_pathways.commit()
        Next
    End Sub
End Module
