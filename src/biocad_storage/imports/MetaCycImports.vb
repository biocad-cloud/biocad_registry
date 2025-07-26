Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Data.BioCyc

Public Class MetaCycImports

    ReadOnly registry As biocad_registry
    ReadOnly metacyc As Workspace

    Sub New(registry As biocad_registry, metacyc As Workspace)
        Me.registry = registry
        Me.metacyc = metacyc
    End Sub

    Public Sub ImportsCompounds()
        Dim compoundList = metacyc.compounds.features.ToArray
        Dim compoundSet As New List(Of MetaLib)

        For Each cpd As compounds In TqdmWrapper.Wrap(compoundList)
            Dim db_xrefs = compounds.GetDbLinks(cpd).GroupBy(Function(a) a.DBName).ToDictionary(Function(a) a.Key.ToLower, Function(a) a.First.entry)
            Dim meta As New MetaLib With {
                .ID = cpd.uniqueId,
                .name = cpd.commonName,
                .xref = New xref With {
                    .SMILES = cpd.SMILES,
                    .chebi = db_xrefs.TryGetValue("chebi"),
                    .CAS = {db_xrefs.TryGetValue("cas")},
                    .HMDB = db_xrefs.TryGetValue("hmdb"),
                    .chemspider = db_xrefs.TryGetValue("chemspider"),
                    .DrugBank = db_xrefs.TryGetValue("drugbank"),
                    .pubchem = db_xrefs.TryGetValue("pubchem"),
                    .extras = New Dictionary(Of String, String()) From {
                        {"MetaboLights", {db_xrefs.TryGetValue("metabolights")}},
                        {"MetaNetX", {db_xrefs.TryGetValue("metanetx")}}
                    },
                    .Wikipedia = db_xrefs.TryGetValue("|wikipedia|"),
                    .MetaCyc = cpd.uniqueId
                },
                .synonym = cpd.synonyms,
                .formula = compounds.FormulaString(cpd),
                .exact_mass = FormulaScanner.EvaluateExactMass(.formula),
                .description = cpd.comment
            }

            If Len(meta.xref.chebi) > 0 Then
                meta.xref.chebi = "CHEBI:" & meta.xref.chebi
            End If

            Call compoundSet.Add(meta)
        Next

        Call MetaboliteCommit.CommitMetabolites(compoundSet, registry)
        Call MetaboliteCommit.CommitDbXrefs(compoundSet, registry)
        Call MetaboliteCommit.CommitStructData(compoundSet, registry)
        Call MetaboliteCommit.CommitSynonyms(compoundSet, registry)
    End Sub

    Public Sub ImportsTranscriptUnits()
        Dim operons = metacyc.transunits.features.ToArray
        Dim links = registry.cluster_link.open_transaction.ignore
        Dim metacyc_key As UInteger = registry.getVocabulary("BioCyc", "External Database")
        Dim gene_key = registry.vocabulary_terms.gene_term
        Dim taxid = If(metacyc.species Is Nothing, "0", metacyc.species.NCBITaxonomyId)

        For Each unit As transunits In TqdmWrapper.Wrap(operons)
            Dim check = registry.conserved_cluster _
                .where(field("db_xref") = unit.uniqueId, field("tax_id") = taxid) _
                .find(Of biocad_registryModel.conserved_cluster)

            If check Is Nothing Then
                registry.conserved_cluster.add(
                      field("db_xref") = unit.uniqueId,
                      field("name") = unit.commonName,
                      field("size") = unit.components.TryCount,
                      field("description") = unit.comment,
                      field("tax_id") = taxid
                )
                check = registry.conserved_cluster _
                    .where(field("db_xref") = unit.uniqueId,
                           field("tax_id") = taxid) _
                    .order_by("id", desc:=True) _
                    .find(Of biocad_registryModel.conserved_cluster)
            End If

            For Each id As String In unit.components.SafeQuery
                Dim find_gene = registry.db_xrefs _
                    .where(field("db_key") = metacyc_key,
                           field("xref") = id,
                           field("type") = gene_key) _
                    .find(Of biocad_registryModel.db_xrefs)

                If Not find_gene Is Nothing Then
                    Call links.add(
                        field("cluster_id") = check.id,
                        field("gene_id") = find_gene.obj_id
                    )
                End If
            Next
        Next

        Call links.commit()
    End Sub

    Public Sub ImportsGenes()
        Dim genes = metacyc.genes.features.ToArray
        Dim taxid = If(metacyc.species Is Nothing, "0", metacyc.species.NCBITaxonomyId)
        Dim gene_key = registry.vocabulary_terms.gene_term
        Dim xrefs = registry.db_xrefs.open_transaction.ignore
        Dim names = registry.synonym.open_transaction.ignore
        Dim metacyc_key As UInteger = registry.getVocabulary("BioCyc", "External Database")
        Dim prot_key = registry.vocabulary_terms.protein_term

        For Each gene As genes In TqdmWrapper.Wrap(genes)
            Dim mol_xref = {gene.accession1, gene.accession2}.Select(Function(id) $"{taxid}:{id}").ToArray
            Dim mol = registry.molecule _
                .where(field("xref_id").in(mol_xref),
                       field("type") = gene_key) _
                .find(Of biocad_registryModel.molecule)

            If Not mol Is Nothing Then
                Call xrefs.add(field("obj_id") = mol.id,
                               field("db_key") = metacyc_key,
                               field("xref") = gene.uniqueId,
                               field("type") = gene_key)

                For Each name As String In {gene.commonName}.JoinIterates(gene.synonyms)
                    If Not name.StringEmpty(, True) Then
                        Call names.add(field("obj_id") = mol.id,
                                       field("type_id") = gene_key,
                                       field("synonym") = name,
                                       field("lang") = "en",
                                       field("hashcode") = name.ToLower.MD5)
                    End If
                Next

                If Not gene.product.StringEmpty(, True) Then
                    Dim prot_mol = registry.molecule.where(field("parent") = mol.id).find(Of biocad_registryModel.molecule)

                    If Not prot_mol Is Nothing Then
                        Call xrefs.add(field("obj_id") = prot_mol.id,
                                       field("db_key") = metacyc_key,
                                       field("xref") = gene.product,
                                       field("type") = prot_key)
                    End If
                End If
            End If
        Next

        Call xrefs.commit()
        Call names.commit()
    End Sub

    Public Sub ImportsReactions()
        Dim reactionList = metacyc.reactions.features.ToArray
        Dim leftKey = registry.getVocabulary("substrate", "Compound Role")
        Dim rightKey = registry.getVocabulary("product", "Compound Role")
        Dim db_key As UInteger = registry.getVocabulary("MetaCyc", "External Database")
        Dim substrates = registry.reaction_graph.open_transaction.ignore
        Dim enzyme = registry.getVocabulary("Enzymatic Catalysis", "Regulation Type")
        Dim catalysis = registry.regulation_graph.open_transaction.ignore

        For Each reaction In TqdmWrapper.Wrap(reactionList)
            Dim check = registry.reaction _
                .where(field("db_xref") = reaction.uniqueId,
                       field("source_dbkey") = db_key) _
                .find(Of biocad_registryModel.reaction)

            If check Is Nothing Then
                registry.reaction.add(
                    field("db_xref") = reaction.uniqueId, field("source_dbkey") = db_key,
                    field("name") = If(reaction.commonName, If(reaction.synonyms.IsNullOrEmpty, reaction.uniqueId, reaction.synonyms.JoinBy(" / "))),
                    field("equation") = reaction.equation.ToString,
                    field("note") = reaction.comment
                )
                check = registry.reaction _
                    .where(field("db_xref") = reaction.uniqueId,
                           field("source_dbkey") = db_key) _
                    .order_by("id", desc:=True) _
                    .find(Of biocad_registryModel.reaction)
            End If

            For Each item In reaction.left.SafeQuery
                Dim check_mol = registry.db_xrefs _
                    .where(field("db_key") = db_key,
                           field("xref") = item.ID) _
                    .find(Of biocad_registryModel.db_xrefs)

                Call substrates.add(
                    field("reaction") = check.id,
                    field("molecule_id") = If(check_mol Is Nothing, 0, check_mol.obj_id),
                    field("db_xref") = item.ID,
                    field("role") = leftKey,
                    field("factor") = If(item.Stoichiometry.IsNaNImaginary, 1, item.Stoichiometry),
                    field("note") = item.ToString
                )
            Next
            For Each item In reaction.right.SafeQuery
                Dim check_mol = registry.db_xrefs.where(field("db_key") = db_key, field("xref") = item.ID).find(Of biocad_registryModel.db_xrefs)

                Call substrates.add(
                    field("reaction") = check.id,
                    field("molecule_id") = If(check_mol Is Nothing, 0, check_mol.obj_id),
                    field("db_xref") = item.ID,
                    field("role") = rightKey,
                    field("factor") = If(item.Stoichiometry.IsNaNImaginary, 1, item.Stoichiometry),
                    field("note") = item.ToString
                )
            Next

            If reaction.ec_number IsNot Nothing Then
                For Each ec_id In reaction.ec_number
                    Call catalysis.add(
                        field("term") = ec_id.ECNumberString,
                        field("role") = enzyme,
                        field("reaction_id") = check.id
                    )
                Next
            End If
        Next

        Call substrates.commit()
        Call catalysis.commit()
    End Sub
End Class
