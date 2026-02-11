Imports BioNovoGene.BioDeep.Chemistry
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
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
Imports SMRUCC.genomics.Data.BioCyc
Imports SMRUCC.genomics.Data.Regprecise
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports SMRUCC.genomics.SequenceModel.NucleotideModels
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

        For Each chunk As SpectraSection() In pull.populates(Of SpectraSection)(env).SplitIterator(5000)
            Call MoNADatabase.MakeImports(registry, chunk)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_spectraverse")>
    Public Function imports_spectraverse(registry As biocad_registry, <RRawVectorArgument> spectraverse As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of spectraverse)(spectraverse, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each chunk As spectraverse() In pull.populates(Of spectraverse)(env).SplitIterator(5000)
            Call MoNADatabase.MakeImports(registry, chunk)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_pubchem")>
    Public Function imports_pubchem(registry As biocad_registry, <RRawVectorArgument> pubchem As Object,
                                    Optional skip_from As UInteger = 0,
                                    Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of PugViewRecord)(pubchem, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim chunks = pull.populates(Of PugViewRecord)(env).Where(Function(c) Not c Is Nothing) _
            .Where(Function(c) UInteger.Parse(c.RecordNumber) > skip_from) _
            .SplitIterator(1000)
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
        Call BioCycDatabase.ImportsMetaCycReactions(registry, metacyc)
        Return Nothing
    End Function

    <ExportAPI("imports_metacyc_compounds")>
    Public Function imports_metacyc(registry As biocad_registry, metacyc As Workspace) As Object
        Call BioCycDatabase.ImportsMetaCyc(registry, metacyc)
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
