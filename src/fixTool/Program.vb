Imports biocad_registry
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Module Program

    ReadOnly mysql As New ConnectionUri() With {
        .Database = "cad_registry",
        .error_log = "Z:/aaaa.log",
        .IPAddress = "192.168.3.15",
        .Password = "123456",
        .Port = 3306,
        .User = "xieguigang"
    }
    ReadOnly registry As New biocad_registry.biocad_registry(mysql)

    Sub Main(args As String())
        ' Console.WriteLine("Hello World!")
        Call removesDuplicatedMolecules()
    End Sub

    Sub removesDuplicatedMolecules()
        Do While True
            Dim xref_ids As String() = registry.molecule _
                .group_by("xref_id") _
                .having(field("xref_id").count > 1) _
                .limit(1000) _
                .project(Of String)("xref_id")

            If xref_ids.IsNullOrEmpty Then
                Exit Do
            Else
                Call Console.WriteLine("Processing page data...")
            End If

            Dim bar As Tqdm.ProgressBar = Nothing

            For Each id As String In TqdmWrapper.Wrap(xref_ids, bar:=bar)
                Dim mols = registry.molecule _
                    .where(field("xref_id") = id) _
                    .select(Of biocad_registryModel.molecule)

                For Each duplicated In mols.Skip(1)
                    Call registry.molecule _
                        .where(field("id") = duplicated.id) _
                        .delete()
                Next

                Call bar.SetLabel(id)
            Next
        Loop

        Pause()
    End Sub
End Module
