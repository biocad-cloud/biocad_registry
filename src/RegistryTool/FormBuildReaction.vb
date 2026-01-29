Imports Galaxy.Workbench.CommonDialogs
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports RegistryTool.My
Imports SMRUCC.genomics.ComponentModel.EquaionModel.DefaultTypes

Public Class FormBuildReaction
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text.StringEmpty(, True) Then
            Return
        End If
        If ListBox1.Items.Count = 0 Then
            Return
        End If
        If ListBox2.Items.Count = 0 Then
            Return
        End If

        If Create() Then
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Function Create() As Boolean
        Dim links As New List(Of (role_id As UInteger, species_id As UInteger))
        Dim eq As New Equation
        Dim left As New List(Of CompoundSpecieReference)
        Dim right As New List(Of CompoundSpecieReference)

        For Each symbol As SymbolHolder In ListBox1.Items
            Call links.Add((symbol.role, symbol.species_id))
            Call left.Add(New CompoundSpecieReference With {.ID = symbol.note, .Stoichiometry = symbol.factor})
        Next
        For Each symbol As SymbolHolder In ListBox2.Items
            Call links.Add((symbol.role, symbol.species_id))
            Call right.Add(New CompoundSpecieReference With {.ID = symbol.note, .Stoichiometry = symbol.factor})
        Next

        eq.Reactants = left.ToArray
        eq.Products = right.ToArray

        Dim hashcode As String = links.CalculateReactionHashCode
        Dim rxn = MyApplication.biocad_registry.reaction.where(field("hashcode") = hashcode).find(Of biocad_registryModel.reaction)

        If rxn IsNot Nothing Then
            ' just make reaction data updates
            MessageBox.Show("target reaction model is already been existed inside the registry, just update the reaction metadata")
            Return False
        Else
            Dim max_id As UInteger = MyApplication.biocad_registry.reaction.aggregate(Of UInteger)("max(id)") + 1
            Dim id As String = $"BIOCAD_RXN-{max_id}"

            Call MyApplication.biocad_registry.reaction.add(
                field("db_xref") = id,
                field("db_source") = MyApplication.biocad_registry.biocad_vocabulary.db_ManualAudit,
                field("hashcode") = hashcode,
                field("main_id") = 0,
                field("name") = Strings.Trim(TextBox1.Text),
                field("ec_number") = Strings.Trim(TextBox3.Text),
                field("equation") = eq.ToString,
                field("note") = TextBox2.Text
            )

            rxn = MyApplication.biocad_registry.reaction.where(field("hashcode") = hashcode, field("db_xref") = id).find(Of biocad_registryModel.reaction)

            If rxn Is Nothing Then
                Return False
            End If

            Dim net As CommitTransaction = MyApplication.biocad_registry.metabolic_network.ignore.open_transaction

            For Each symbol As SymbolHolder In ListBox1.Items
                Call net.add(field("reaction_id") = rxn.id,
                             field("factor") = symbol.factor,
                             field("species_id") = symbol.species_id,
                             field("symbol_id") = symbol.symbol_id,
                             field("role") = symbol.role,
                             field("compartment_id") = 1,
                             field("note") = symbol.note)
            Next
            For Each symbol As SymbolHolder In ListBox2.Items
                Call net.add(field("reaction_id") = rxn.id,
                     field("factor") = symbol.factor,
                     field("species_id") = symbol.species_id,
                     field("symbol_id") = symbol.symbol_id,
                     field("role") = symbol.role,
                     field("compartment_id") = 1,
                     field("note") = symbol.note)
            Next

            Call net.commit()

            Return True
        End If
    End Function

    ''' <summary>
    ''' add left
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        Call InputDialog.Input(Of FormSetSubstrate)(
            Sub(s)
                Dim symbol As SymbolHolder = s.GetSymbol
                symbol.role = MyApplication.biocad_registry.MetabolicSubstrateRole.id
                ListBox1.Items.Add(symbol)
            End Sub)
    End Sub

    ''' <summary>
    ''' add right
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem1.Click
        Call InputDialog.Input(Of FormSetSubstrate)(
            Sub(s)
                Dim symbol As SymbolHolder = s.GetSymbol
                symbol.role = MyApplication.biocad_registry.MetabolicProductRole.id
                ListBox2.Items.Add(symbol)
            End Sub)
    End Sub
End Class

Public Class SymbolHolder

    Public Property factor As Double
    Public Property species_id As UInteger
    Public Property role As UInteger
    Public Property symbol_id As String
    Public Property note As String

    Public Overrides Function ToString() As String
        Return $"{factor} {note}"
    End Function

End Class