Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

Public Class FormMoleculeEditor

    Public id As String

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

        Dim struct = MyApplication.biocad_registry.sequence_graph _
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
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim name As String = Strings.Trim(TextBox2.Text)
        MyApplication.biocad_registry.molecule.where(field("id") = UInteger.Parse(id.Match("\d+"))).save(field("name") = name)
    End Sub
End Class

Public Class XrefID

    <DatabaseField> Public Property xref_id As UInteger
    <DatabaseField> Public Property dbname As String
    <DatabaseField> Public Property xref As String

End Class