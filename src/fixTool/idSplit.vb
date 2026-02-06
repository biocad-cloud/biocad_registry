Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data

Public Module idSplit

    Public Sub splitJointID(db As UInteger, split_regex$, ParamArray q As FieldAssert())
        Dim page_size As Integer = 1000

        Do While True
            Dim list As biocad_registryModel.db_xrefs() = registry.db_xrefs _
                .where(q) _
                .limit(page_size) _
                .select(Of biocad_registryModel.db_xrefs)

            If list.IsNullOrEmpty Then
                Exit Do
            End If

            Dim id_trans = registry.db_xrefs.open_transaction.ignore

            For Each invalid In TqdmWrapper.Wrap(list)
                Dim newIds = invalid.db_xref.StringSplit(split_regex)

                For Each newId As String In newIds.Select(AddressOf Strings.Trim).Select(Function(idsplit) idsplit.Trim(","c, ";"c))
                    Dim exists = registry.db_xrefs.where(field("obj_id") = invalid.obj_id,
                                                         field("db_name") = db,
                                                         field("db_xref") = newId,
                                                         field("db_source") = invalid.db_source,
                                                         field("type") = invalid.type).find(Of biocad_registryModel.db_xrefs)
                    If exists Is Nothing Then
                        id_trans.ignore.add(
                            field("obj_id") = invalid.obj_id,
                            field("db_name") = db,
                            field("db_xref") = newId,
                            field("db_source") = invalid.db_source,
                            field("type") = invalid.type
                        )
                    End If
                Next

                Call id_trans.add(registry.db_xrefs.where(field("id") = invalid.id).delete_sql)
            Next

            Call id_trans.commit()
        Loop
    End Sub

    Public Sub splitCAS_id()
        Dim db As UInteger = registry.biocad_vocabulary.db_cas

        Call splitJointID(db, "((\s)|[,;])+", field("db_name") = db, field("db_xref").instr(" ") Or field("db_xref").instr(",") Or field("db_xref").instr(";"))
    End Sub
End Module
