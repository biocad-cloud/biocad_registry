﻿Imports System.ComponentModel
Imports System.IO
Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports RegistryTool.My
Imports SMRUCC.genomics
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed
Imports Metadata = BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaLib

Public Class FormMain : Implements AppHost

    Private ReadOnly Property AppHost_ClientRectangle As Rectangle Implements AppHost.ClientRectangle
        Get
            Return New Rectangle(Location, Size)
        End Get
    End Property

    Public ReadOnly Property ActiveDocument As Form Implements AppHost.ActiveDocument
        Get
            Return Nothing
        End Get
    End Property

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

    Private Async Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonRuntime.Hook(Me)
        Await IntializeMainWindow()
    End Sub

    Private Async Function IntializeMainWindow() As Task
        If Not MyApplication.Load Then
            Call MessageBox.Show("Application initialization error!", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Dim topics = Await Task.Run(Function() MyApplication.biocad_registry.vocabulary _
            .where(field("category") = "Topic") _
            .project(Of String)("term"))

        ExportBloodTagToolStripMenuItem.DropDownItems.Clear()

        For Each tag As String In topics.OrderBy(Function(s) s.ToLower)
            Dim item As New ToolStripMenuItem(tag)
            item.Tag = tag
            AddHandler item.Click, Sub() ExportTagToolStripMenuItem_Click(tag)
            ExportBloodTagToolStripMenuItem.DropDownItems.Add(item)
        Next

        OpenMoleculeToolStripMenuItem.DropDownItems.Clear()

        For Each entry As MoleculeEditHistory In MyApplication.settings.molecule_history.SafeQuery
            Dim item As New ToolStripMenuItem(entry.ToString)
            item.Tag = entry
            AddHandler item.Click, Sub() Call Workbench.OpenMoleculeEditor(entry.id, entry.name)
            OpenMoleculeToolStripMenuItem.DropDownItems.Add(item)
        Next
    End Function

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
            Dim repo As New RepositoryWriter(filename.Open(System.IO.FileMode.OpenOrCreate, doClear:=True))

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

    Private Sub ExportTagToolStripMenuItem_Click(tag As String)
        Using file As New SaveFileDialog With {
            .Filter = "id file(*.txt)|*.txt|Molecule table(*.csv)|*.csv",
            .FileName = tag & ".txt"
        }
            If file.ShowDialog = DialogResult.OK Then
                If file.FileName.ExtensionSuffix("txt") Then
                    Call MyApplication.biocad_registry.ExportTagList(tag).SaveTo(file.FileName)
                Else
                    Call MyApplication.biocad_registry.ExportTagData(tag).SaveTo(file.FileName)
                End If
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

                            For Each filepath As String In file.FileNames
                                Call println(" -> imports: " & filepath.BaseName)

                                If filepath.ExtensionSuffix("json") Then
                                    Try
                                        Call kb.MakeImports(PubMedTextTable.ParseJSON(filepath), {Topic})
                                    Catch ex As Exception
                                        Call MessageBox.Show(ex, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End Try
                                Else
                                    Call kb.MakePageImports(PubMedTextTable.LoadTable(filepath), {Topic})
                                End If
                            Next

                            Return True
                        End Function)
                End If
            End If
        End Using
    End Sub

    Private Sub ExportAnnotationTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportAnnotationTableToolStripMenuItem.Click
        Using idfile As New OpenFileDialog With {.Filter = "ID List(*.txt)|*.txt"}
            If idfile.ShowDialog = DialogResult.OK Then
                Dim ids As String() = idfile.FileName.ReadAllLines

                Using file As New SaveFileDialog With {.Filter = "Annotation Table(*.csv)|*.csv"}
                    If file.ShowDialog = DialogResult.OK Then
                        Dim exports As New ExportMetabolites(MyApplication.biocad_registry)
                        Dim list As MetaInfo() = TaskProgress.LoadData(Function(println As Action(Of String)) exports.setOntology("Coconut NPclass").ExportByID(ids, wrap_tqdm:=False).ToArray(Of MetaInfo))
                        Dim df As New DataFrame With {
                            .rownames = list.Select(Function(a) a.ID).ToArray
                        }

                        For Each m As MetaInfo In list
                            If m.name.StringEmpty(, True) AndAlso Not m.super_class.StringEmpty(, True) Then
                                m.name = m.super_class & " NP" & m.formula
                            End If
                        Next

                        Call df.add("name", From m As MetaInfo In list Select m.name)
                        Call df.add("formula", From m As MetaInfo In list Select m.formula)
                        Call df.add("exact_mass", From m As MetaInfo In list Select m.exact_mass)
                        Call df.add("cas", From m As MetaInfo In list Select m.xref.CAS.DefaultFirst)
                        Call df.add("kegg", From m As MetaInfo In list Select m.xref.KEGG)
                        Call df.add("hmdb", From m As MetaInfo In list Select m.xref.HMDB)
                        Call df.add("lipidmaps", From m As MetaInfo In list Select m.xref.lipidmaps)
                        Call df.add("mesh", From m As MetaInfo In list Select m.xref.MeSH)
                        Call df.add("wikipedia", From m As MetaInfo In list Select m.xref.Wikipedia)
                        Call df.add("smiles", From m As MetaInfo In list Select m.xref.SMILES)
                        Call df.add("super_class", From m As MetaInfo In list Select m.super_class)
                        Call df.add("class", From m As MetaInfo In list Select m.class)
                        Call df.add("sub_class", From m As MetaInfo In list Select m.sub_class)

                        Call df.WriteCsv(file.FileName, blank:="NULL")
                    End If
                End Using
            End If
        End Using
    End Sub

    Private Sub OpenMoleculeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenMoleculeToolStripMenuItem.Click
        Dim id = InputBox("Enter a molecule id to open data editor:")

        If Not id.StringEmpty(, True) Then
            Call Workbench.OpenMoleculeEditor(id, "")
        End If
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            Call form.Close()
        Next
    End Sub

    Private Sub SearchNamesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchNamesToolStripMenuItem.Click
        Dim edit As New FormTextEditor

        edit.SetPromptText("Input the molecule name at here line by line:")
        edit.ShowDialog()

        Dim names = edit.TextLines
        Dim editor As New FormBatchEditor

        If names.IsNullOrEmpty Then
            Return
        End If

        editor.MdiParent = Me
        editor.Show()
    End Sub

    Private Sub BatchOperationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BatchOperationToolStripMenuItem.Click
        Dim edit As New FormTextEditor

        edit.SetPromptText("Input the molecule registry id at here line by line:")
        edit.ShowDialog()

        Dim idset = edit.TextLines
        Dim editor As New FormBatchEditor

        If idset.IsNullOrEmpty Then
            Return
        End If

        editor.MdiParent = Me
        editor.LoadFromIDSet(idset)
        editor.Show()
    End Sub

    Public Event ResizeForm As AppHost.ResizeFormEventHandler Implements AppHost.ResizeForm
    Public Event CloseWorkbench As AppHost.CloseWorkbenchEventHandler Implements AppHost.CloseWorkbench

    Private Sub SearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchToolStripMenuItem.Click
        Dim getName As String = InputBox("Input the molecule name to make search:")

        If getName.StringEmpty(, True) Then
            Return
        End If

        Dim text As String = getName.Replace("'", " ").Replace("-", " ").Replace("+", " ")
        Dim sql = $"SELECT 
    t1.id, t1.name, formula, mass, term AS type, t1.note
FROM
    ((SELECT 
        *
    FROM
        cad_registry.molecule
    WHERE
        MATCH (name , note) AGAINST ('{text}' IN BOOLEAN MODE)) UNION (SELECT 
        molecule.*
    FROM
        synonym
    LEFT JOIN molecule ON molecule.id = synonym.obj_id
    WHERE
        MATCH (synonym) AGAINST ('{text}' IN BOOLEAN MODE))) t1
        LEFT JOIN
    vocabulary ON vocabulary.id = t1.type"

        Dim molecules As MoleculeSearch() = TaskProgress.LoadData(Function(println As Action(Of String)) MyApplication.biocad_registry.molecule.getDriver.Query(Of MoleculeSearch)(sql, throwExp:=False))

        If molecules.IsNullOrEmpty Then
            Call MessageBox.Show("Sorry, no search result.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim view As New FormDbView()
        view.LoadTableView(Function() molecules)
        view.SetViewer(Sub(row)
                           Dim id As String = row.Cells(0).Value.ToString
                           Dim name As String = row.Cells(1).Value.ToString

                           Call Workbench.OpenMoleculeEditor(id, name)
                       End Sub)
        view.MdiParent = Me
        view.Text = $"Search Result of '{text}'"
        view.Show()
    End Sub

    Public Sub SetWorkbenchVisible(visible As Boolean) Implements AppHost.SetWorkbenchVisible

    End Sub

    Public Sub SetWindowState(stat As FormWindowState) Implements AppHost.SetWindowState
        WindowState = stat
    End Sub

    Public Function GetDesktopLocation() As Point Implements AppHost.GetDesktopLocation
        Return Location
    End Function

    Public Function GetClientSize() As Size Implements AppHost.GetClientSize
        Return Size
    End Function

    Public Iterator Function GetDocuments() As IEnumerable(Of Form) Implements AppHost.GetDocuments

    End Function

    Public Function GetDockPanel() As Control Implements AppHost.GetDockPanel
        Return Nothing
    End Function

    Public Function GetWindowState() As FormWindowState Implements AppHost.GetWindowState
        Return WindowState
    End Function

    Public Sub SetTitle(title As String) Implements AppHost.SetTitle
        Text = title
    End Sub

    Public Sub StatusMessage(msg As String, Optional icon As Image = Nothing) Implements AppHost.StatusMessage
        ToolStripStatusLabel1.Text = msg
        ToolStripStatusLabel1.Image = icon
    End Sub

    Public Sub Warning(msg As String) Implements AppHost.Warning
        ToolStripStatusLabel1.Text = msg
    End Sub

    Public Sub LogText(text As String) Implements AppHost.LogText

    End Sub

    Public Sub ShowProperties(obj As Object) Implements AppHost.ShowProperties

    End Sub

    Private Sub FormMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        RaiseEvent ResizeForm(Location, Size)
    End Sub

    Private Sub FormMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        RaiseEvent CloseWorkbench(e)

        Call CommonRuntime.SaveUISettings()
    End Sub
End Class
