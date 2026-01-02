
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
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
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

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

End Module
