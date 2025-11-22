Imports biocad_storage
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module idSplit

    Public Sub splitJointID()
        Dim page_size As Integer = 1000
        Dim db = 282

        Do While True
            Dim list = registry.db_xrefs _
                .where(field("db_key") = db, field("xref").instr(",")) _
                .limit(page_size) _
                .select(Of biocad_registryModel.db_xrefs)

            If list.IsNullOrEmpty Then
                Exit Do
            End If

            Dim id_trans = registry.db_xrefs.open_transaction.ignore

            For Each invalid In TqdmWrapper.Wrap(list)
                Dim newIds = invalid.xref.StringSplit("\s*,\s*") ' \s*,\s*

                For Each newId In newIds.Select(AddressOf Strings.Trim)
                    Dim exists = registry.db_xrefs.where(field("obj_id") = invalid.obj_id,
                                                         field("db_key") = db,
                                                         field("xref") = newId).find(Of biocad_registryModel.db_xrefs)
                    If exists Is Nothing Then
                        id_trans.add(
                            field("obj_id") = invalid.obj_id,
                            field("db_key") = db,
                            field("xref") = newId,
                            field("type") = invalid.type
                        )
                    End If
                Next

                Call id_trans.add(registry.db_xrefs.where(field("id") = invalid.id).delete_sql)
            Next

            Call id_trans.commit()
        Loop
    End Sub
End Module
