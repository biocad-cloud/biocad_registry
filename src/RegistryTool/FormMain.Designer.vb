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
        Me.OpenMoleculeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VocabularyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoleculesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SubCellularCompartmentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlavorOdorsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExportMetabolitesDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportEnzymeDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportKEGGIDMappingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExportBloodTagToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportAnnotationTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenBankToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PubMedKnowledgeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SearchNamesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.DataToolStripMenuItem, Me.ImportsToolStripMenuItem, Me.WindowsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1535, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenMoleculeToolStripMenuItem, Me.SearchNamesToolStripMenuItem, Me.ToolStripMenuItem4, Me.SettingsToolStripMenuItem, Me.AboutToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenMoleculeToolStripMenuItem
        '
        Me.OpenMoleculeToolStripMenuItem.Name = "OpenMoleculeToolStripMenuItem"
        Me.OpenMoleculeToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OpenMoleculeToolStripMenuItem.Text = "Open Molecule"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(177, 6)
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'DataToolStripMenuItem
        '
        Me.DataToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VocabularyToolStripMenuItem, Me.MoleculesToolStripMenuItem, Me.SubCellularCompartmentsToolStripMenuItem, Me.FlavorOdorsToolStripMenuItem, Me.ToolStripMenuItem2, Me.ExportMetabolitesDatabaseToolStripMenuItem, Me.ExportEnzymeDatabaseToolStripMenuItem, Me.ExportKEGGIDMappingToolStripMenuItem, Me.ToolStripMenuItem3, Me.ExportBloodTagToolStripMenuItem, Me.ExportAnnotationTableToolStripMenuItem})
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
        'ExportEnzymeDatabaseToolStripMenuItem
        '
        Me.ExportEnzymeDatabaseToolStripMenuItem.Name = "ExportEnzymeDatabaseToolStripMenuItem"
        Me.ExportEnzymeDatabaseToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ExportEnzymeDatabaseToolStripMenuItem.Text = "Export Enzyme Database"
        '
        'ExportKEGGIDMappingToolStripMenuItem
        '
        Me.ExportKEGGIDMappingToolStripMenuItem.Name = "ExportKEGGIDMappingToolStripMenuItem"
        Me.ExportKEGGIDMappingToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ExportKEGGIDMappingToolStripMenuItem.Text = "Export KEGG IDMapping"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(221, 6)
        '
        'ExportBloodTagToolStripMenuItem
        '
        Me.ExportBloodTagToolStripMenuItem.Name = "ExportBloodTagToolStripMenuItem"
        Me.ExportBloodTagToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ExportBloodTagToolStripMenuItem.Text = "Export Tag Data"
        '
        'ExportAnnotationTableToolStripMenuItem
        '
        Me.ExportAnnotationTableToolStripMenuItem.Name = "ExportAnnotationTableToolStripMenuItem"
        Me.ExportAnnotationTableToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ExportAnnotationTableToolStripMenuItem.Text = "Export Annotation Table"
        '
        'ImportsToolStripMenuItem
        '
        Me.ImportsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GenBankToolStripMenuItem, Me.PubMedKnowledgeToolStripMenuItem})
        Me.ImportsToolStripMenuItem.Name = "ImportsToolStripMenuItem"
        Me.ImportsToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.ImportsToolStripMenuItem.Text = "Imports"
        '
        'GenBankToolStripMenuItem
        '
        Me.GenBankToolStripMenuItem.Name = "GenBankToolStripMenuItem"
        Me.GenBankToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.GenBankToolStripMenuItem.Text = "GenBank"
        '
        'PubMedKnowledgeToolStripMenuItem
        '
        Me.PubMedKnowledgeToolStripMenuItem.Name = "PubMedKnowledgeToolStripMenuItem"
        Me.PubMedKnowledgeToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.PubMedKnowledgeToolStripMenuItem.Text = "PubMed Knowledge"
        '
        'WindowsToolStripMenuItem
        '
        Me.WindowsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseAllToolStripMenuItem})
        Me.WindowsToolStripMenuItem.Name = "WindowsToolStripMenuItem"
        Me.WindowsToolStripMenuItem.Size = New System.Drawing.Size(68, 20)
        Me.WindowsToolStripMenuItem.Text = "Windows"
        '
        'CloseAllToolStripMenuItem
        '
        Me.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        Me.CloseAllToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.CloseAllToolStripMenuItem.Text = "Close All"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 892)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1535, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(42, 17)
        Me.ToolStripStatusLabel1.Text = "Ready!"
        '
        'SearchNamesToolStripMenuItem
        '
        Me.SearchNamesToolStripMenuItem.Name = "SearchNamesToolStripMenuItem"
        Me.SearchNamesToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SearchNamesToolStripMenuItem.Text = "Search Names"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1535, 914)
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
    Friend WithEvents ExportEnzymeDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportKEGGIDMappingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents ExportBloodTagToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PubMedKnowledgeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportAnnotationTableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenMoleculeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WindowsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchNamesToolStripMenuItem As ToolStripMenuItem
End Class
