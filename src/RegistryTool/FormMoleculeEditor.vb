Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports BioNovoGene.BioDeep.Chemoinformatics.SMILES
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

    Private Sub FormMoleculeEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim mol = MyApplication.biocad_registry.molecule _
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

        struct = MyApplication.biocad_registry.sequence_graph _
            .where(field("molecule_id") = mol.id) _
            .find(Of biocad_registryModel.sequence_graph)

        If Not struct Is Nothing Then
            TextBox1.Text = struct.sequence
            TextBox5.Text = struct.morgan
        End If

        Dim xrefs = MyApplication.biocad_registry.db_xrefs _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("db_key")) _
            .where(field("obj_id") = mol.id) _
            .select(Of XrefID)("db_xrefs.id as xref_id", "term as dbname", "xref")

        DataGridView1.Rows.Clear()

        For Each id As XrefID In xrefs
            Dim offset = DataGridView1.Rows.Add(id.dbname, id.xref)
            DataGridView1.Rows(offset).Tag = id
        Next

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)

        Call WebKit.Init(WebView21)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
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
End Class

Public Class XrefID

    <DatabaseField> Public Property xref_id As UInteger
    <DatabaseField> Public Property dbname As String
    <DatabaseField> Public Property xref As String

End Class