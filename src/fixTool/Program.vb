Imports System
Imports biocad_registry
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
    ReadOnly registry As New biocad_registry(mysql)

    Sub Main(args As String())
        Console.WriteLine("Hello World!")
    End Sub

    Sub removesDuplicatedMolecules()
        Do While True
            Dim xref_ids As String() = registry.molecule.group_by("xref_id").having(field("xref_id").count > 1).limit(1000).project(Of String)("xref_id")
        Loop
    End Sub
End Module
