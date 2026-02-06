Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text.Xml
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_exports
Imports SMRUCC.genomics.ComponentModel
Imports SMRUCC.genomics.ComponentModel.EquaionModel
Imports SMRUCC.genomics.Data.BioCyc

Module BioCycDatabase

    Public Sub ImportsMetaCycReactions(registry As biocad_registry, metacyc As Workspace)
        Dim models = metacyc.LoadModels.ToArray
        Call registry.importsReactions(models, db_name:="BioCyc")
    End Sub

    <Extension>
    Private Iterator Function LoadModels(metacyc As Workspace) As IEnumerable(Of EquaionModel.Reaction)
        Dim reactions As SMRUCC.genomics.Data.BioCyc.reactions() = metacyc.reactions.features.ToArray
        Dim filter = From r As SMRUCC.genomics.Data.BioCyc.reactions
                     In reactions
                     Where Not (r.left.IsNullOrEmpty OrElse r.right.IsNullOrEmpty)
                     Select r

        For Each r As SMRUCC.genomics.Data.BioCyc.reactions In filter
            Dim left As SideCompound() = r.left.Select(Function(c) New SideCompound With {.side = "left", .compound = New CompoundSpecies(c.ID)}).ToArray
            Dim right As SideCompound() = r.right.Select(Function(c) New SideCompound With {.side = "right", .compound = New CompoundSpecies(c.ID)}).ToArray

            Yield New SMRUCC.genomics.ComponentModel.EquaionModel.Reaction With {
                .entry = r.uniqueId,
                .comment = r.comment,
                .definition = r.commonName,
                .enzyme = r.ec_number.SafeQuery.Select(Function(e) e.ECNumberString).ToArray,
                .equation = r.equation,
                .compounds = left.JoinIterates(right).ToArray
            }
        Next
    End Function

    <Extension>
    Private Sub ImportsCompoundClass(registry As biocad_registry, compoundSet As AttrDataCollection(Of SMRUCC.genomics.Data.BioCyc.compounds))
        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim db_metacyc As UInteger = vocabulary.db_biocyc
        Dim superAtoms As New Index(Of String)

        ' save ontology group
        For Each cpd As SMRUCC.genomics.Data.BioCyc.compounds In compoundSet.features
            If Not cpd.superAtoms.StringEmpty Then
                Call superAtoms.Add(cpd.superAtoms)
            End If
        Next

        Dim classIndex As New Dictionary(Of String, UInteger)

        For Each id As String In TqdmWrapper.Wrap(superAtoms.Objects)
            ' subclass
            Dim subclass As SMRUCC.genomics.Data.BioCyc.compounds = compoundSet(id)
            Dim classinfo As SMRUCC.genomics.Data.BioCyc.compounds = compoundSet(subclass.types.DefaultFirst)

            Dim classNode As ontology = registry.ontology.where(field("term_id") = classinfo.uniqueId, field("ontology_id") = db_metacyc).find(Of ontology)
            Dim subclassNode As ontology = registry.ontology.where(field("term_id") = subclass.uniqueId, field("ontology_id") = db_metacyc).find(Of ontology)

            If classNode Is Nothing Then
                Call registry.ontology.add(field("term_id") = classinfo.uniqueId,
                                           field("ontology_id") = db_metacyc,
                                           field("term") = classinfo.commonName,
                                           field("note") = classinfo.comment)

                classNode = registry.ontology _
                    .where(field("term_id") = classinfo.uniqueId,
                           field("ontology_id") = db_metacyc) _
                    .find(Of ontology)
            End If
            If subclassNode Is Nothing Then
                Call registry.ontology.add(field("term_id") = subclass.uniqueId,
                                           field("ontology_id") = db_metacyc,
                                           field("term") = subclass.commonName,
                                           field("note") = subclass.comment)

                subclassNode = registry.ontology _
                    .where(field("term_id") = subclass.uniqueId,
                           field("ontology_id") = db_metacyc) _
                    .find(Of ontology)
            End If

            If classNode IsNot Nothing AndAlso subclassNode IsNot Nothing Then
                classIndex(classNode.term_id) = classNode.id
                classIndex(subclassNode.term_id) = subclassNode.id

                If registry.ontology_relation _
                    .where(field("term_id") = subclassNode.id, field("is_a") = classNode.id) _
                    .find(Of ontology_relation) Is Nothing Then

                    registry.ontology_relation.add(field("term_id") = subclassNode.id,
                                                   field("is_a") = classNode.id)
                End If
            End If
        Next
    End Sub

    Public Sub ImportsMetaCyc(registry As biocad_registry, metacyc As Workspace)
        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim metabolite_type As UInteger = vocabulary.GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id
        Dim db_metacyc As UInteger = vocabulary.db_biocyc
        Dim compoundSet As AttrDataCollection(Of SMRUCC.genomics.Data.BioCyc.compounds) = metacyc.compounds

        Call registry.ImportsCompoundClass(compoundSet)

        For Each cpd As SMRUCC.genomics.Data.BioCyc.compounds In TqdmWrapper.Wrap(compoundSet.features.ToArray)
            Dim formula As String = SMRUCC.genomics.Data.BioCyc.compounds.FormulaString(cpd)
            Dim dblinks = SMRUCC.genomics.Data.BioCyc.compounds.GetDbLinks(cpd).ToArray
            Dim dbgroups As Dictionary(Of String, String()) = dblinks _
                .GroupBy(Function(a) a.DBName.ToLower) _
                .ToDictionary(Function(a) a.Key,
                              Function(a)
                                  Return a.Select(Function(ai) ai.entry).ToArray
                              End Function)
            Dim meta As New MetaInfo With {
                .ID = cpd.uniqueId,
                .formula = formula,
                .exact_mass = FormulaScanner.EvaluateExactMass(formula),
                .description = cpd.comment,
                .name = XmlEntity.UnescapeHTML(cpd.commonName),
                .IUPACName = .name,
                .synonym = cpd.synonyms _
                    .SafeQuery _
                    .Select(Function(name) XmlEntity.UnescapeHTML(name)) _
                    .ToArray,
                .xref = New xref With {
                    .HMDB = dbgroups.TryGetValue("HMDB").DefaultFirst,
                    .chebi = dbgroups.TryGetValue("CHEBI").DefaultFirst,
                    .pubchem = dbgroups.TryGetValue("PUBCHEM").DefaultFirst,
                    .Wikipedia = dbgroups.TryGetValue("|Wikipedia|").DefaultFirst,
                    .CAS = dbgroups.TryGetValue("CAS"),
                    .DrugBank = dbgroups.TryGetValue("DRUGBANK").DefaultFirst,
                    .SMILES = cpd.SMILES,
                    .MetaCyc = cpd.uniqueId
                }
            }

            If meta.name = "" Then
                meta.name = meta.ID
            End If

            Dim m As metabolites = registry.FindMolecule(meta, "biocyc", nameSearch:=True)
            Dim term_id As ontology = Nothing

            If Not cpd.superAtoms.StringEmpty Then
                term_id = registry.ontology.where(field("term_id") = cpd.superAtoms).find(Of ontology)
            End If

            ' just ignores this error
            If m Is Nothing Then
                Continue For
            End If
            If term_id IsNot Nothing Then
                Call registry.metabolite_class.add(field("metabolite_id") = m.id, field("class_id") = term_id.id)
            End If

            Call registry.SaveDbLinks(meta, m, db_metacyc)
            Call registry.SaveStructureData(m, meta.xref.SMILES)
            Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_metacyc)
        Next
    End Sub
End Module
