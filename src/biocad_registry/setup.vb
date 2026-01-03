
Imports BioNovoGene.BioDeep.Chemistry
Imports BioNovoGene.BioDeep.Chemistry.LipidMaps
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
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
Imports SMRUCC.genomics.ComponentModel.DBLinkBuilder
Imports SMRUCC.genomics.Data.Regprecise
Imports SMRUCC.genomics.Data.Regtransbase.WebServices
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
        Dim pull As Compound() = keggLib.populates(Of Compound)(env).ToArray

        For Each cpd As Compound In TqdmWrapper.Wrap(pull)
            Dim m As metabolites = registry.metabolites _
                .where(field("kegg_id") = cpd.entry) _
                .find(Of metabolites)
            Dim db_xrefs As DBLink() = cpd.DbLinks
            Dim cas_id As String = db_xrefs.KeyItem("CAS")

            If m Is Nothing Then
                Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(cpd.formula)
                Dim name As String = cpd.commonNames.DefaultFirst([default]:=cpd.entry)

                registry.metabolites.add(
                    field("name") = name,
                    field("hashcode") = name.ToLower.MD5,
                    field("formula") = cpd.formula,
                    field("exact_mass") = If(exact_mass < 0, 0, exact_mass),
                    field("kegg_id") = cpd.entry,
                    field("cas_id") = cas_id
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

            registry.db_xrefs.ignore.add(field("db_source") = kegg_db,
                                         field("db_name") = kegg_db,
                                         field("db_xref") = cpd.entry,
                                         field("type") = metabolite_type,
                                         field("obj_id") = m.id)

            For Each name As String In cpd.commonNames.SafeQuery
                name = Strings.Trim(name)

                If name <> "" Then
                    registry.synonym.add(
                        field("obj_id") = m.id,
                        field("type") = metabolite_type,
                        field("db_source") = kegg_db,
                        field("synonym") = name,
                        field("hashcode") = Strings.LCase(name).MD5,
                        field("lang") = "en"
                    )
                End If
            Next
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
            Dim m As metabolites = Nothing
            Dim kegg_id As String = Strings.Trim(met.kegg_id)
            Dim pubchem_cid As String = Strings.Trim(met.pubchem_cid)
            Dim chebi_id As String = Strings.Trim(met.chebi_id)
            Dim name As String = Strings.Trim(met.refmet_name)
            Dim hashcode As String = Strings.Trim(met.refmet_name).ToLower.MD5
            Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(met.formula)
            Dim hmdb_id As String = Strings.Trim(met.hmdb_id)
            Dim lipidmaps_id As String = Strings.Trim(met.lipidmaps_id)

            If exact_mass < 0 Then
                exact_mass = 0
            End If

            If Not pubchem_cid.StringEmpty(, True) Then
                pubchem_cid = pubchem_cid.Match("\d+")
            Else
                pubchem_cid = Nothing
            End If
            If Not chebi_id.StringEmpty(, True) Then
                chebi_id = chebi_id.Match("\d+")
            Else
                chebi_id = Nothing
            End If

            If pubchem_cid = "" Then
                pubchem_cid = Nothing
            End If
            If chebi_id = "" Then
                chebi_id = Nothing
            End If

            If Not kegg_id.StringEmpty(, True) Then
                m = registry.metabolites.where(field("kegg_id") = met.kegg_id).find(Of metabolites)
            End If

            If m Is Nothing Then
                Dim obj = registry.db_xrefs.where(field("type") = metabolite_type,
                                                  field("db_name") = refmet_db,
                                                  field("db_xref") = met.refmet_id).find(Of db_xrefs)
                If obj Is Nothing Then
                    registry.metabolites.add(
                        field("name") = name,
                        field("hashcode") = hashcode,
                        field("formula") = met.formula,
                        field("exact_mass") = exact_mass,
                        field("pubchem_cid") = pubchem_cid,
                        field("chebi_id") = chebi_id,
                        field("hmdb_id") = met.hmdb_id,
                        field("lipidmaps_id") = met.lipidmaps_id,
                        field("kegg_id") = met.kegg_id
                    )

                    m = registry.metabolites _
                        .where(field("hashcode") = hashcode,
                               field("exact_mass").between(exact_mass - 0.5, exact_mass + 0.5)) _
                        .order_by("id", desc:=True) _
                        .find(Of metabolites)
                Else
                    m = registry.metabolites.where(field("id") = obj.obj_id).find(Of metabolites)
                End If
            End If
            If m Is Nothing Then
                Continue For
            End If

            Dim updates As New List(Of FieldAssert)

            If m.kegg_id.StringEmpty(, True) AndAlso Not kegg_id.StringEmpty Then
                updates.Add(field("kegg_id") = kegg_id)
            End If
            If m.chebi_id = 0 AndAlso Not chebi_id Is Nothing Then
                updates.Add(field("chebi_id") = chebi_id)
            End If
            If m.pubchem_cid = 0 AndAlso Not pubchem_cid Is Nothing Then
                updates.Add(field("pubchem_cid") = pubchem_cid)
            End If
            If m.hmdb_id.StringEmpty(, True) AndAlso Not hmdb_id.StringEmpty(, True) Then
                updates.Add(field("hmdb_id") = hmdb_id)
            End If
            If m.lipidmaps_id.StringEmpty AndAlso Not lipidmaps_id.StringEmpty(, True) Then
                updates.Add(field("lipidmaps_id") = lipidmaps_id)
            End If
            If updates.Any Then
                Call registry.metabolites.where(field("id") = m.id).save(updates.ToArray)
            End If

            If Not pubchem_cid.StringEmpty Then
                registry.db_xrefs.ignore.add(field("db_source") = refmet_db, field("db_name") = vocabulary.db_pubchem, field("db_xref") = pubchem_cid, field("type") = metabolite_type, field("obj_id") = m.id)
            End If
            If Not chebi_id.StringEmpty Then
                chebi_id = $"ChEBI:{chebi_id}"
                registry.db_xrefs.ignore.add(field("db_source") = refmet_db, field("db_name") = vocabulary.db_chebi, field("db_xref") = chebi_id, field("type") = metabolite_type, field("obj_id") = m.id)
            End If
            If Not met.hmdb_id.StringEmpty Then
                registry.db_xrefs.ignore.add(field("db_source") = refmet_db, field("db_name") = vocabulary.db_hmdb, field("db_xref") = met.hmdb_id, field("type") = metabolite_type, field("obj_id") = m.id)
            End If
            If Not met.lipidmaps_id.StringEmpty Then
                registry.db_xrefs.ignore.add(field("db_source") = refmet_db, field("db_name") = vocabulary.db_lipidmaps, field("db_xref") = met.lipidmaps_id, field("type") = metabolite_type, field("obj_id") = m.id)
            End If
            If Not met.kegg_id.StringEmpty Then
                registry.db_xrefs.ignore.add(field("db_source") = refmet_db, field("db_name") = vocabulary.db_kegg, field("db_xref") = met.kegg_id, field("type") = metabolite_type, field("obj_id") = m.id)
            End If

            registry.db_xrefs.ignore.add(field("db_source") = refmet_db, field("db_name") = refmet_db, field("db_xref") = met.refmet_id, field("type") = metabolite_type, field("obj_id") = m.id)

            Dim super_class As ontology = registry.ontology.where(field("ontology_id") = refmet_db, field("term_id") = met.super_class).find(Of ontology)

            If super_class Is Nothing Then
                registry.ontology.add(field("ontology_id") = refmet_db, field("term_id") = met.super_class, field("term") = met.super_class)
                super_class = registry.ontology.where(field("ontology_id") = refmet_db, field("term_id") = met.super_class).find(Of ontology)
            End If

            Dim main_class As ontology = registry.ontology.where(field("ontology_id") = refmet_db, field("term_id") = met.main_class).find(Of ontology)

            If main_class Is Nothing Then
                registry.ontology.add(field("ontology_id") = refmet_db, field("term_id") = met.main_class, field("term") = met.main_class)
                main_class = registry.ontology.where(field("ontology_id") = refmet_db, field("term_id") = met.main_class).find(Of ontology)
                registry.ontology_relation.add(field("term_id") = main_class.id, field("is_a") = super_class.id)
            End If

            Dim sub_class As ontology = registry.ontology.where(field("ontology_id") = refmet_db, field("term_id") = met.sub_class).find(Of ontology)

            If sub_class Is Nothing Then
                registry.ontology.add(field("ontology_id") = refmet_db, field("term_id") = met.sub_class, field("term") = met.sub_class)
                sub_class = registry.ontology.where(field("ontology_id") = refmet_db, field("term_id") = met.sub_class).find(Of ontology)
                registry.ontology_relation.add(field("term_id") = sub_class.id, field("is_a") = main_class.id)
            End If

            Call registry.metabolite_class.add(field("metabolite_id") = m.id, field("class_id") = sub_class.id, field("note") = met.refmet_id)

            registry.synonym.add(
                field("obj_id") = m.id,
                field("type") = metabolite_type,
                field("db_source") = refmet_db,
                field("synonym") = name,
                field("hashcode") = Strings.LCase(name).MD5,
                field("lang") = "en"
            )
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
            Dim pubchem_cid As String = Strings.Trim(met.pubchem_compound_id)
            Dim chebi_id As String = Strings.Trim(met.chebi_id)
            Dim name As String = Strings.Trim(met.name)
            Dim hashcode As String = name.ToLower.MD5
            Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(met.chemical_formula)

            If exact_mass < 0 Then
                exact_mass = 0
            End If

            If Not pubchem_cid.StringEmpty(, True) Then
                pubchem_cid = pubchem_cid.Match("\d+")
            Else
                pubchem_cid = Nothing
            End If
            If Not chebi_id.StringEmpty(, True) Then
                chebi_id = chebi_id.Match("\d+")
            Else
                chebi_id = Nothing
            End If

            If pubchem_cid = "" Then
                pubchem_cid = Nothing
            End If
            If chebi_id = "" Then
                chebi_id = Nothing
            End If

            Dim meta As MetaLib = TMIC.HMDB.CreateReferenceData(met)
            Dim m As metabolites = registry.metabolites.where(field("hmdb_id") = meta.ID).find(Of metabolites)

            If m Is Nothing Then
                Call registry.metabolites.add(
                    field("name") = name,
                    field("hashcode") = hashcode,
                    field("formula") = met.chemical_formula,
                    field("exact_mass") = exact_mass,
                    field("cas_id") = meta.xref.CAS.DefaultFirst,
                    field("pubchem_cid") = pubchem_cid,
                    field("chebi_id") = chebi_id,
                    field("hmdb_id") = meta.ID,
                    field("lipidmaps_id") = meta.xref.lipidmaps,
                    field("kegg_id") = meta.xref.KEGG,
                    field("biocyc") = meta.xref.MetaCyc,
                    field("mesh_id") = meta.xref.MeSH,
                    field("wikipedia") = meta.xref.Wikipedia,
                    field("note") = meta.description
                )
                m = registry.metabolites.where(field("hmdb_id") = meta.ID).order_by("id", desc:=True).find(Of metabolites)
            ElseIf m.note.StringEmpty(, True) Then
                registry.metabolites.where(field("id") = m.id).save(field("note") = meta.description)
            End If

            Dim model As registry_resolver = registry.registry_resolver.where(field("type") = metabolite_type, field("symbol_id") = m.id).find(Of registry_resolver)

            If model Is Nothing Then
                Dim model_symbol As String = met.name.makeSymbol

                registry.registry_resolver.add(
                    field("register_name") = model_symbol,
                    field("type") = metabolite_type,
                    field("symbol_id") = m.id
                )
                model = registry.registry_resolver.where(field("type") = metabolite_type, field("symbol_id") = m.id).find(Of registry_resolver)
            End If

            If model IsNot Nothing AndAlso met.biological_properties IsNot Nothing Then
                Dim biospecimen As String() = met.biological_properties.biospecimen_locations.biospecimen
                Dim cellular_locations As String() = met.biological_properties.cellular_locations.cellular
                Dim tissues As String() = met.biological_properties.tissue_locations.tissue

                For Each topic As String In biospecimen.JoinIterates(cellular_locations).JoinIterates(tissues)
                    If Not topic.StringEmpty(, True) Then
                        Dim term As vocabulary = vocabulary.GetTopic(topic)

                        Call registry.topic.ignore.add(
                            field("topic_id") = term.id,
                            field("model_id") = model.id,
                            field("note") = meta.ID
                        )
                    End If
                Next
            End If

            Call registry.SaveDbLinks(vocabulary, meta, m, db_hmdb, pubchem_cid, chebi_id)
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

        For Each lipid As LipidMaps.MetaData In pull.populates(Of LipidMaps.MetaData)(env)
            Dim pubchem_cid As String = Strings.Trim(lipid.PUBCHEM_CID)
            Dim chebi_id As String = Strings.Trim(lipid.CHEBI_ID)
            Dim name As String = Strings.Trim(lipid.NAME)
            Dim hashcode As String = name.ToLower.MD5
            Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(lipid.FORMULA)

            If exact_mass < 0 Then
                exact_mass = 0
            End If

            If Not pubchem_cid.StringEmpty(, True) Then
                pubchem_cid = pubchem_cid.Match("\d+")
            Else
                pubchem_cid = Nothing
            End If
            If Not chebi_id.StringEmpty(, True) Then
                chebi_id = chebi_id.Match("\d+")
            Else
                chebi_id = Nothing
            End If

            If pubchem_cid = "" Then
                pubchem_cid = Nothing
            End If
            If chebi_id = "" Then
                chebi_id = Nothing
            End If

            Dim meta As MetaLib = lipid.CreateMetabolite
            Dim m As metabolites = registry.metabolites.where(field("lipidmaps_id") = meta.ID).find(Of metabolites)

            If m Is Nothing Then
                Call registry.metabolites.add(
                    field("name") = name,
                    field("hashcode") = hashcode,
                    field("formula") = lipid.FORMULA,
                    field("exact_mass") = exact_mass,
                    field("cas_id") = meta.xref.CAS.DefaultFirst,
                    field("pubchem_cid") = pubchem_cid,
                    field("chebi_id") = chebi_id,
                    field("hmdb_id") = meta.xref.HMDB,
                    field("lipidmaps_id") = meta.xref.lipidmaps,
                    field("kegg_id") = meta.xref.KEGG,
                    field("biocyc") = meta.xref.MetaCyc,
                    field("mesh_id") = meta.xref.MeSH,
                    field("wikipedia") = meta.xref.Wikipedia,
                    field("note") = meta.description
                )
                m = registry.metabolites.where(field("lipidmaps_id") = meta.ID).order_by("id", desc:=True).find(Of metabolites)
            ElseIf m.note.StringEmpty(, True) Then
                registry.metabolites.where(field("id") = m.id).save(field("note") = meta.description)
            End If

            Call registry.SaveDbLinks(vocabulary, meta, m, db_lipidmaps, pubchem_cid, chebi_id)
            Call registry.SaveStructureData(m, meta.xref.SMILES)
            Call registry.SaveMetaboliteClass(m, ontology_id, (meta.kingdom, meta.super_class, meta.class, meta.sub_class), meta.ID)
            Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_lipidmaps)
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

                Next
            Next
        Next

        Return Nothing
    End Function
End Module
