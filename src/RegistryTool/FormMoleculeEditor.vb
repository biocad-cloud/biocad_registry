Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemoinformatics
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports BioNovoGene.BioDeep.Chemoinformatics.SMILES
Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.text.markdown
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.Web.WebView2.Core
Imports Ollama
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.Configs
Imports RegistryTool.My
Imports SMRUCC.genomics.Model.MotifGraph
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure.Kmer

Public Class FormMoleculeEditor

    Public id As String

    Public Const viewer As String = "<!DOCTYPE html SYSTEM ""about:legacy-compat"">
<html xmlns=""http://www.w3.org/1999/xhtml"" lang=""en"" xml:lang=""en"">

<head>    
    <script type=""text/javascript"" src=""http://biocad.innovation.ac.cn/resources/vendor/smiles-drawer.min.js""></script>
    <script id=""struct-data"" type=""plain/text"">{$struct_data}</script>
</head>

<body>
<canvas id=""smiles-canvas"" width=""450px"" height=""300px"" role=""img"" focusable=""false""
                            style=""padding: 5px;""></canvas>
</body>

<script type=""text/javascript"">
let options = { width: 450, height: 300 };
    // Initialize the drawer to draw to canvas
    let smilesDrawer = new SmilesDrawer.Drawer(options);
    // Alternatively, initialize the SVG drawer:
    // let svgDrawer = new SmilesDrawer.SvgDrawer(options);

    function rendering(input_value) {
        // Clean the input (remove unrecognized characters, such as spaces and tabs) and parse it
        SmilesDrawer.parse(input_value, function (tree) {
            // Draw to the canvas
            smilesDrawer.draw(tree, ""smiles-canvas"", ""light"", false);
            // Alternatively, draw to SVG:
            // svgDrawer.draw(tree, 'output-svg', 'dark', false);
        });
    }

    rendering(document.getElementById(""struct-data"").innerText);
