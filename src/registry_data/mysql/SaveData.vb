
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
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
                            class_id As UInteger)

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
                Call trans _
                    .ignore _
                    .add(
                        field("obj_id") = obj_id,
                        field("type") = class_id,
                        field("db_source") = db_source,
                        field("synonym") = synonym,
                        field("hashcode") = Strings.LCase(synonym).MD5,
                        field("lang") = "en"
                )
            End If
        Next

        Call trans.commit()
    End Sub
End Module
