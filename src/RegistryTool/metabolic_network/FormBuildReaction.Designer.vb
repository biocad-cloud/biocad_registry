Imports Galaxy.Workbench.CommonDialogs

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormBuildReaction
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
        Label2 = New Label()
        TextBox1 = New TextBox()
        TextBox2 = New TextBox()
        Label3 = New Label()
        TextBox3 = New TextBox()
        Label4 = New Label()
        Label5 = New Label()
        ListBox1 = New ListBox()
        ContextMenuStrip1 = New ContextMenuStrip(components)
        AddToolStripMenuItem = New ToolStripMenuItem()
        ListBox2 = New ListBox()
        ContextMenuStrip2 = New ContextMenuStrip(components)
        AddToolStripMenuItem1 = New ToolStripMenuItem()
        Button1 = New Button()
        Button2 = New Button()
        GroupBox1 = New GroupBox()
        ContextMenuStrip1.SuspendLayout()
        ContextMenuStrip2.SuspendLayout()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(14, 27)
        Label1.Name = "Label1"
        Label1.Size = New Size(91, 15)
        Label1.TabIndex = 0
        Label1.Text = "Reaction Name:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(69, 58)
        Label2.Name = "Label2"
        Label2.Size = New Size(36, 15)
        Label2.TabIndex = 1
        Label2.Text = "Note:"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(120, 24)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(548, 23)
        TextBox1.TabIndex = 2
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(120, 55)
        TextBox2.Multiline = True
        TextBox2.Name = "TextBox2"
        TextBox2.ScrollBars = ScrollBars.Both
        TextBox2.Size = New Size(548, 141)
        TextBox2.TabIndex = 3
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(34, 211)
        Label3.Name = "Label3"
        Label3.Size = New Size(71, 15)
        Label3.TabIndex = 4
        Label3.Text = "EC Number:"
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(120, 208)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(146, 23)
        TextBox3.TabIndex = 5
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(34, 249)
        Label4.Name = "Label4"
        Label4.Size = New Size(61, 15)
        Label4.TabIndex = 6
        Label4.Text = "Substrates"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(368, 249)
        Label5.Name = "Label5"
        Label5.Size = New Size(54, 15)
        Label5.TabIndex = 7
        Label5.Text = "Products"
        ' 
        ' ListBox1
        ' 
        ListBox1.ContextMenuStrip = ContextMenuStrip1
        ListBox1.FormattingEnabled = True
        ListBox1.ItemHeight = 15
        ListBox1.Location = New Point(34, 272)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(306, 289)
        ListBox1.TabIndex = 8
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {AddToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(97, 26)
        ' 
        ' AddToolStripMenuItem
        ' 
        AddToolStripMenuItem.Name = "AddToolStripMenuItem"
        AddToolStripMenuItem.Size = New Size(96, 22)
        AddToolStripMenuItem.Text = "Add"
        ' 
        ' ListBox2
        ' 
        ListBox2.ContextMenuStrip = ContextMenuStrip2
        ListBox2.FormattingEnabled = True
        ListBox2.ItemHeight = 15
        ListBox2.Location = New Point(368, 272)
        ListBox2.Name = "ListBox2"
        ListBox2.Size = New Size(300, 289)
        ListBox2.TabIndex = 9
        ' 
        ' ContextMenuStrip2
        ' 
        ContextMenuStrip2.Items.AddRange(New ToolStripItem() {AddToolStripMenuItem1})
        ContextMenuStrip2.Name = "ContextMenuStrip2"
        ContextMenuStrip2.Size = New Size(97, 26)
        ' 
        ' AddToolStripMenuItem1
        ' 
        AddToolStripMenuItem1.Name = "AddToolStripMenuItem1"
        AddToolStripMenuItem1.Size = New Size(96, 22)
        AddToolStripMenuItem1.Text = "Add"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(512, 594)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 10
        Button1.Text = "Cancel"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(602, 594)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 23)
        Button2.TabIndex = 11
        Button2.Text = "OK"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(TextBox1)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(TextBox2)
        GroupBox1.Controls.Add(ListBox1)
        GroupBox1.Controls.Add(ListBox2)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(TextBox3)
        GroupBox1.Controls.Add(Label5)
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(682, 570)
        GroupBox1.TabIndex = 12
        GroupBox1.TabStop = False
        GroupBox1.Text = "Create New Metabolic Reaction"
        ' 
        ' FormBuildReaction
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(705, 630)
        Controls.Add(GroupBox1)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Name = "FormBuildReaction"
        Text = "Create Reaction"
        ContextMenuStrip1.ResumeLayout(False)
        ContextMenuStrip2.ResumeLayout(False)
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents AddToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents GroupBox1 As GroupBox
End Class
