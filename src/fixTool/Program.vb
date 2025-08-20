Imports biocad_storage
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Data.Framework
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

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
        Call TagDataExport.ExportHMDBMetabolites(registry).SaveTo("Z:/hmdb.csv")
        ' Call imports_drugdata()
        ' Call export_plantid()
        ' Call fingerprintBuilder.run()
        '  Call imports_CoconutNP()

        ' Call registry.BuildUniqeHashCode
        ' Call importsMetaCyc()
        ' Call imports_all_plantcyc()

        Pause()
    End Sub

    Sub removesInvalidNameChars()
        For i As Integer = 0 To 100000
            Dim q = registry.molecule _
                .where((field("name").instr("""") = 1) Or (field("name").instr("'") = 1)) _
                .limit(100) _
                .select(Of biocad_registryModel.molecule)

            If q.IsNullOrEmpty Then
                Exit For
            End If

            For Each item In q
                registry.molecule _
                    .where(field("id") = item.id) _
                    .save(field("name") = item.name.Trim(""""c, "'"c, " "c))
            Next

            Call Console.WriteLine("------------")
        Next
    End Sub

    Sub removesDuplicatedSequence()
        Do While True
            Dim xref_ids As biocad_registryModel.sequence_graph() = registry.sequence_graph _
                .group_by("molecule_id", "hashcode") _
                .having(field("*").count > 1) _
                .limit(1000) _
                .select(Of biocad_registryModel.sequence_graph)("molecule_id", "hashcode")

            If xref_ids.IsNullOrEmpty Then
                Exit Do
            Else
                Call Console.WriteLine("Processing page data...")
            End If

            Dim bar As Tqdm.ProgressBar = Nothing
            Dim trans As CommitTransaction = registry.sequence_graph.open_transaction

            For Each id As biocad_registryModel.sequence_graph In TqdmWrapper.Wrap(xref_ids, bar:=bar)
                Dim mols = registry.sequence_graph _
                    .where(field("molecule_id") = id.molecule_id, field("hashcode") = id.hashcode) _
                    .select(Of biocad_registryModel.sequence_graph)

                For Each duplicated In mols.Skip(1)
                    Call trans.delete(field("id") = duplicated.id)
                Next

                Call bar.SetLabel(id.hashcode)
            Next

            Call trans.commit()
        Loop

        Call Console.WriteLine("job done!")

        Pause()
    End Sub

    Sub removesDuplicatedMolecules()
        Do While True
            Dim xref_ids As String() = registry.molecule _
                .group_by("xref_id") _
                .having(field("*").count > 1) _
                .limit(10000) _
                .project(Of String)("xref_id")

            If xref_ids.IsNullOrEmpty Then
                Exit Do
            Else
                Call Console.WriteLine("Processing page data...")
            End If

            Dim bar As Tqdm.ProgressBar = Nothing
            Dim trans As CommitTransaction = registry.molecule.open_transaction

            For Each id As String In TqdmWrapper.Wrap(xref_ids, bar:=bar)
                Dim mols = registry.molecule _
                    .where(field("xref_id") = id) _
                    .select(Of biocad_registryModel.molecule)

                For Each duplicated In mols.Skip(1)
                    Call trans.delete(field("id") = duplicated.id)
                Next

                Call bar.SetLabel(id)
            Next

            Call trans.commit()
        Loop

        Call Console.WriteLine("job done!")

        Pause()
    End Sub
End Module
