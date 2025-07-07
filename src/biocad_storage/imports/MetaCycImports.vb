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

    Public Sub ImportsReactions()
        Dim reactionList = metacyc.reactions.features.ToArray
        Dim leftKey = registry.getVocabulary("substrate", "Compound Role")
        Dim rightKey = registry.getVocabulary("product", "Compound Role")
        Dim db_key As UInteger = registry.getVocabulary("MetaCyc", "External Database")
        Dim substrates = registry.reaction_graph.open_transaction.ignore
        Dim enzyme = registry.getVocabulary("Enzymatic Catalysis", "Regulation Type")
        Dim catalysis = registry.regulation_graph.open_transaction.ignore

        For Each reaction In TqdmWrapper.Wrap(reactionList)
            Dim check = registry.reaction.where(field("db_xref") = reaction.uniqueId, field("source_dbkey") = db_key).find(Of biocad_registryModel.reaction)
            If check Is Nothing Then
                registry.reaction.add(
                    field("db_xref") = reaction.uniqueId, field("source_dbkey") = db_key,
                    field("name") = If(reaction.commonName, If(reaction.synonyms.IsNullOrEmpty, reaction.uniqueId, reaction.synonyms.JoinBy(" / "))),
                    field("equation") = reaction.equation.ToString,
                    field("note") = reaction.comment
                )
                check = registry.reaction.where(field("db_xref") = reaction.uniqueId, field("source_dbkey") = db_key).order_by("id", desc:=True).find(Of biocad_registryModel.reaction)
            End If

            For Each item In reaction.left.SafeQuery
                Dim check_mol = registry.db_xrefs.where(field("db_key") = db_key, field("xref") = item.ID).find(Of biocad_registryModel.db_xrefs)

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
                Call catalysis.add(
                    field("term") = reaction.ec_number.ECNumberString,
                    field("role") = enzyme,
                    field("reaction_id") = check.id
                )
            End If
        Next

        Call substrates.commit()
        Call catalysis.commit()
    End Sub
End Class
