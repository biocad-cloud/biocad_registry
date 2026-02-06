Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports registry_data

Module Program

    Friend ReadOnly mysql As New ConnectionUri() With {
        .Database = "cad_registry",
        .error_log = "Z:/aaaa.log",
        .IPAddress = "192.168.3.15",
        .Password = "123456",
        .Port = 3306,
        .User = "xieguigang"
    }
    Friend ReadOnly registry As New biocad_registry(mysql)

    Sub Main(args As String())
        Call removesInvalidNameChars()
    End Sub

    Sub removesInvalidNameChars()
        For i As Integer = 0 To 100000
            Dim q = registry.metabolites _
                .where((field("name").instr("""") = 1) Or (field("name").instr("'") = 1)) _
                .limit(100) _
                .select(Of biocad_registryModel.metabolites)

            If q.IsNullOrEmpty Then
                Exit For
            End If

            Dim update_names = registry.metabolites.ignore.open_transaction

            For Each item In TqdmWrapper.Wrap(q)
                Dim clean_name = item.name.Trim(""""c, "'"c, " "c)

                Call update_names.add(registry.metabolites _
                    .where(field("id") = item.id) _
                    .save_sql(field("name") = clean_name,
                              field("hashcode") = clean_name.ToLower.MD5))
            Next

            Call update_names.commit()

            Call Console.WriteLine("------------")
        Next
    End Sub


End Module
