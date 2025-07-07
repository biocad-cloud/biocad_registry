Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
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

            Call compoundSet.Add(meta)
        Next

        Call MetaboliteCommit.CommitMetabolites(compoundSet, registry)
        Call MetaboliteCommit.CommitDbXrefs(compoundSet, registry)
        Call MetaboliteCommit.CommitStructData(compoundSet, registry)
        Call MetaboliteCommit.CommitSynonyms(compoundSet, registry)
    End Sub
End Class
