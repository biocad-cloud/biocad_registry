Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem.ExtensionModels
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module PathwayImports

    Public Sub ImportsPubChemPathway(registry As biocad_registry, pathways As PathwayGraph(), source As String)
        Dim db_key As UInteger = registry.getVocabulary(source, "External Database")
        Dim cid As UInteger = registry.vocabulary_terms.pubchem_term

        For Each pathway As PathwayGraph In TqdmWrapper.Wrap(pathways)
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

            For Each mol As biocad_registryModel.db_xrefs In link_cids
                Call link_pathways.add(
                    field("pathway_id") = ref.id,
                    field("entity_id") = mol.obj_id,
                    field("note") = pathway.name
                )
            Next

            Call link_pathways.commit()
        Next
    End Sub
End Module
