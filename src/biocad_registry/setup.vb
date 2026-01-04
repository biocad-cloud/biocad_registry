
Imports BioNovoGene.BioDeep.Chemistry
Imports BioNovoGene.BioDeep.Chemistry.ChEBI
Imports BioNovoGene.BioDeep.Chemistry.LipidMaps
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.TMIC.HMDB
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Assembly.KEGG.DBGET.bGetObject
Imports SMRUCC.genomics.Data
Imports SMRUCC.genomics.Data.Regprecise
Imports SMRUCC.genomics.Data.Regtransbase.WebServices
Imports SMRUCC.genomics.foundation.OBO_Foundry
Imports SMRUCC.genomics.foundation.OBO_Foundry.IO.Models
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports ontology = registry_data.biocad_registryModel.ontology

''' <summary>
''' The Initial setup of the database
''' </summary>
''' 
<Package("setup")>
Public Module setup

    <ExportAPI("setup_kegg")>
    Public Function setup_kegg(registry As biocad_registry,
                               <RRawVectorArgument>
                               kegg As Object,
                               Optional env As Environment = Nothing) As Object

        Dim keggLib As pipeline = pipeline.TryCreatePipeline(Of Compound)(kegg, env)
        Dim vocabulary As New biocad_vocabulary(registry)

        If keggLib.isError Then
            Return keggLib.getError
        End If

        Dim kegg_db As UInteger = vocabulary.db_kegg
        Dim metabolite_type As UInteger = vocabulary.GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id
        Dim pull As Compound() = keggLib.populates(Of Compound)(env).OrderBy(Function(a) a.commonNames.DefaultFirst).ToArray

        For Each cpd As Compound In TqdmWrapper.Wrap(pull)
            Dim m As metabolites = registry.metabolites _
                .where(field("kegg_id") = cpd.entry) _
                .find(Of metabolites)
            Dim db_xrefs As xref = cpd.Xref
            Dim cas_id As String = db_xrefs.CAS.DefaultFirst
            Dim chebi As String = db_xrefs.chebi.Match("\d+")

            If chebi = "" Then
                chebi = Nothing
            End If

            If m Is Nothing Then
                Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(cpd.formula)
                Dim name As String = cpd.commonNames.DefaultFirst([default]:=cpd.entry)

                registry.metabolites.add(
                    field("name") = name,
                    field("hashcode") = name.ToLower.MD5,
                    field("formula") = cpd.formula,
                    field("exact_mass") = If(exact_mass < 0, 0, exact_mass),
                    field("kegg_id") = cpd.entry,
                    field("cas_id") = cas_id,
                    field("chebi_id") = chebi
                )
                m = registry.metabolites _
                    .where(field("kegg_id") = cpd.entry) _
                    .order_by("id", desc:=True) _
                    .find(Of metabolites)
            ElseIf m.cas_id.StringEmpty(, True) Then
                registry.metabolites.where(field("id") = m.id).save(field("cas_id") = cas_id)
            End If

            If Not cas_id.StringEmpty(, True) Then
                registry.db_xrefs.ignore.add(field("db_source") = kegg_db,
                                             field("db_name") = vocabulary.db_cas,
                                             field("db_xref") = cas_id,
                                             field("type") = metabolite_type,
                                             field("obj_id") = m.id)
            End If
            If Not chebi.StringEmpty(, True) Then
                registry.db_xrefs.ignore.add(field("db_source") = kegg_db,
                                             field("db_name") = vocabulary.db_chebi,
                                             field("db_xref") = "ChEBI:" & chebi,
                                             field("type") = metabolite_type,
                                             field("obj_id") = m.id)
            End If

            registry.db_xrefs.ignore.add(field("db_source") = kegg_db,
                                         field("db_name") = kegg_db,
                                         field("db_xref") = cpd.entry,
                                         field("type") = metabolite_type,
                                         field("obj_id") = m.id)

            Call registry.SaveSynonyms(m, cpd.commonNames, kegg_db)
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' use kegg+refmet as template for make setup of the metabolites table
    ''' </summary>
    ''' <param name="refmet"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("setup_refmet")>
    Public Function setup_metabolites(registry As biocad_registry,
                                      <RRawVectorArgument>
                                      refmet As Object,
                                      Optional env As Environment = Nothing) As Object

        Dim refmetLib As pipeline = pipeline.TryCreatePipeline(Of RefMet)(refmet, env)
        Dim vocabulary As New biocad_vocabulary(registry)
        Dim metabolite_type As UInteger = vocabulary.GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id

        If refmetLib.isError Then
            Return refmetLib.getError
        End If

        Dim refmet_db As UInteger = vocabulary.GetDatabaseResource("RefMet").id
        Dim pull As RefMet() = refmetLib.populates(Of RefMet)(env).ToArray

        For Each met As RefMet In TqdmWrapper.Wrap(pull)
            Dim meta As MetaLib = met.CastModel
            Dim m As metabolites = registry.FindMolecule(meta, "kegg_id")

            registry.db_xrefs.ignore.add(
                field("db_source") = refmet_db,
                field("db_name") = refmet_db,
                field("db_xref") = met.refmet_id,
                field("type") = metabolite_type,
                field("obj_id") = m.id
            )

            Call registry.SaveDbLinks(vocabulary, meta, m, refmet_db)
            Call registry.SaveSynonyms(m, {m.name}, refmet_db)
            Call registry.SaveMetaboliteClass(m, refmet, (met.super_class, met.main_class, met.sub_class, Nothing), met.refmet_id)
        Next

        Return Nothing
    End Function

    <ExportAPI("setup_hmdb")>
    Public Function setup_hmdb(registry As biocad_registry,
                               <RRawVectorArgument>
                               hmdb As Object,
                               Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of metabolite)(hmdb, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim vocabulary As New biocad_vocabulary(registry)
        Dim metabolite_type As UInteger = vocabulary.GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id
        Dim db_hmdb As UInteger = vocabulary.db_hmdb
        Dim ontology_id As UInteger = vocabulary.GetVocabulary(biocad_vocabulary.ExternalDatabase, "WishartLab ClassyFire").id

        For Each met As metabolite In pull.populates(Of metabolite)(env)
            Dim meta As MetaLib = TMIC.HMDB.CreateReferenceData(met)
            Dim m As metabolites = registry.FindMolecule(meta, primaryKey:="hmdb_id")
            Dim model As registry_resolver = registry.registry_resolver.where(field("type") = metabolite_type, field("symbol_id") = m.id).find(Of registry_resolver)

            If model Is Nothing Then
                Dim model_symbol As String = met.name.makeSymbol

                registry.registry_resolver.add(
                    field("register_name") = model_symbol,
                    field("type") = metabolite_type,
                    field("symbol_id") = m.id
                )
                model = registry.registry_resolver.where(field("type") = metabolite_type,
                                                         field("symbol_id") = m.id).find(Of registry_resolver)
            End If

            If model IsNot Nothing AndAlso met.biological_properties IsNot Nothing Then
                Dim biospecimen As String() = met.biological_properties.biospecimen_locations.biospecimen
                Dim cellular_locations As String() = met.biological_properties.cellular_locations.cellular
                Dim tissues As String() = met.biological_properties.tissue_locations.tissue
                Dim trans As CommitTransaction = registry.topic.open_transaction.ignore

                For Each topic As String In biospecimen.JoinIterates(tissues)
                    If Not topic.StringEmpty(, True) Then
                        Dim term As vocabulary = vocabulary.GetTopic(topic)

                        Call trans.ignore.add(
                            field("topic_id") = term.id,
                            field("model_id") = model.id,
                            field("note") = meta.ID
                        )
                    End If
                Next

                For Each location As String In cellular_locations.SafeQuery
                    Dim loc = registry.compartment_location.where(field("name") = location).find(Of compartment_location)

                    If loc Is Nothing Then
                        registry.compartment_location.add(field("name") = location, field("fullname") = location)
                        loc = registry.compartment_location.where(field("name") = location).find(Of compartment_location)
                    End If

                    Call trans.add(
                        registry.compartment_enrich.ignore.add_sql(
                            field("metabolite_id") = m.id,
                            field("location_id") = loc.id,
                            field("evidence") = met.accession
                        )
                    )
                Next

                Call trans.commit()
            End If

            Call registry.SaveDbLinks(vocabulary, meta, m, db_hmdb)
            Call registry.SaveStructureData(m, meta.xref.SMILES)
            Call registry.SaveMetaboliteClass(m, ontology_id, (meta.kingdom, meta.super_class, meta.class, meta.sub_class), meta.ID)
            Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_hmdb)
        Next

        Return Nothing
    End Function

    <ExportAPI("setup_lipidmaps")>
    Public Function setup_lipidmaps(registry As biocad_registry,
                                    <RRawVectorArgument>
                                    lipidmaps As Object,
                                    Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of LipidMaps.MetaData)(lipidmaps, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim vocabulary As New biocad_vocabulary(registry)
        Dim metabolite_type As UInteger = vocabulary.GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id
        Dim db_lipidmaps As UInteger = vocabulary.db_lipidmaps
        Dim ontology_id As UInteger = db_lipidmaps

        For Each lipid As LipidMaps.MetaData In TqdmWrapper.Wrap(pull.populates(Of LipidMaps.MetaData)(env).ToArray)
            Dim meta As MetaLib = lipid.CreateMetabolite
            Dim m As metabolites = registry.FindMolecule(meta, "lipidmaps_id")

            Call registry.SaveDbLinks(vocabulary, meta, m, db_lipidmaps)
            Call registry.SaveStructureData(m, meta.xref.SMILES)
            Call registry.SaveMetaboliteClass(m, ontology_id, (meta.kingdom, meta.super_class, meta.class, meta.sub_class), meta.ID)
            Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_lipidmaps)
        Next

        Return Nothing
    End Function

    <ExportAPI("setup_chebi")>
    Public Function saveChebi(registry As biocad_registry, chebi As OBOFile, Optional env As Environment = Nothing) As Object
        Dim metabolites As Models.MetaInfo() = ChEBIObo.ImportsMetabolites(chebi).ToArray
        Dim vocabulary As New biocad_vocabulary(registry)
        Dim metabolite_type As UInteger = vocabulary.GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id
        Dim db_chebi As UInteger = vocabulary.db_chebi
        Dim terms As BasicTerm() = chebi.GetRawTerms.Select(Function(t) t.ExtractBasic).ToArray

        Using trans As CommitTransaction = registry.ontology.ignore.open_transaction
            For Each term As BasicTerm In terms
                Call trans.ignore.add(
                    field("term_id") = term.id,
                    field("term") = term.name,
                    field("ontology_id") = db_chebi,
                    field("note") = term.def
                )
            Next
        End Using

        Using trans As CommitTransaction = registry.ontology_relation.ignore.open_transaction
            For Each term As BasicTerm In terms
                Dim term_id As ontology = registry.ontology.where(field("term_id") = term.id).find(Of ontology)

                For Each is_a As String In term.is_a.SafeQuery
                    Dim parent As ontology = registry.ontology.where(field("term_id") = is_a).find(Of ontology)

                    Call trans.ignore.add(
                        field("term_id") = term_id.id,
                        field("is_a") = parent.id
                    )
                Next
            Next
        End Using

        For Each meta As Models.MetaInfo In TqdmWrapper.Wrap(metabolites)
            Dim m As metabolites = registry.FindMolecule(meta, "chebi_id")
            Dim term_id As ontology = registry.ontology.where(field("term_id") = meta.ID).find(Of ontology)

            Call registry.metabolite_class.add(field("metabolite_id") = m.id, field("class_id") = term_id.id)
            Call registry.SaveDbLinks(vocabulary, meta, m, db_chebi)
            Call registry.SaveStructureData(m, meta.xref.SMILES)
            Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_chebi)
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Save RegPrecise motif sites 
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="genomes"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("save_motif")>
    Public Function save_regprecise(registry As biocad_registry, <RRawVectorArgument> genomes As Object, Optional env As Environment = Nothing) As Object
        Dim database As pipeline = pipeline.TryCreatePipeline(Of BacteriaRegulome)(genomes, env)

        If database.isError Then
            Return database.getError
        End If

        Dim vocabulary As New biocad_vocabulary(registry)
        Dim db_regprecise As UInteger = vocabulary.GetDatabaseResource("RegPrecise").id

        For Each genome As BacteriaRegulome In TqdmWrapper.Wrap(database.populates(Of BacteriaRegulome)(env).ToArray)
            Dim taxid As UInteger = genome.genome.taxonomyId

            For Each TF As SMRUCC.genomics.Data.Regprecise.Regulator In genome.regulome.AsEnumerable
                Dim motifPlaceholder As motif = registry.motif.where(field("name") = TF.regulog.name).find(Of motif)

                If motifPlaceholder Is Nothing Then
                    registry.motif.add(field("name") = TF.regulog.name,
                                       field("family") = TF.family,
                                       field("pwm") = "",
                                       field("width") = 0,
                                       field("note") = TF.regulationMode & " - " & If(TF.pathway, TF.biological_process.JoinBy(";")))
                    motifPlaceholder = registry.motif.where(field("name") = TF.regulog.name).find(Of motif)
                End If

                For Each site As MotifFasta In TF.regulatorySites.SafeQuery
                    Dim id As String = $"{site.locus_tag}:{site.position}"
                    Dim check = registry.nucleotide_data _
                        .where(field("source_id") = id,
                               field("source_db") = db_regprecise,
                               field("model_id") = motifPlaceholder.id,
                               field("organism_source") = taxid) _
                        .find(Of nucleotide_data)

                    If check Is Nothing Then
                        Dim seq = Strings.UCase(Regtransbase.WebServices.Regulator.SequenceTrimming(site.SequenceData)).Replace("-", "N")

                        registry.nucleotide_data.add(
                            field("source_id") = id,
                            field("source_db") = db_regprecise,
                            field("name") = If(site.name.StringEmpty, id, $"{site.name}:{site.position}"),
                            field("function") = "",
                            field("is_motif") = 1,
                            field("left") = 0,
                            field("strand") = "+",
                            field("operon_id") = 0,
                            field("model_id") = motifPlaceholder.id,
                            field("organism_source") = taxid,
                            field("sequence") = seq,
                            field("checksum") = seq.MD5
                        )
                    End If
                Next
            Next
        Next

        Return Nothing
    End Function
End Module
