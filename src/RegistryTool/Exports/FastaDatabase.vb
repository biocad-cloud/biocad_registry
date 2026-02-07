Imports System.IO
Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.Linq
Imports registry_data
Imports RegistryTool.My
Imports SMRUCC.genomics
Imports SMRUCC.genomics.SequenceModel.FASTA

Module FastaDatabase

    Private Function BuildMySQL() As String
        Return "SELECT 
    t1.id, compartment_name, name, sequence
FROM
    (SELECT 
        compartment_name, molecule.id, name
    FROM
        cad_registry.subcellular_location
    LEFT JOIN molecule ON molecule.id = obj_id
    LEFT JOIN subcellular_compartments ON subcellular_compartments.id = compartment_id
    WHERE
        compartment_id IN (99 , 102, 106)
            AND obj_id IN (SELECT 
                obj_id
            FROM
                cad_registry.db_xrefs
            WHERE
                type = 212 AND db_key = 354)) t1
        LEFT JOIN
    sequence_graph ON t1.id = molecule_id
WHERE
    CHAR_LENGTH(sequence) > 0
"
    End Function

    Public Sub ExportMembraneTransporter()
        Dim registry As biocad_registry = MyApplication.biocad_registry
        Dim sql As String = BuildMySQL()
        Dim proteins As MembraneProtein() = Nothing

        Using file As New SaveFileDialog With {.Filter = "Sequence Database(*.fasta)|*.fasta"}
            If file.ShowDialog = DialogResult.OK Then
                Call ProgressSpinner.DoLoading(
                    loading:=Sub()
                                 proteins = registry.getDriver.Query(Of MembraneProtein)(sql)
                             End Sub)
                Call WriteFasta(proteins, file.FileName)
            End If
        End Using
    End Sub

    Private Sub WriteFasta(proteins As MembraneProtein(), filename As String)
        Using s As Stream = filename.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
            Using fasta As New SequenceModel.FASTA.StreamWriter(s)
                Call TaskProgress.RunAction(
                    run:=Sub(p As ITaskProgress)
                             For Each protein As MembraneProtein In proteins.SafeQuery
                                 Call fasta.Add(New FastaSeq With {
                                    .Headers = {protein.compartment_name, protein.id, protein.name},
                                    .SequenceData = protein.sequence
                                 })
                             Next
                         End Sub,
                    title:="Save data",
                    info:="Export fasta sequnece database...")
            End Using
        End Using
    End Sub
End Module
