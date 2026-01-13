Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
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
        Dim prot_fasta As UInteger = registry.biocad_vocabulary.protein_data
        Dim db_rhea As UInteger = registry.biocad_vocabulary.GetDatabaseResource("Rhea").id
        Dim enzyme As UInteger = registry.biocad_vocabulary.GetVocabulary("Metabolic Role", biocad_vocabulary.RoleEnzyme).id
        Dim db_EC As UInteger = registry.biocad_vocabulary.db_ECNumber
        Dim keywords As New Dictionary(Of String, vocabulary)

        For Each block As entry() In proteins.SplitIterator(5000)
            ' db_xrefs
            Dim sql As CommitTransaction = registry.db_xrefs.ignore.open_transaction

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

                Dim loc_uid As UInteger = 0

                ' sub-cellular location
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
                        loc_uid = cc.id
                        Call sql.add(registry.subcellular_location.add_sql(field("protein_id") = check.id, field("location_id") = cc.id, field("evidence") = prot.accessions.First))
                    End If
                Next

                For Each acc As String In prot.accessions
                    Call sql.add(field("obj_id") = check.id, field("type") = prot_fasta, field("db_name") = db_uniprot, field("db_xref") = acc, field("db_source") = db_uniprot)
                Next

                If Not locus_tag.StringEmpty Then
                    Call sql.add(field("obj_id") = check.id, field("type") = prot_fasta, field("db_name") = db_genbank, field("db_xref") = locus_tag, field("db_source") = db_uniprot)
                End If

                ' enzyme catalysis
                For Each cat As comment In prot.CommentList.TryGetValue("catalytic activity")
                    Dim rhea As dbReference = cat.dbReferences.KeyItem("Rhea")

                    If rhea Is Nothing Then
                        Continue For
                    End If

                    Dim rxn = registry.reaction.where(field("db_xref") = rhea.id, field("db_source") = db_rhea).find(Of biocad_registryModel.reaction)

                    If Not rxn Is Nothing Then
                        Call sql.add(registry.metabolic_network.add_sql(
                            field("reaction_id") = rxn.id,
                            field("factor") = 1,
                            field("species_id") = check.id,
                            field("symbol_id") = prot.accessions.First,
                            field("role") = enzyme,
                            field("compartment_id") = loc_uid,
                            field("note") = $"{prot.accessions.First} - {cat.GetText}"
                        ))
                    End If
                Next

                ' ec number
                For Each ec_num As String In prot.DbReferenceIds("EC")
                    Call sql.add(field("obj_id") = check.id,
                                 field("type") = prot_fasta,
                                 field("db_name") = db_EC,
                                 field("db_xref") = ec_num,
                                 field("db_source") = db_uniprot)
                Next

                For Each keyword In prot.keywords.SafeQuery
                    Dim term As vocabulary = keywords.ComputeIfAbsent(
                        keyword.value,
                        Function(str)
                            Return registry.biocad_vocabulary.GetVocabulary("UniProt Keyword", str)
                        End Function)

                    Call sql.add(registry.topic.add_sql(
                        field("topic_id") = term.id,
                        field("type") = prot_fasta,
                        field("model_id") = check.id,
                        field("note") = keyword.id
                    ))
                Next
            Next

            Call sql.commit()
        Next
    End Sub

End Module
