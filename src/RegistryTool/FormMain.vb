Imports System.IO
Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.JSON
Imports RegistryTool.My
Imports SMRUCC.genomics
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed
Imports Metadata = BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaLib

Public Class FormMain

    Private Sub VocabularyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VocabularyToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.SetFilter("Vocabulary Category", NameOf(biocad_registryModel.vocabulary.category))
        view.LoadTableView(Function() MyApplication.biocad_registry.vocabulary.select(Of biocad_registryModel.vocabulary)("*"))
        view.MdiParent = Me
        view.Text = "`biocad_registry`.`vocabulary`"
        view.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        App.Exit(0)
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MyApplication.Load Then

        End If

        Call MyApplication.SetHost(Me)
    End Sub

    Private Sub SubCellularCompartmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SubCellularCompartmentsToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.LoadTableView(Function() MyApplication.biocad_registry.subcellular_compartments.select(Of biocad_registryModel.subcellular_compartments)("*"))
        view.MdiParent = Me
        view.Text = "`biocad_registry`.`subcellular_compartments`"
        view.Show()
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Call New FormSettings().ShowDialog()
    End Sub

    Private Sub MoleculesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoleculesToolStripMenuItem.Click
        Dim view As New FormMoleculeTable
        view.MdiParent = Me
        view.Show()
    End Sub

    Private Sub ExportMetabolitesDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportMetabolitesDatabaseToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "Metabolite Annotation Database(*.dat)|*.dat|Metabolite Table(*.csv)|*.csv"}
            If file.ShowDialog = DialogResult.OK Then
                Call MyApplication.Loading(
                    Function(println)
                        Return ExportLocal(println, file.FileName)
                    End Function)
                Call MessageBox.Show("Export metabolite local annotation repository database success!", "Task Finish", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Shared Function ExportLocal(println As Action(Of String), filename As String) As Boolean
        Dim i As Integer = 0
        Dim block As i32 = 1

        If filename.ExtensionSuffix("csv") Then
            Dim csv As New System.IO.StreamWriter(filename.Open(FileMode.OpenOrCreate, doClear:=True))
            Dim row As New RowObject From {"ID", "name", "formula", "exact_mass", "cas_id", "kegg_id", "hmdb_id", "chebi_id", "pubchem_cid", "lipidmaps_id", "smiles"}
            Dim db_xrefs As xref

            Call csv.WriteLine(row.AsLine)
            Call row.Clear()
            Call println("Export metabolite annotation into table sheet...")

            For Each mol As Metadata In MetaboliteAnnotations.ExportAnnotation
                db_xrefs = mol.xref
                row.AddRange({mol.ID, mol.name, mol.formula, mol.exact_mass, db_xrefs.CAS.FirstOrDefault, db_xrefs.KEGG, db_xrefs.HMDB, db_xrefs.chebi, db_xrefs.pubchem, db_xrefs.lipidmaps, db_xrefs.SMILES})
                csv.WriteLine(row.AsLine)
                row.Clear()
                i += 1

                If i > 2000 Then
                    i = 0
                    println($"commit block data: {++block}")
                    csv.Flush()
                End If
            Next

            Call csv.Flush()
            Call csv.Close()
            Call csv.Dispose()
        Else
            Dim repo As New RepositoryWriter(filename.Open(IO.FileMode.OpenOrCreate, doClear:=True))

            Call println("Export metabolite annotation into local repository...")

            For Each mol As Metadata In MetaboliteAnnotations.ExportAnnotation
                If i > 2000 Then
                    i = 0
                    repo.CommitBlock()
                    println($"commit block data: {++block}")
                Else
                    i += 1
                    repo.Add(mol)
                End If
            Next

            Call repo.Dispose()
        End If

        Return True
    End Function

    Private Sub FlavorOdorsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FlavorOdorsToolStripMenuItem.Click
        Dim viewer As New FormOdors
        viewer.MdiParent = Me
        viewer.Show()
    End Sub

    Private Sub ImportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportsToolStripMenuItem.Click

    End Sub

    Private Sub GenBankToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenBankToolStripMenuItem.Click
        Using file As New OpenFileDialog With {
            .Filter = "NCBI Genbank(*.gbff;*.gb)|*.gbff;*.gb|Genbank Compression Archive File(*.gz)|*.gz"
        }
            If file.ShowDialog = DialogResult.OK Then
                Dim gbff_stream As Stream

                If file.FileName.ExtensionSuffix("gz") Then
                    gbff_stream = file.FileName.ReadBinary.UnGzipStream
                Else
                    gbff_stream = file.FileName.Open(FileMode.Open, doClear:=False, [readOnly]:=True)
                End If

                For Each gb As GBFF.File In GBFF.File.LoadDatabase(gbff_stream)
                    Call New GenBankImports(MyApplication.biocad_registry, gb).ImportsData()
                Next
            End If
        End Using
    End Sub

    Private Sub ExportEnzymeDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportEnzymeDatabaseToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "Enzyme Protein Sequence(*.fasta)|*.fasta"}
            If file.ShowDialog = DialogResult.OK Then
                Dim s As Stream = file.FileName.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
                Dim str As New SequenceModel.FASTA.StreamWriter(s)

                Call MyApplication.Loading(
                    Function(println)
                        Call str.Add(ProtFasta.ExportEnzyme(MyApplication.biocad_registry))
                        Return True
                    End Function)
                Call str.Dispose()
                Call s.Dispose()
                Call MessageBox.Show("Export enzyme database to local annotation repository file success!",
                                     "Task Finish",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub ExportKEGGIDMappingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportKEGGIDMappingToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "id mapping file(*.json)|*.json"}
            If file.ShowDialog = DialogResult.OK Then
                Call MyApplication.Loading(
                    Function(println)
                        Call MyApplication.biocad_registry _
                            .ExportIdMapping("KEGG") _
                            .GetJson _
                            .SaveTo(file.FileName)

                        Return True
                    End Function)
                Call MessageBox.Show("Export KEGG id mapping to local annotation repository file success!",
                                     "Task Finish",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub ExportBloodTagToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportBloodTagToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "id file(*.txt)|*.txt"}
            If file.ShowDialog = DialogResult.OK Then
                Call MyApplication.biocad_registry.ExportTagList("Blood").SaveTo(file.FileName)
            End If
        End Using
    End Sub

    Private Sub PubMedKnowledgeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PubMedKnowledgeToolStripMenuItem.Click
        Using file As New OpenFileDialog With {.Filter = "pubmed json(*.json)|*.json|pubmed table(*.csv)|*.csv", .Multiselect = True}
            If file.ShowDialog = DialogResult.OK Then
                Dim Topic As String = InputBox("Set Topic Term of current set of the pubmed knowledge data:")

                If Not Topic.StringEmpty(, True) Then
                    Call MyApplication.Loading(
                        Function(println)
                            Dim kb As New PubChemArticleImports(MyApplication.biocad_registry)

                            For Each jsonfile As String In file.FileNames
                                Call println(" -> imports: " & jsonfile.BaseName)
                                Call kb.MakeImports(PubChemTextJSON.ParseJSON(jsonfile), Topic)
                            Next

                            Return True
                        End Function)
                End If
            End If
        End Using
    End Sub
End Class
