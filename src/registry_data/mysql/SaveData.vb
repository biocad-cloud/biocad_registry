
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

''' <summary>
''' common method module for save mysql data
''' </summary>
Public Module SaveData

    <Extension>
    Public Sub SaveSynonyms(registry As biocad_registry,
                            obj_id As UInteger,
                            synonyms As IEnumerable(Of String),
                            db_source As UInteger,
                            class_id As UInteger,
                            Optional commit As CommitTransaction = Nothing)

        Dim trans As CommitTransaction = registry.synonym.open_transaction.ignore

        For Each synonym As String In synonyms.SafeQuery
            If synonym.StringEmpty(, True) Then
                Continue For
            End If

            Dim hashcode As String = Strings.LCase(synonym).MD5
            Dim check = registry.synonym.where(
                field("obj_id") = obj_id,
                field("type") = class_id,
                field("db_source") = db_source,
                field("hashcode") = hashcode).find(Of synonym)

            If check Is Nothing Then
                Dim data = {
                    field("obj_id") = obj_id,
                    field("type") = class_id,
                    field("db_source") = db_source,
                    field("synonym") = synonym,
                    field("hashcode") = Strings.LCase(synonym).MD5,
                    field("lang") = "en"
                }

                If commit Is Nothing Then
                    Call trans.ignore.add(data)
                Else
                    Call commit.add(registry.synonym.add_sql(data))
                End If
            End If
        Next

        If commit Is Nothing Then
            Call trans.commit()
        End If
    End Sub

    <Extension>
    Public Sub DecodeEntity(registry As biocad_registry, Optional page_size As Integer = 10000)
        Dim unescape = GreekAlphabets.lower _
            .ReverseMaps _
            .ToDictionary(Function(a) $"&{a.Key};",
                          Function(a)
                              Return a.Value
                          End Function)

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data = registry.metabolites _
                .limit(offset, page_size) _
                .select(Of biocad_registryModel.metabolites)

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            Dim updates As CommitTransaction = registry.metabolites.ignore.open_transaction

            For Each m As biocad_registryModel.metabolites In page_data
                Dim name As New StringBuilder(m.name)

                For Each alphabet In unescape
                    Call name.Replace(alphabet.Key, alphabet.Value)
                Next

                Dim clean_name As String = name.ToString.Trim(""""c).Trim

                If clean_name <> m.name Then
                    Call updates.add(registry.metabolites _
                        .where(field("id") = m.id) _
                        .save(field("name") = clean_name))
                End If
            Next

            Call updates.commit()
        Next
    End Sub
End Module
