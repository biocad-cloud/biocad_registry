Imports Galaxy.Workbench.CommonDialogs

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormSetSubstrate
    Inherits InputDialog

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Label1 = New Label()
        TextBox1 = New TextBox()
        Button1 = New Button()
        Label2 = New Label()
        NumericUpDown1 = New NumericUpDown()
        Label3 = New Label()
        Label4 = New Label()
        ListBox1 = New ListBox()
        ContextMenuStrip1 = New ContextMenuStrip(components)
        SetMoleculeReferenceToolStripMenuItem = New ToolStripMenuItem()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(24, 19)
        Label1.Name = "Label1"
        Label1.Size = New Size(80, 15)
        Label1.TabIndex = 0
        Label1.Text = "Search Name:"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(134, 16)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(325, 23)
        TextBox1.TabIndex = 1
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(477, 16)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 2
        Button1.Text = "Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(61, 50)
        Label2.Name = "Label2"
        Label2.Size = New Size(43, 15)
        Label2.TabIndex = 3
        Label2.Text = "Factor:"
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Location = New Point(134, 48)
        NumericUpDown1.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(120, 23)
        NumericUpDown1.TabIndex = 4
        NumericUpDown1.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(50, 88)
        Label3.Name = "Label3"
        Label3.Size = New Size(54, 15)
        Label3.TabIndex = 5
        Label3.Text = "Selected:"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(134, 88)
        Label4.Name = "Label4"
        Label4.Size = New Size(52, 15)
        Label4.TabIndex = 6
        Label4.Text = "<NULL>"
        ' 
        ' ListBox1
        ' 
        ListBox1.ContextMenuStrip = ContextMenuStrip1
        ListBox1.FormattingEnabled = True
        ListBox1.ItemHeight = 15
        ListBox1.Location = New Point(24, 122)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(635, 244)
        ListBox1.TabIndex = 7
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {SetMoleculeReferenceToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(198, 26)
        ' 
        ' SetMoleculeReferenceToolStripMenuItem
        ' 
        SetMoleculeReferenceToolStripMenuItem.Name = "SetMoleculeReferenceToolStripMenuItem"
        SetMoleculeReferenceToolStripMenuItem.Size = New Size(197, 22)
        SetMoleculeReferenceToolStripMenuItem.Text = "Set Molecule Reference"
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(665, 306)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 23)
        Button2.TabIndex = 8
        Button2.Text = "OK"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(665, 343)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 23)
        Button3.TabIndex = 9
        Button3.Text = "Cancel"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(584, 16)
        Button4.Name = "Button4"
        Button4.Size = New Size(131, 23)
        Button4.TabIndex = 10
        Button4.Text = "Create Placeholder"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' FormSetSubstrate
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(743, 403)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(ListBox1)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(NumericUpDown1)
        Controls.Add(Label2)
        Controls.Add(Button1)
        Controls.Add(TextBox1)
        Controls.Add(Label1)
        Name = "FormSetSubstrate"
        Text = "Form1"
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SetMoleculeReferenceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
End Class
