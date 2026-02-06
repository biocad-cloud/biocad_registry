Imports BioNovoGene.BioDeep.Chemistry
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.NCBI
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports Microsoft.VisualBasic.Text.Xml
Imports Microsoft.VisualBasic.Text.Xml.Models
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_exports
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.Motif
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.SequenceLogo
Imports SMRUCC.genomics.Assembly.KEGG
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.Uniprot.XML
Imports SMRUCC.genomics.ComponentModel.EquaionModel
Imports SMRUCC.genomics.Data.BioCyc
Imports SMRUCC.genomics.Data.Regprecise
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("registry")>
Module registry

    <ExportAPI("save_uniprot")>
    Public Function saveUniprot(registry As biocad_registry, <RRawVectorArgument> uniprot As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of entry)(uniprot, env)

        If pull.isError Then
            Return pull.getError
        End If

        Call registry.importsUniProt(pull.populates(Of entry)(env))

        Return Nothing
    End Function

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

    <ExportAPI("imports_mona")>
    Public Function imports_mona(registry As biocad_registry, <RRawVectorArgument> mona As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of SpectraSection)(mona, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim db_mona As UInteger = registry.biocad_vocabulary.GetDatabaseResource("MoNA").id
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type

        For Each chunk As SpectraSection() In pull.populates(Of SpectraSection)(env).SplitIterator(5000)
            For Each spectra As SpectraSection In TqdmWrapper.Wrap(chunk)
                spectra.name = Strings.Trim(spectra.name).Trim(""""c, " "c, "'"c)

                Dim clean_name As String = spectra.name

                If clean_name.IsPattern("NCGC\d+[-]\d+[!].+") Then
                    clean_name = clean_name.GetTagValue("!").Value.ToLower
                End If

                If clean_name.StartsWith("(((Cl)|[CHONPS])\d*)+_", RegexICMul) Then
                    clean_name = clean_name.GetTagValue("_").Value
                End If

                If clean_name = "" Then
                    clean_name = spectra.name
                End If

                ' check mona id reference
                Dim m As metabolites = registry.metabolites.getDriver.ExecuteScalar(Of biocad_registryModel.metabolites)($"
SELECT 
    *
FROM
    cad_registry.metabolites
WHERE
    id = (SELECT 
            obj_id
        FROM
            db_xrefs
        WHERE
            type = {metabolite_type} AND db_name = {db_mona}
                AND db_xref = '{spectra.ID}'
                AND db_source = {db_mona}
        LIMIT 1) 
LIMIT 1;")

                If m Is Nothing Then
                    m = registry.FindMolecule(spectra, "kegg_id", nameSearch:=True)
                End If

                If m Is Nothing Then
                    Continue For
                End If

                Call registry.SaveDbLinks(spectra, m, db_mona, saveID:=True)
                Call registry.SaveStructureData(m, spectra.xref.SMILES)
                Call registry.SaveSynonyms(m, spectra.synonym.JoinIterates({spectra.name, spectra.IUPACName}).Distinct, db_mona)
            Next
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
        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim db_pubchem As UInteger = vocabulary.db_pubchem

        For Each block As PugViewRecord() In chunks
            For Each meta As MetaInfo In TqdmWrapper.Wrap(block.Select(Function(c) c.GetMetaInfo).ToArray)
                ' 不信任pubchem id的映射结果，在这里直接设置kegg_id来避免直接通过pubchem id查找到结果
                Dim m As metabolites = registry.FindMolecule(meta, "kegg_id", nameSearch:=True)

                If m Is Nothing Then
                    Continue For
                End If

                Call registry.SaveDbLinks(meta, m, db_pubchem)
                Call registry.SaveStructureData(m, meta.xref.SMILES)
                Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_pubchem)
            Next
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_kegg_reactions")>
    Public Function imports_kegg_reactions(registry As biocad_registry, <RRawVectorArgument> kegg As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of DBGET.bGetObject.Reaction)(kegg, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim models As SMRUCC.genomics.ComponentModel.EquaionModel.Reaction() = pull _
            .populates(Of DBGET.bGetObject.Reaction)(env) _
            .Where(Function(r) Not r.ID.StringEmpty(, True)) _
            .Select(Function(r)
                        Return SMRUCC.genomics.ComponentModel.EquaionModel.Reaction.FromKeggReaction(r)
                    End Function) _
            .ToArray

        Call registry.importsReactions(models, db_name:="kegg")

        Return Nothing
    End Function

    <ExportAPI("imports_rhea")>
    Public Function imports_rhea(registry As biocad_registry, <RRawVectorArgument> rhea As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of SMRUCC.genomics.ComponentModel.EquaionModel.Reaction)(rhea, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim models = pull.populates(Of SMRUCC.genomics.ComponentModel.EquaionModel.Reaction)(env).ToArray

        Call registry.importsReactions(models, db_name:="Rhea")

        Return Nothing
    End Function

    <ExportAPI("imports_metacyc_reactions")>
    Public Function imports_metacyc_reactions(registry As biocad_registry, metacyc As Workspace) As Object
        Dim reactions = metacyc.reactions.features.ToArray
        Dim models = reactions _
            .Where(Function(r) Not (r.left.IsNullOrEmpty OrElse r.right.IsNullOrEmpty)) _
            .Select(Function(r)
                        Dim left As SideCompound() = r.left.Select(Function(c) New SideCompound With {.side = "left", .compound = New CompoundSpecies(c.ID)}).ToArray
                        Dim right As SideCompound() = r.right.Select(Function(c) New SideCompound With {.side = "right", .compound = New CompoundSpecies(c.ID)}).ToArray

                        Return New SMRUCC.genomics.ComponentModel.EquaionModel.Reaction With {
                            .entry = r.uniqueId,
                            .comment = r.comment,
                            .definition = r.commonName,
                            .enzyme = r.ec_number.SafeQuery.Select(Function(e) e.ECNumberString).ToArray,
                            .equation = r.equation,
                            .compounds = left.JoinIterates(right).ToArray
                        }
                    End Function) _
            .ToArray

        Call registry.importsReactions(models, db_name:="BioCyc")

        Return Nothing
    End Function

    <ExportAPI("imports_metacyc_compounds")>
    Public Function imports_metacyc(registry As biocad_registry, metacyc As Workspace) As Object
        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
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

        For Each id As String In TqdmWrapper.Wrap(superAtoms.Objects)
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

        For Each cpd As compounds In TqdmWrapper.Wrap(compoundSet.features.ToArray)
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

        Return Nothing
    End Function

    ''' <summary>
    ''' Save RegPrecise regulation network
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="genomes"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("save_TRN")>
    Public Function save_regprecise(registry As biocad_registry, <RRawVectorArgument> genomes As Object, Optional env As Environment = Nothing) As Object
        Dim database As pipeline = pipeline.TryCreatePipeline(Of BacteriaRegulome)(genomes, env)

        If database.isError Then
            Return database.getError
        End If

        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim db_regprecise As UInteger = vocabulary.GetDatabaseResource("RegPrecise").id
        Dim db_genbank As UInteger = vocabulary.db_genbank
        Dim TRN As CommitTransaction = registry.regulatory_network.ignore.open_transaction

        For Each genome As BacteriaRegulome In TqdmWrapper.Wrap(database.populates(Of BacteriaRegulome)(env).ToArray)
            Dim taxid As UInteger = genome.genome.taxonomyId

            For Each TF As SMRUCC.genomics.Data.Regprecise.Regulator In genome.regulome.AsEnumerable
                Dim motifPlaceholder As motif = registry.motif.where(field("name") = TF.regulog.name).find(Of motif)("id")

                If motifPlaceholder Is Nothing Then
                    Continue For
                End If

                Dim effects As Double = 1

                If Not TF.regulationMode.StringEmpty Then
                    If TF.regulationMode.StartsWith("repressor") Then
                        If Strings.InStr(TF.regulationMode, "activator") Then
                            effects = -0.5
                        Else
                            effects = -1
                        End If
                    Else
                        If Strings.InStr(TF.regulationMode, "repressor") Then
                            effects = 0.5
                        Else
                            effects = 1
                        End If
                    End If
                End If

                For Each tag As NamedValue In TF.locus_tags.SafeQuery
                    Dim protTF As protein_data = registry.protein_data _
                        .where(field("source_id") = tag.name,
                               field("source_db") = db_genbank,
                               field("ncbi_taxid") = taxid) _
                        .find(Of protein_data)("id", "ncbi_taxid")

                    If protTF IsNot Nothing Then
                        Call TRN.add(
                            field("regulator") = protTF.id,
                            field("motif_site") = motifPlaceholder.id,
                            field("effector_name") = TF.effector,
                            field("effector") = 0,
                            field("effects") = effects,
                            field("db_source") = db_regprecise,
                            field("note") = TF.regulationMode & " - " & TF.biological_process.JoinBy("; ")
                        )
                    End If
                Next
            Next
        Next

        Call TRN.commit()

        Return Nothing
    End Function

    <ExportAPI("update_logo")>
    Public Function update_logo(registry As biocad_registry) As Object
        Dim nethost As New NetworkByteOrderBuffer

        For Each model As motif In TqdmWrapper.Wrap(registry.motif.select(Of motif)("id", "name"))
            Dim sites = registry.nucleotide_data.where(field("is_motif") <> 0, field("model_id") = model.id).select(Of nucleotide_data)("source_id", "sequence")
            Dim fq As FastaSeq() = sites.Select(Function(si) New FastaSeq(si.sequence, si.source_id)).ToArray

            If fq.Length < 3 Then
                Continue For
            End If

            Dim gibbs As New GibbsSampler(fq, fq.Average(Function(si) si.Length) * 0.85)
            Dim motifdata As MSAMotif = gibbs.find
            Dim motif As MotifPWM = motifdata.CreateMotif
            Dim pwm As Double()() = motif
            Dim matrix As String = nethost.Base64String(pwm.IteratesALL, gzip:=False)
            Dim w As Integer = pwm.Length
            Dim logo = DrawingDevice.DrawFrequency(motif, title:=model.name, logoPadding:="padding:30% 5% 20% 8%;", driver:=Drivers.SVG)
            Dim logoUri As String = logo.GetDataURI.ToString

            ' 在这里分为两个步骤做这条记录的更新
            ' 避免可能出现的sql语句过长，数据量过大导致的问题
            Call registry.motif.where(field("id") = model.id).save(field("pwm") = matrix, field("width") = w)
            Call registry.motif.where(field("id") = model.id).save(field("logo") = logoUri)
        Next

        Return Nothing
    End Function

End Module
