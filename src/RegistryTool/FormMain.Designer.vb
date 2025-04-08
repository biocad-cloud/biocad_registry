<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VocabularyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoleculesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SubCellularCompartmentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlavorOdorsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExportMetabolitesDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.GenBankToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.DataToolStripMenuItem, Me.ImportsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(113, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'DataToolStripMenuItem
        '
        Me.DataToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VocabularyToolStripMenuItem, Me.MoleculesToolStripMenuItem, Me.SubCellularCompartmentsToolStripMenuItem, Me.FlavorOdorsToolStripMenuItem, Me.ToolStripMenuItem2, Me.ExportMetabolitesDatabaseToolStripMenuItem})
        Me.DataToolStripMenuItem.Name = "DataToolStripMenuItem"
        Me.DataToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.DataToolStripMenuItem.Text = "Data"
        '
        'VocabularyToolStripMenuItem
        '
        Me.VocabularyToolStripMenuItem.Name = "VocabularyToolStripMenuItem"
        Me.VocabularyToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.VocabularyToolStripMenuItem.Text = "Vocabulary"
        '
        'MoleculesToolStripMenuItem
        '
        Me.MoleculesToolStripMenuItem.Name = "MoleculesToolStripMenuItem"
        Me.MoleculesToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.MoleculesToolStripMenuItem.Text = "Molecules"
        '
        'SubCellularCompartmentsToolStripMenuItem
        '
        Me.SubCellularCompartmentsToolStripMenuItem.Name = "SubCellularCompartmentsToolStripMenuItem"
        Me.SubCellularCompartmentsToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.SubCellularCompartmentsToolStripMenuItem.Text = "SubCellular Compartments"
        '
        'FlavorOdorsToolStripMenuItem
        '
        Me.FlavorOdorsToolStripMenuItem.Name = "FlavorOdorsToolStripMenuItem"
        Me.FlavorOdorsToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.FlavorOdorsToolStripMenuItem.Text = "Flavor Odors"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(221, 6)
        '
        'ExportMetabolitesDatabaseToolStripMenuItem
        '
        Me.ExportMetabolitesDatabaseToolStripMenuItem.Name = "ExportMetabolitesDatabaseToolStripMenuItem"
        Me.ExportMetabolitesDatabaseToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ExportMetabolitesDatabaseToolStripMenuItem.Text = "Export Metabolites Database"
        '
        'ImportsToolStripMenuItem
        '
        Me.ImportsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GenBankToolStripMenuItem})
        Me.ImportsToolStripMenuItem.Name = "ImportsToolStripMenuItem"
        Me.ImportsToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.ImportsToolStripMenuItem.Text = "Imports"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 374)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(800, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(42, 17)
        Me.ToolStripStatusLabel1.Text = "Ready!"
        '
        'GenBankToolStripMenuItem
        '
        Me.GenBankToolStripMenuItem.Name = "GenBankToolStripMenuItem"
        Me.GenBankToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.GenBankToolStripMenuItem.Text = "GenBank"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 396)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormMain"
        Me.Text = "Registry Tool"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VocabularyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SubCellularCompartmentsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents MoleculesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents ExportMetabolitesDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents FlavorOdorsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GenBankToolStripMenuItem As ToolStripMenuItem
End Class