</script>
</html>"

    Dim struct As biocad_registryModel.sequence_graph
    Dim mol As biocad_registryModel.molecule
    Dim morgan As ProteinStructure.MorganFingerprint

    Private Async Sub FormMoleculeEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mol = MyApplication.biocad_registry.molecule _
            .where(field("id") = UInteger.Parse(id.Match("\d+"))) _
            .find(Of biocad_registryModel.molecule)

        SMILES.MolecularFingerprint.Length = 1024
        morgan = New ProteinStructure.MorganFingerprint(SMILES.MolecularFingerprint.Length)

        If mol Is Nothing Then
            MessageBox.Show($"There is no molecule object that associated with the given unique id: {id}", "Missing Object", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.Close()
            Return
        Else
            MyApplication.settings.molecule_history = MyApplication.settings.molecule_history _
                .JoinIterates(New MoleculeEditHistory With {.id = mol.id, .name = mol.name}) _
                .Take(50) _
                .ToArray
            MyApplication.settings.Save()
        End If

        TextBox2.Text = mol.name
        TextBox3.Text = mol.formula
        Label7.Text = FormulaScanner.EvaluateExactMass(mol.formula).ToString("F4")
        TextBox4.Text = mol.note
        LinkLabel1.Text = $"http://biocad.innovation.ac.cn/molecule/BioCAD{mol.id.ToString.PadLeft(11, "0"c)}/"

        struct = MyApplication.biocad_registry.sequence_graph _
            .where(field("molecule_id") = mol.id) _
            .find(Of biocad_registryModel.sequence_graph)

        If Not struct Is Nothing Then
            TextBox1.Text = struct.sequence
            TextBox5.Text = struct.morgan
        End If

        For Each term As TopicTerm In TopicTerm.GetTopics
            Call ComboBox2.Items.Add(term)
        Next

        For Each term As TopicTerm In TopicTerm.GetDatabaseTerm
            Call ComboBox3.Items.Add(term)
        Next

        Dim taxonomy As OrganismSource() = MyApplication.biocad_registry.taxonomy_source _
            .left_join("ncbi_taxonomy") _
            .on(field("`ncbi_taxonomy`.id") = field("ncbi_taxid")) _
            .where(field("molecule_id") = mol.id) _
            .distinct _
            .select(Of OrganismSource)("ncbi_taxid", "taxname")

        Call ListBox3.Items.Clear()

        For Each link As OrganismSource In taxonomy.OrderBy(Function(a) a.taxname)
            If Not link.taxname.StringEmpty(, True) Then
                Call ListBox3.Items.Add(link)
            End If
        Next

        Call refreshNames()
        Call refreshXrefs()
        Call refreshTags()

        Await LoadReactions()
        Await WebViewLoader.Init(WebView21)
    End Sub

    Private Async Function LoadReactions() As Task
        Dim reaction_ids As UInteger() = Await Task.Run(Function() MyApplication.biocad_registry.reaction_graph.where(field("molecule_id") = mol.id).distinct.project(Of UInteger)("reaction"))

        If reaction_ids.IsNullOrEmpty Then
            Return
        End If

        Dim reactions As biocad_registryModel.reaction() = Await Task.Run(Function() MyApplication.biocad_registry.reaction.where(field("id").in(reaction_ids)).select(Of biocad_registryModel.reaction)())

        For Each rxn In reactions
            Dim offset = DataGridView2.Rows.Add(rxn.name, rxn.equation, rxn.note)
            DataGridView2.Rows(offset).HeaderCell.Value = rxn.id
        Next

        Call DataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Function

    Private Sub refreshNames(Optional lang As String = Nothing)
        Dim q As FieldAssert() = {
            field("obj_id") = mol.id,
            field("type_id") = mol.type
        }

        If Not lang.StringEmpty(, True) Then
            q = q _
                .JoinIterates(field("lang") = lang) _
                .ToArray
        End If

        Dim names = MyApplication.biocad_registry.synonym _
            .where(q) _
            .order_by("synonym") _
            .select(Of biocad_registryModel.synonym) _
            .OrderBy(Function(a) a.synonym) _
            .ToArray

        Call ListBox1.Items.Clear()

        For Each name As biocad_registryModel.synonym In names
            Call ListBox1.Items.Add(name.synonym)
        Next
    End Sub

    Private Sub refreshXrefs()
        Dim xrefs = MyApplication.biocad_registry.db_xrefs _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("db_key")) _
            .where(field("obj_id") = mol.id) _
            .order_by("dbname") _
            .select(Of XrefID)("db_xrefs.id as xref_id", "term as dbname", "xref", "db_key")

        DataGridView1.Rows.Clear()

        For Each id As XrefID In xrefs
            Dim offset = DataGridView1.Rows.Add(id.dbname, id.xref)
            DataGridView1.Rows(offset).Tag = id
        Next

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub refreshTags()
        Call ListBox2.Items.Clear()

        For Each tag As Tag In Tag.GetTags(mol.id)
            Call ListBox2.Items.Add(tag)
        Next
    End Sub

    Private Sub SaveCommonName() Handles Button2.Click
        Dim name As String = Strings.Trim(TextBox2.Text)
        MyApplication.biocad_registry.molecule.where(field("id") = UInteger.Parse(id.Match("\d+"))).save(field("name") = name)
    End Sub

    Private Sub WebView21_CoreWebView2InitializationCompleted(sender As Object, e As CoreWebView2InitializationCompletedEventArgs) Handles WebView21.CoreWebView2InitializationCompleted
        If Not struct Is Nothing Then
            Call WebView21.NavigateToString(viewer.Replace("{$struct_data}", struct.sequence))
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim smilesOrSeq As String = Strings.Trim(TextBox1.Text)
        Dim checksum As Byte()

        If mol.type = MyApplication.biocad_registry.vocabulary_terms.metabolite_term Then
            checksum = MolecularFingerprint.ConvertToMorganFingerprint(smilesOrSeq, radius:=3)
        Else
            Dim graph = KMerGraph.FromSequence(smilesOrSeq, k:=3)
            checksum = morgan.CalculateFingerprintCheckSum(graph, radius:=3)
        End If

        Dim fingerprint = checksum.GZipAsBase64

        TextBox5.Text = fingerprint

        If struct Is Nothing Then
            MyApplication.biocad_registry.sequence_graph.add(
                field("sequence") = smilesOrSeq,
                     field("morgan") = fingerprint,
                     field("hashcode") = smilesOrSeq.MD5,
                     field("molecule_id") = UInteger.Parse(id.Match("\d+"))
            )

            struct = MyApplication.biocad_registry.sequence_graph _
               .where(field("molecule_id") = UInteger.Parse(id.Match("\d+"))) _
               .order_by("id", desc:=True) _
               .find(Of biocad_registryModel.sequence_graph)

            WebView21_CoreWebView2InitializationCompleted(Nothing, Nothing)
        Else
            MyApplication.biocad_registry.sequence_graph _
               .where(field("id") = struct.id) _
               .save(field("sequence") = smilesOrSeq,
                     field("morgan") = fingerprint,
                     field("hashcode") = smilesOrSeq.MD5)
        End If
    End Sub

    Private Sub SetAsDisplayNameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetAsDisplayNameToolStripMenuItem.Click
        If ListBox1.SelectedIndex < 0 Then
            Return
        End If

        Dim lang As String = CStr(ComboBox1.SelectedItem)

        If lang <> "en" Then
            MessageBox.Show("Only allows synonym names in english language to be set as display name.", "Invalid Language", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        Dim name = ListBox1.Items(ListBox1.SelectedIndex).ToString
        TextBox2.Text = name
        Call SaveCommonName()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)
        Dim db As String = CStr(row.Cells(0).Value)
        Dim xref As String = CStr(row.Cells(1).Value)
        Dim url As String

        Select Case LCase(db)
            Case "pubchem" : url = $"https://pubchem.ncbi.nlm.nih.gov/compound/{xref}"
            Case Else
                Call Workbench.StatusMessage($"Unkonw url builder for db xref {db}:{xref}")
                Return
        End Select

        Call Tools.OpenUrlWithDefaultBrowser(url)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim desc = Strings.Trim(TextBox4.Text)

        MyApplication.biocad_registry.molecule.where(field("id") = UInteger.Parse(id.Match("\d+"))).save(field("note") = desc)
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim int = UInteger.Parse(id.Match("\d+"))
        Dim url = $"http://biocad.innovation.ac.cn/molecule/BioCAD{int.ToString.PadLeft(11, "0"c)}/"

        Call Tools.OpenUrlWithDefaultBrowser(url)
    End Sub

    Private Sub ContextMenuStrip2_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip2.Opening

    End Sub

    Private Sub ClearThisTagToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearThisTagToolStripMenuItem.Click
        Dim sel As Tag = ListBox2.SelectedItem

        If sel Is Nothing Then
            Return
        End If

        Call MyApplication.biocad_registry.molecule_tags.where(field("tag_id") = sel.tag_id, field("molecule_id") = sel.molecule_id).delete()
        Call refreshTags()
    End Sub

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        Dim sel = DataGridView1.SelectedRows

        If sel.Count = 0 Then
            Return
        End If

        Dim db As XrefID = sel(0).Tag
        Dim all_xrefs = MyApplication.biocad_registry.db_xrefs _
            .where(field("db_key") = db.db_key,
                   field("obj_id") = mol.id) _
            .select(Of biocad_registryModel.db_xrefs)
        Dim edit As New FormTextEditor

        edit.SetText(all_xrefs.Select(Function(a) a.xref))
        edit.SetPromptText("Edit the database xref id(delete text for removes id from database, add text for add new id into database)")
        edit.ShowDialog()

        Dim current = all_xrefs.GroupBy(Function(a) a.xref).ToDictionary(Function(a) a.Key, Function(a) a.ToArray)

        Call TaskProgress.RunAction(
            Sub(println As ITaskProgress)
                Call println.SetInfo("Commit data changes into the database...")

                For Each id As String In edit.TextLines
                    If current.ContainsKey(id) Then
                        ' no changed
                    Else
                        ' add new
                        Call MyApplication.biocad_registry.db_xrefs.add(
                            field("db_key") = db.db_key,
                            field("obj_id") = mol.id,
                            field("xref") = id,
                            field("type") = mol.type
                        )
                    End If
                Next

                Dim modified As Index(Of String) = edit.TextLines.Indexing

                For Each key As String In current.Keys
                    If key Like modified Then
                        ' no changed
                    Else
                        ' deleted
                        Call MyApplication.biocad_registry.db_xrefs.where(field("db_key") = db.db_key,
                            field("obj_id") = mol.id,
                            field("xref") = key,
                            field("type") = mol.type).delete()
                    End If
                Next
            End Sub)

        Call refreshXrefs()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If ComboBox2.SelectedIndex < 0 Then
            Return
        End If

        Dim term As TopicTerm = ComboBox2.SelectedItem

        If MyApplication.biocad_registry.molecule_tags _
            .where(field("tag_id") = term.id,
                   field("molecule_id") = mol.id) _
            .find(Of biocad_registryModel.molecule_tags) Is Nothing Then

            Call MyApplication.biocad_registry.molecule_tags.add(
                field("tag_id") = term.id,
                field("molecule_id") = mol.id,
                field("description") = "Molecule Editor"
            )
            Call refreshTags()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex <= 0 Then
            ' show all
            Call refreshNames()
        Else
            Call refreshNames(lang:=CStr(ComboBox1.SelectedItem))
        End If
    End Sub

    Private Sub EditToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem1.Click
        If ComboBox1.SelectedIndex <= 0 Then
            Call MessageBox.Show("A language for the synonym name must be selected!",
                                 "Invalid Language",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning)
            Return
        End If

        Dim lang As String = CStr(ComboBox1.SelectedItem)
        Dim edit As New FormTextEditor
        Dim names = MyApplication.biocad_registry.synonym.where(field("lang") = lang, field("obj_id") = mol.id).project(Of String)("synonym")

        Call edit.SetText(names)
        Call edit.SetPromptText($"edit the synonym names({lang}):")
        Call edit.ShowDialog()

        Dim editData As String() = edit.TextLines

        Call TaskProgress.RunAction(
            Sub(println As ITaskProgress)
                Dim current = names.Indexing

                Call println.SetInfo("Commit modified data to database...")

                For Each name As String In edit.TextLines
                    If name Like current Then
                        ' no changed
                    Else
                        ' add new
                        Call MyApplication.biocad_registry.synonym.add(
                            field("type_id") = mol.type,
                            field("obj_id") = mol.id,
                            field("synonym") = name,
                            field("lang") = lang,
                            field("hashcode") = name.ToLower.MD5
                        )
                    End If
                Next

                Dim modified As Index(Of String) = edit.TextLines.Indexing

                For Each key As String In current.Objects
                    If key Like modified Then
                        ' no changed
                    Else
                        ' deleted
                        Call MyApplication.biocad_registry.synonym _
                            .where(field("obj_id") = mol.id,
                                   field("lang") = lang,
                                   field("type_id") = mol.type,
                                   field("synonym") = key).delete()
                    End If
                Next
            End Sub)

        Call refreshNames(lang)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim prompt As String = $"please talk me about the biological function of the compound: '{TextBox2.Text}' in a short conclusion abstract text"
        Dim msg As DeepSeekResponse = TaskProgress.LoadData(Function(println As Action(Of String)) MyApplication.ollama.Chat(prompt))
        Dim markdown As New MarkdownRender

        If Not msg Is Nothing Then
            Try
                TextBox4.Text = markdown.Transform(msg.output)
            Catch ex As Exception
                TextBox4.Text = msg.output
            End Try
        End If
    End Sub

    Private Sub ChineseNameTranslationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChineseNameTranslationToolStripMenuItem.Click
        If ListBox1.SelectedIndex < 0 Then
            Return
        End If

        Dim lang As String = CStr(ComboBox1.SelectedItem)

        If lang <> "en" Then
            MessageBox.Show("Only allows synonym names in english language to be translated into chinese name.", "Invalid Language", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        Dim name = ListBox1.Items(ListBox1.SelectedIndex).ToString
        Dim prompt As String = $"将下面的这个化合物名称翻译为中文：'{name}'，如果没有正式的翻译，请进行音译。使用下面的json格式返回结果给我以方便我进行数据解析：{{""zh_name"": ""translated_name""}}"
        Dim msg As DeepSeekResponse = TaskProgress.LoadData(Function(println As Action(Of String)) MyApplication.ollama.Chat(prompt))

        If Not msg Is Nothing Then
            Try
                Dim json As String = msg.output.Match("[{].+[}]")
                Dim zh_name = json.LoadJSON(Of TranslatedName)

                If MyApplication.biocad_registry.synonym.where(field("type_id") = mol.type,
                    field("obj_id") = mol.id,
                    field("synonym") = zh_name.ToString,
                    field("lang") = "zh").find(Of biocad_registryModel.synonym) Is Nothing Then

                    Call MyApplication.biocad_registry.synonym.add(
                        field("type_id") = mol.type,
                        field("obj_id") = mol.id,
                        field("synonym") = zh_name.ToString,
                        field("lang") = "zh",
                        field("hashcode") = zh_name.ToString.ToLower.MD5
                    )
                End If
            Catch ex As Exception
                Call Workbench.StatusMessage(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If ComboBox3.SelectedIndex < 0 Then
            Return
        End If
        Dim new_id As String = Strings.Trim(TextBox6.Text)
        Dim dbname As TopicTerm = ComboBox3.SelectedItem

        If new_id.StringEmpty(, True) Then
            Return
        End If

        If MyApplication.biocad_registry.db_xrefs.where(field("db_key") = dbname.id,
            field("obj_id") = mol.id,
            field("xref") = new_id,
            field("type") = mol.type).find(Of biocad_registryModel.db_xrefs) Is Nothing Then

            Call MyApplication.biocad_registry.db_xrefs.add(
                field("db_key") = dbname.id,
                field("obj_id") = mol.id,
                field("xref") = new_id,
                field("type") = mol.type
            )
        End If

        Call refreshXrefs()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedIndex < 0 Then
            Return
        End If

        Dim dbname As TopicTerm = ComboBox3.SelectedItem
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedIndex > -1 Then
            Workbench.StatusMessage(ListBox1.SelectedItem.ToString)
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        If ListBox1.SelectedIndex > -1 Then
            Call Clipboard.SetText(ListBox1.SelectedItem.ToString)
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        Dim formula As String = Strings.Trim(TextBox3.Text)
        Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(formula)

        Label7.Text = exact_mass.ToString("F4")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim formula As String = Strings.Trim(TextBox3.Text)
        Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(formula)

        Call MyApplication.biocad_registry.molecule.where(field("id") = mol.id).save(
            field("formula") = formula,
            field("mass") = exact_mass
        )
    End Sub

    Private Sub ListSourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListSourceToolStripMenuItem.Click
        If ListBox3.SelectedIndex < 0 Then
            Return
        End If

        Dim source As OrganismSource = DirectCast(ListBox3.SelectedItem, OrganismSource)
        Dim molecules As MoleculeSearch() = TaskProgress _
            .LoadData(Function(println As Action(Of String))
                          Return MyApplication.biocad_registry.taxonomy_source _
                            .left_join("molecule").on(field("`molecule`.id") = field("molecule_id")) _
                            .left_join("vocabulary").on(field("`vocabulary`.id") = field("`molecule`.type")) _
                            .where(field("ncbi_taxid") = source.ncbi_taxid) _
                            .distinct _
                            .select(Of MoleculeSearch)("`molecule`.id",
    "`molecule`.name",
    "formula",
    "mass",
    "term AS type",
    "`molecule`.note")
                      End Function)
        Dim view As New FormDbView()
        view.LoadTableView(Function() molecules)
        view.SetViewer(Sub(row)
                           Dim id As String = row.Cells(0).Value.ToString
                           Dim name As String = row.Cells(1).Value.ToString

                           Call Workbench.OpenMoleculeEditor(id, name)
                       End Sub)
        view.Text = $"Metabolite From Taxonomy '{source}'"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub SearchByThisDbXrefToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchByThisDbXrefToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim link As XrefID = DataGridView1.SelectedRows(0).Tag

        If link Is Nothing Then
            Return
        End If

        Dim molecules As MoleculeSearch() = TaskProgress _
            .LoadData(Function(println As Action(Of String))
                          Return MyApplication.biocad_registry.db_xrefs _
                            .left_join("molecule").on(field("`molecule`.id") = field("obj_id")) _
                            .left_join("vocabulary").on(field("`vocabulary`.id") = field("`molecule`.type")) _
                            .where(field("db_key") = link.db_key, field("xref") = link.xref) _
                            .distinct _
                            .select(Of MoleculeSearch)("`molecule`.id",
    "`molecule`.name",
    "formula",
    "mass",
    "term AS type",
    "`molecule`.note")
                      End Function)
        Dim view As New FormDbView()
        view.LoadTableView(Function() molecules)
        view.SetViewer(Sub(row)
                           Dim id As String = row.Cells(0).Value.ToString
                           Dim name As String = row.Cells(1).Value.ToString

                           Call Workbench.OpenMoleculeEditor(id, name)
                       End Sub)
        view.Text = $"Metabolite with Xref '{link}'"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub OpenMoleculeDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenMoleculeDataToolStripMenuItem.Click
        If DataGridView3.SelectedRows.Count = 0 Then
            Return
        End If

        Dim meta = DataGridView3.SelectedRows(0)
        Dim registry_id As String = CStr(meta.Cells(0).Value)
        Dim name As String = CStr(meta.Cells(1).Value)

        If registry_id <> "" Then
            Call Workbench.OpenMoleculeEditor(registry_id, name)
        End If
    End Sub

    Private Async Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        If DataGridView2.SelectedRows.Count = 0 Then
            Return
        End If

        Dim rxn = DataGridView2.SelectedRows(0)
        Dim rxn_id As String = CStr(rxn.HeaderCell.Value)
        Dim graph = Await Task.Run(
            Function()
                Return MyApplication.biocad_registry.reaction_graph _
                    .left_join("molecule") _
                    .on(field("molecule_id") = field("molecule.id")) _
                    .left_join("vocabulary") _
                    .on(field("vocabulary.id") = field("role")) _
                    .where(field("reaction") = UInteger.Parse(rxn_id)) _
                    .select(Of reaction_graphdata)("molecule.*", "db_xref", "term AS role")
            End Function)

        DataGridView3.Rows.Clear()

        For Each compound In graph
            Call DataGridView3.Rows.Add(
                compound.id,
                compound.db_xref,
                compound.name,
                compound.formula,
                compound.mass,
                compound.role
            )
        Next
    End Sub
End Class

Public Class reaction_graphdata

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property mass As Double
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property db_xref As String
    <DatabaseField> Public Property role As String

End Class