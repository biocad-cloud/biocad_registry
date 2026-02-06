Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports registry_data
Imports SMRUCC.genomics.Data.Regtransbase.WebServices.Regulogs

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
        ' Call removesInvalidNameChars()
        ' Call fixMoNANames()
        Call idSplit.splitCAS_id()
    End Sub

    Sub fixMoNANames()
        Dim sel = "SELECT 
    *
FROM
    metabolites
WHERE
    id IN (SELECT DISTINCT
            obj_id
        FROM
            cad_registry.db_xrefs
        WHERE
            db_name = 742)
        AND (INSTR(name, 'NCGC') = 1 OR INSTR(name, 'MLS') = 1)
ORDER BY id DESC"

        Dim q = registry.metabolites.getDriver.Query(Of biocad_registryModel.metabolites)(sel)
        Dim update_names = registry.metabolites.ignore.open_transaction

        For Each invalid In TqdmWrapper.Wrap(q)
            Dim clean_name = invalid.name.StringReplace("((NCGC)|(MLS))\d+[-]\d+", "").Trim(" "c, "!"c, """"c, "-"c, "_"c)

            If clean_name = "" Then
                clean_name = invalid.name.Trim(" "c, "!"c, """"c, "-"c, "_"c)
            End If

            If clean_name.StartsWith("(((Cl)|[CHONPS])\d*)+_", RegexICMul) Then
                clean_name = clean_name.GetTagValue("_").Value
            End If

            If clean_name <> "" Then
                Call update_names.add(registry.metabolites _
                       .where(field("id") = invalid.id) _
                       .save_sql(field("name") = clean_name,
                                 field("hashcode") = clean_name.ToLower.MD5))
            End If
        Next

        Call update_names.commit()
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
