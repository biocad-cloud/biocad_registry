Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Text.Xml
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Data.BioCyc
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("registry")>
Module registry

    <ExportAPI("save_genbank")>
    Public Function save_genbank(registry As biocad_registry,
                                 <RRawVectorArgument>
                                 genbank As Object,
                                 Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of GBFF.File)(genbank, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each gb_asm As GBFF.File In pull.populates(Of GBFF.File)(env)
            Call registry.SaveGenBank(gb_asm)
        Next

        Return Nothing
    End Function

    <ExportAPI("make_genbank_dbxrefs")>
    Public Function save_genbank_xrefs(registry As biocad_registry,
                                       <RRawVectorArgument>
                                       genbank As Object,
                                       Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of GBFF.File)(genbank, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each gb_asm As GBFF.File In pull.populates(Of GBFF.File)(env)
            Call registry.SaveDbXrefs(gb_asm)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_pubchem")>
    Public Function imports_pubchem(registry As biocad_registry, <RRawVectorArgument> pubchem As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of PugViewRecord)(pubchem, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim chunks = pull.populates(Of PugViewRecord)(env).Where(Function(c) Not c Is Nothing).SplitIterator(1000)
        Dim vocabulary As New biocad_vocabulary(registry)
        Dim db_pubchem As UInteger = vocabulary.db_pubchem

        For Each block As PugViewRecord() In chunks
            For Each meta As MetaInfo In TqdmWrapper.Wrap(block.Select(Function(c) c.GetMetaInfo).ToArray)
                ' 不信任pubchem id的映射结果，在这里直接设置kegg_id来避免直接通过pubchem id查找到结果
                Dim m As metabolites = registry.FindMolecule(meta, "kegg_id", nameSearch:=True)

                If m Is Nothing Then
                    Continue For
                End If

                Call registry.SaveDbLinks(vocabulary, meta, m, db_pubchem)
                Call registry.SaveStructureData(m, meta.xref.SMILES)
                Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_pubchem)
            Next
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_metacyc")>
    Public Function imports_metacyc(registry As biocad_registry, metacyc As Workspace) As Object
        Dim vocabulary As New biocad_vocabulary(registry)
        Dim metabolite_type As UInteger = vocabulary.GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id
        Dim db_metacyc As UInteger = vocabulary.db_biocyc
        Dim superAtoms As New Index(Of String)
        Dim compoundSet As AttrDataCollection(Of compounds) = metacyc.compounds

        ' save ontology group
        For Each cpd As compounds In compoundSet.features
            If Not cpd.superAtoms.StringEmpty Then
                Call superAtoms.Add(cpd.superAtoms)
            End If
        Next

        Dim classIndex As New Dictionary(Of String, UInteger)

        For Each id As String In superAtoms.Objects
            ' subclass
            Dim subclass As compounds = compoundSet(id)
            Dim classinfo As compounds = compoundSet(subclass.types.DefaultFirst)

            Dim classNode As ontology = registry.ontology.where(field("term_id") = classinfo.uniqueId, field("ontology_id") = db_metacyc).find(Of ontology)
            Dim subclassNode As ontology = registry.ontology.where(field("term_id") = subclass.uniqueId, field("ontology_id") = db_metacyc).find(Of ontology)

            If classNode Is Nothing Then
                Call registry.ontology.add(field("term_id") = classinfo.uniqueId, field("ontology_id") = db_metacyc, field("term") = classinfo.commonName, field("note") = classinfo.comment)
                classNode = registry.ontology.where(field("term_id") = classinfo.uniqueId, field("ontology_id") = db_metacyc).find(Of ontology)
            End If
            If subclassNode Is Nothing Then
                Call registry.ontology.add(field("term_id") = subclass.uniqueId, field("ontology_id") = db_metacyc, field("term") = subclass.commonName, field("note") = subclass.comment)
                subclassNode = registry.ontology.where(field("term_id") = subclass.uniqueId, field("ontology_id") = db_metacyc).find(Of ontology)
            End If

            If classNode IsNot Nothing AndAlso subclassNode IsNot Nothing Then
                classIndex(classNode.term_id) = classNode.id
                classIndex(subclassNode.term_id) = subclassNode.id

                If registry.ontology_relation.where(field("term_id") = subclassNode.id, field("is_a") = classNode.id).find(Of ontology_relation) Is Nothing Then
                    registry.ontology_relation.add(field("term_id") = subclassNode.id, field("is_a") = classNode.id)
                End If
            End If
        Next

        For Each cpd As compounds In compoundSet.features
            Dim formula As String = compounds.FormulaString(cpd)
            Dim dblinks = compounds.GetDbLinks(cpd).ToArray
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
                    .SMILES = cpd.SMILES
                }
            }

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

            Call registry.SaveDbLinks(vocabulary, meta, m, db_metacyc)
            Call registry.SaveStructureData(m, meta.xref.SMILES)
            Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_metacyc)
        Next

        Return Nothing
    End Function
End Module
