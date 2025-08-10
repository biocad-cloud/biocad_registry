Public Class FormBatchEditor

    Dim list As MoleculeBatchView()

    Public Sub LoadFromIDSet(ids As IEnumerable(Of String))
        list = MoleculeBatchView.FromIdSet(ids.Select(Function(id) UInteger.Parse(id.Match("\d+"))))

        For Each mol As MoleculeBatchView In list
            Dim offset = DataGridView1.Rows.Add(mol.id, mol.name, mol.tags, mol.note)
            DataGridView1.Rows(offset).Tag = mol
        Next

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub
End Class