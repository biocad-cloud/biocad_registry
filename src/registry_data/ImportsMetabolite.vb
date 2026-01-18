Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel
Imports std = System.Math

Public Module ImportsMetabolite

    <Extension>
    Public Sub SaveStructureData(registry As biocad_registry,
                                 m As metabolites,
                                 smiles As String)

        If Not smiles.StringEmpty Then
            Dim struct = registry.struct_data.where(field("metabolite_id") = m.id).find(Of struct_data)

            If struct Is Nothing Then
                registry.struct_data.add(
                    field("metabolite_id") = m.id,
                    field("checksum") = smiles.MD5,
                    field("smiles") = smiles
                )
            End If
        End If
    End Sub

    ''' <summary>
    ''' save class information into ontology table
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="m"></param>
    ''' <param name="ontology_id"></param>
    ''' <param name="ont"></param>
    ''' <param name="source_id"></param>
    <Extension>
    Public Sub SaveMetaboliteClass(registry As biocad_registry,
                                   m As metabolites,
                                   ontology_id As UInteger,
                                   ont As (kingdom$, super_class$, class$, sub_class$),
                                   source_id As String)
        Dim kingdom As ontology
        Dim super_class As ontology
        Dim [class] As ontology
        Dim sub_class As ontology
        Dim node As ontology = Nothing

        If Not ont.kingdom.StringEmpty Then
            kingdom = registry.ontology _
                .where(field("ontology_id") = ontology_id,
                       field("term_id") = ont.kingdom) _
                .find(Of ontology)

            If kingdom Is Nothing Then
                registry.ontology.add(field("ontology_id") = ontology_id,
                                      field("term_id") = ont.kingdom,
                                      field("term") = ont.kingdom)
                kingdom = registry.ontology _
                    .where(field("ontology_id") = ontology_id,
                           field("term_id") = ont.kingdom) _
                    .find(Of ontology)
            End If

            If kingdom IsNot Nothing Then node = kingdom

            If kingdom IsNot Nothing AndAlso Not ont.super_class.StringEmpty Then
                super_class = registry.ontology _
                    .where(field("ontology_id") = ontology_id,
                           field("term_id") = ont.super_class) _
                    .find(Of ontology)

                If super_class Is Nothing Then
                    registry.ontology.add(field("ontology_id") = ontology_id,
                                          field("term_id") = ont.super_class,
                                          field("term") = ont.super_class)
                    super_class = registry.ontology _
                        .where(field("ontology_id") = ontology_id,
                               field("term_id") = ont.super_class) _
                        .find(Of ontology)
                    registry.ontology_relation.add(field("term_id") = super_class.id,
                                                   field("is_a") = kingdom.id)
                End If

                If super_class IsNot Nothing Then node = super_class

                If super_class IsNot Nothing AndAlso Not ont.class.StringEmpty Then
                    [class] = registry.ontology _
                        .where(field("ontology_id") = ontology_id,
                               field("term_id") = ont.class) _
                        .find(Of ontology)

                    If [class] Is Nothing Then
                        registry.ontology.add(field("ontology_id") = ontology_id,
                                              field("term_id") = ont.class,
                                              field("term") = ont.class)
                        [class] = registry.ontology _
                            .where(field("ontology_id") = ontology_id,
                                   field("term_id") = ont.class) _
                            .find(Of ontology)
                        registry.ontology_relation.add(field("term_id") = [class].id,
                                                       field("is_a") = super_class.id)
                    End If

                    If [class] IsNot Nothing Then node = [class]

                    If [class] IsNot Nothing AndAlso Not ont.sub_class.StringEmpty Then
                        sub_class = registry.ontology _
                            .where(field("ontology_id") = ontology_id,
                                   field("term_id") = ont.sub_class) _
                            .find(Of ontology)

                        If sub_class Is Nothing Then
                            registry.ontology.add(field("ontology_id") = ontology_id,
                                                  field("term_id") = ont.sub_class,
                                                  field("term") = ont.sub_class)
                            sub_class = registry.ontology _
                                .where(field("ontology_id") = ontology_id,
                                       field("term_id") = ont.sub_class) _
                                .find(Of ontology)
                            registry.ontology_relation.add(field("term_id") = sub_class.id,
                                                           field("is_a") = [class].id)
                        End If

                        node = sub_class
                    End If
                End If
            End If
        End If

        If node IsNot Nothing Then
            Call registry.metabolite_class _
                .ignore _
                .add(field("metabolite_id") = m.id,
                     field("class_id") = node.id,
                     field("note") = source_id)
        End If
    End Sub

    <Extension>
    Public Sub SaveSynonyms(registry As biocad_registry,
                            m As metabolites,
                            synonyms As IEnumerable(Of String),
                            db_source As UInteger)
        Dim metabolite_type As UInteger = New biocad_vocabulary(registry).metabolite_type
        Dim trans As CommitTransaction = registry.synonym.open_transaction.ignore

        For Each synonym As String In synonyms.SafeQuery
            If synonym Is Nothing Then
                Continue For
            End If

            Call trans _
                .ignore _
                .add(
                    field("obj_id") = m.id,
                    field("type") = metabolite_type,
                    field("db_source") = db_source,
                    field("synonym") = synonym,
                    field("hashcode") = Strings.LCase(synonym).MD5,
                    field("lang") = "en"
            )
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Private Function PickName(m As metabolites) As String
        If Not m.biocyc.StringEmpty Then
            Return m.biocyc
        ElseIf Not m.mesh_id.StringEmpty Then
            Return m.mesh_id
        ElseIf Not m.wikipedia.StringEmpty Then
            Return m.wikipedia
        ElseIf Not m.kegg_id.StringEmpty Then
            Return m.kegg_id
        ElseIf Not m.hmdb_id.StringEmpty Then
            Return m.hmdb_id
        ElseIf Not m.lipidmaps_id Then
            Return m.lipidmaps_id
        ElseIf Not m.formula.StringEmpty Then
            Return m.formula
        ElseIf Not m.cas_id.StringEmpty Then
            Return m.cas_id
        Else
            Return $"Metabolite_{m.id}"
        End If
    End Function

    Private Sub UpdateHashCode(registry As biocad_registry, Optional page_size As Integer = 5000)
        Dim updates As CommitTransaction = registry.metabolites.open_transaction

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data = registry.metabolites.limit(offset, page_size).select(Of metabolites)()

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"update hashcode for metabolite data page {page}...".debug
            End If

            For Each name As metabolites In page_data
                If name.name.StringEmpty Then
                    name.name = name.PickName
                    Call registry.metabolites _
                        .where(field("id") = name.id) _
                        .save(field("name") = name.name)
                End If

                Dim hashcode As String = Strings.LCase(name.name).MD5

                If hashcode <> name.hashcode Then
                    Call updates.add(registry.metabolites _
                        .where(field("id") = name.id) _
                        .save_sql(field("hashcode") = hashcode))
                End If
            Next
        Next

        Call "commit the name hashcode".debug
        Call updates.commit()
    End Sub

    <Extension>
    Public Sub MetaboliteLinks(registry As biocad_registry)
        Dim trans As CommitTransaction = registry.metabolites.open_transaction

        Call UpdateHashCode(registry)

        For Each hashcode As String In TqdmWrapper.Wrap(registry.metabolites.group_by("hashcode").having(field("*").count > 1).project(Of String)("hashcode"))
            Dim metas = registry.metabolites.where(field("hashcode") = hashcode).select(Of metabolites)
            Dim top_main As metabolites = metas.OrderByDescending(Function(a) a.MetaboliteScore).First
            Dim updates As New List(Of FieldAssert)

            For Each meta As metabolites In metas
                If std.Abs(meta.exact_mass - top_main.exact_mass) > 0.1 Then
                    Continue For
                End If

                If meta.id = top_main.id Then
                    Call registry.metabolites.where(field("id") = top_main.id).save(field("main_id") = 0)
                Else
                    Call trans.add(registry.metabolites.where(field("id") = meta.id).save_sql(field("main_id") = top_main.id))

                    If top_main.cas_id.StringEmpty AndAlso Not meta.cas_id.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.cas_id)) = meta.cas_id)
                    End If
                    If top_main.kegg_id.StringEmpty AndAlso Not meta.kegg_id.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.kegg_id)) = meta.kegg_id)
                    End If
                    If top_main.hmdb_id.StringEmpty AndAlso Not meta.hmdb_id.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.hmdb_id)) = meta.hmdb_id)
                    End If
                    If top_main.lipidmaps_id.StringEmpty AndAlso Not meta.lipidmaps_id.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.lipidmaps_id)) = meta.lipidmaps_id)
                    End If
                    If top_main.drugbank_id.StringEmpty AndAlso Not meta.drugbank_id.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.drugbank_id)) = meta.drugbank_id)
                    End If
                    If top_main.mesh_id.StringEmpty AndAlso Not meta.mesh_id.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.mesh_id)) = meta.mesh_id)
                    End If
                    If top_main.biocyc.StringEmpty AndAlso Not meta.biocyc.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.biocyc)) = meta.biocyc)
                    End If
                    If top_main.wikipedia.StringEmpty AndAlso Not meta.wikipedia.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.wikipedia)) = meta.wikipedia)
                    End If
                    If top_main.chebi_id = 0 AndAlso meta.chebi_id > 0 Then
                        Call updates.Add(field(NameOf(metabolites.chebi_id)) = meta.chebi_id)
                    End If
                    If top_main.pubchem_cid = 0 AndAlso meta.pubchem_cid > 0 Then
                        Call updates.Add(field(NameOf(metabolites.pubchem_cid)) = meta.pubchem_cid)
                    End If
                    If top_main.note.StringEmpty AndAlso Not meta.note.StringEmpty Then
                        Call updates.Add(field(NameOf(metabolites.note)) = meta.note)
                    End If

                    If updates.Any Then
                        Call trans.add(registry.metabolites.where(field("id") = meta.id).save_sql(updates.ToArray))
                    End If
                End If
            Next
        Next

        Call trans.commit()
    End Sub
End Module
