Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports BioNovoGene.BioDeep.Chemoinformatics.SMILES
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports Microsoft.Web.WebView2.Core
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

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

    Private Sub FormMoleculeEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mol = MyApplication.biocad_registry.molecule _
            .where(field("id") = UInteger.Parse(id.Match("\d+"))) _
            .find(Of biocad_registryModel.molecule)

        If mol Is Nothing Then
            MessageBox.Show($"There is no molecule object that associated with the given unique id: {id}", "Missing Object", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.Close()
            Return
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

        Call refreshNames()
        Call refreshXrefs()
        Call refreshTags()
        Call WebKit.Init(WebView21)
    End Sub

    Private Sub refreshNames()
        Dim names = MyApplication.biocad_registry.synonym _
            .where(field("obj_id") = mol.id,
                   field("type_id") = mol.type) _
            .order_by("synonym") _
            .select(Of biocad_registryModel.synonym)

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
        Dim smiles As String = Strings.Trim(TextBox1.Text)
        Dim checksum = MolecularFingerprint.ConvertToMorganFingerprint(smiles)
        Dim fingerprint = checksum.GZipAsBase64

        TextBox5.Text = fingerprint

        If struct Is Nothing Then
            MyApplication.biocad_registry.sequence_graph.add(
                field("sequence") = smiles,
                     field("morgan") = fingerprint,
                     field("hashcode") = smiles.MD5,
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
               .save(field("sequence") = smiles,
                     field("morgan") = fingerprint,
                     field("hashcode") = smiles.MD5)
        End If
    End Sub

    Private Sub SetAsDisplayNameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetAsDisplayNameToolStripMenuItem.Click
        If ListBox1.SelectedIndex < 0 Then
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
        edit.ShowDialog()

        Dim current = all_xrefs.GroupBy(Function(a) a.xref).ToDictionary(Function(a) a.Key, Function(a) a.ToArray)

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

        Call refreshXrefs()
    End Sub
End Class
