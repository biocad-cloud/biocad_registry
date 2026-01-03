Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

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

        For Each synonym As String In synonyms.SafeQuery
            If synonym Is Nothing Then
                Continue For
            End If

            Call registry.synonym _
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
    End Sub
End Module
