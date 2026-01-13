Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Assembly.Uniprot.XML
Imports protein = registry_data.biocad_registryModel.protein

Public Module ImportsUniProt

    <Extension>
    Public Sub importsUniProt(registry As biocad_registry, proteins As IEnumerable(Of entry))
        Dim db_uniprot As UInteger = registry.biocad_vocabulary.db_uniprot
        Dim db_genbank As UInteger = registry.biocad_vocabulary.db_genbank
        Dim locs As New Dictionary(Of String, compartment_location)
        Dim sql As CommitTransaction = registry.db_xrefs.ignore.open_transaction

        For Each block As entry() In proteins.SplitIterator(5000)
            For Each prot As entry In TqdmWrapper.Wrap(block)
                Dim locus_tag As String = prot.ORF
                Dim name As String = If(prot.geneName, prot.name)
                Dim desc As String = prot.proteinFullName
                Dim taxid As UInteger = prot.NCBITaxonomyId
                Dim seq As String = prot.ProteinSequence
                Dim hash As String = Strings.UCase(seq).MD5
                Dim check As protein

                If locus_tag.StringEmpty Then
                    check = registry.protein_data.where(field("source_id") = prot.accessions.First, field("ncbi_taxid") = taxid, field("source_db") = db_uniprot).find(Of protein)
                Else
                    check = registry.protein_data.where(field("source_id") = locus_tag, field("ncbi_taxid") = taxid, field("source_db") = db_genbank).find(Of protein)
                End If

                If check Is Nothing Then
                    Call registry.protein_data.add(
                        field("source_id") = prot.accessions.First,
                        field("ncbi_taxid") = taxid,
                        field("source_db") = db_uniprot,
                        field("name") = name,
                        field("function") = desc,
                        field("gene_id") = 0,
                        field("protein_id") = 0,
                        field("sequence") = seq,
                        field("checksum") = hash,
                        field("pdb_data") = 0
                    )
                    check = registry.protein_data.where(field("source_id") = prot.accessions.First, field("ncbi_taxid") = taxid, field("source_db") = db_uniprot).order_by("id", True).find(Of protein)
                End If

                If check Is Nothing Then
                    Continue For
                End If

                For Each loc As String In prot.SubCellularLocations
                    Dim cc As compartment_location = locs.ComputeIfAbsent(loc,
                        Function(loc_id)
                            Dim cc_model = registry.compartment_location.where(field("name") = loc_id).find(Of compartment_location)

                            If cc_model Is Nothing Then
                                registry.compartment_location.add(
                                    field("name") = loc_id,
                                    field("fullname") = loc_id
                                )
                                cc_model = registry.compartment_location.where(field("name") = loc_id).order_by("id", True).find(Of compartment_location)
                            End If

                            Return cc_model
                        End Function)

                    If Not cc Is Nothing Then
                        Call sql.add(registry.subcellular_location.add_sql(field("protein_id") = check.id, field("location_id") = cc.id, field("evidence") = prot.accessions.First))
                    End If
                Next


            Next
        Next
    End Sub

End Module
