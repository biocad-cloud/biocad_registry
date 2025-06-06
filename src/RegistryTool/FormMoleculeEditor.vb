Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
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
    End Sub
End Class