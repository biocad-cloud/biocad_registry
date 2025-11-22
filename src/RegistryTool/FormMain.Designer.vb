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
        components = New ComponentModel.Container()
        MenuStrip1 = New MenuStrip()
        FileToolStripMenuItem = New ToolStripMenuItem()
        OpenMoleculeToolStripMenuItem = New ToolStripMenuItem()
        BatchOperationToolStripMenuItem = New ToolStripMenuItem()
        SearchNamesToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem4 = New ToolStripSeparator()
        SettingsToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem1 = New ToolStripSeparator()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        DataToolStripMenuItem = New ToolStripMenuItem()
        VocabularyToolStripMenuItem = New ToolStripMenuItem()
        MoleculesToolStripMenuItem = New ToolStripMenuItem()
        SearchToolStripMenuItem = New ToolStripMenuItem()
        SearchNameToolStripMenuItem = New ToolStripMenuItem()
        SubCellularCompartmentsToolStripMenuItem = New ToolStripMenuItem()
        FlavorOdorsToolStripMenuItem = New ToolStripMenuItem()
        ReactionEditorToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem2 = New ToolStripSeparator()
        ExportMetabolitesDatabaseToolStripMenuItem = New ToolStripMenuItem()
        ExportKEGGMetaboliteTableToolStripMenuItem = New ToolStripMenuItem()
        ExportEnzymeDatabaseToolStripMenuItem = New ToolStripMenuItem()
        ExportMembraneTransporterToolStripMenuItem = New ToolStripMenuItem()
        ExportConservedOperonDatabaseToolStripMenuItem = New ToolStripMenuItem()
        ExportKEGGIDMappingToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem3 = New ToolStripSeparator()
        ExportBloodTagToolStripMenuItem = New ToolStripMenuItem()
        ExportAnnotationTableToolStripMenuItem = New ToolStripMenuItem()
        ImportsToolStripMenuItem = New ToolStripMenuItem()
        GenBankToolStripMenuItem = New ToolStripMenuItem()
        PubMedKnowledgeToolStripMenuItem = New ToolStripMenuItem()
        WindowsToolStripMenuItem = New ToolStripMenuItem()
        CloseAllToolStripMenuItem = New ToolStripMenuItem()
        StatusStrip1 = New StatusStrip()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        m_dockPanel = New Microsoft.VisualStudio.WinForms.Docking.DockPanel()
        VisualStudioToolStripExtender1 = New Microsoft.VisualStudio.WinForms.Docking.VisualStudioToolStripExtender(components)
        MenuStrip1.SuspendLayout()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {FileToolStripMenuItem, DataToolStripMenuItem, ImportsToolStripMenuItem, WindowsToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Padding = New Padding(7, 2, 0, 2)
        MenuStrip1.Size = New Size(836, 24)
        MenuStrip1.TabIndex = 1
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' FileToolStripMenuItem
        ' 
        FileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {OpenMoleculeToolStripMenuItem, BatchOperationToolStripMenuItem, SearchNamesToolStripMenuItem, ToolStripMenuItem4, SettingsToolStripMenuItem, AboutToolStripMenuItem, ToolStripMenuItem1, ExitToolStripMenuItem})
        FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        FileToolStripMenuItem.Size = New Size(37, 20)
        FileToolStripMenuItem.Text = "File"
        ' 
        ' OpenMoleculeToolStripMenuItem
        ' 
        OpenMoleculeToolStripMenuItem.Name = "OpenMoleculeToolStripMenuItem"
        OpenMoleculeToolStripMenuItem.Size = New Size(160, 22)
        OpenMoleculeToolStripMenuItem.Text = "Open Molecule"
        ' 
        ' BatchOperationToolStripMenuItem
        ' 
        BatchOperationToolStripMenuItem.Name = "BatchOperationToolStripMenuItem"
        BatchOperationToolStripMenuItem.Size = New Size(160, 22)
        BatchOperationToolStripMenuItem.Text = "Batch Operation"
        ' 
        ' SearchNamesToolStripMenuItem
        ' 
        SearchNamesToolStripMenuItem.Name = "SearchNamesToolStripMenuItem"
        SearchNamesToolStripMenuItem.Size = New Size(160, 22)
        SearchNamesToolStripMenuItem.Text = "Search Names"
        ' 
        ' ToolStripMenuItem4
        ' 
        ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        ToolStripMenuItem4.Size = New Size(157, 6)
        ' 
        ' SettingsToolStripMenuItem
        ' 
        SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        SettingsToolStripMenuItem.Size = New Size(160, 22)
        SettingsToolStripMenuItem.Text = "Settings"
        ' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(160, 22)
        AboutToolStripMenuItem.Text = "About"
        ' 
        ' ToolStripMenuItem1
        ' 
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New Size(157, 6)
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.Size = New Size(160, 22)
        ExitToolStripMenuItem.Text = "Exit"
        ' 
        ' DataToolStripMenuItem
        ' 
        DataToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {VocabularyToolStripMenuItem, MoleculesToolStripMenuItem, SubCellularCompartmentsToolStripMenuItem, FlavorOdorsToolStripMenuItem, ReactionEditorToolStripMenuItem, ToolStripMenuItem2, ExportMetabolitesDatabaseToolStripMenuItem, ExportEnzymeDatabaseToolStripMenuItem, ExportConservedOperonDatabaseToolStripMenuItem, ExportKEGGIDMappingToolStripMenuItem, ToolStripMenuItem3, ExportBloodTagToolStripMenuItem, ExportAnnotationTableToolStripMenuItem})
        DataToolStripMenuItem.Name = "DataToolStripMenuItem"
        DataToolStripMenuItem.Size = New Size(43, 20)
        DataToolStripMenuItem.Text = "Data"
        ' 
        ' VocabularyToolStripMenuItem
        ' 
        VocabularyToolStripMenuItem.Name = "VocabularyToolStripMenuItem"
        VocabularyToolStripMenuItem.Size = New Size(261, 22)
        VocabularyToolStripMenuItem.Text = "Vocabulary"
        ' 
        ' MoleculesToolStripMenuItem
        ' 
        MoleculesToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SearchToolStripMenuItem, SearchNameToolStripMenuItem})
        MoleculesToolStripMenuItem.Name = "MoleculesToolStripMenuItem"
        MoleculesToolStripMenuItem.Size = New Size(261, 22)
        MoleculesToolStripMenuItem.Text = "Molecules"
        ' 
        ' SearchToolStripMenuItem
        ' 
        SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
        SearchToolStripMenuItem.Size = New Size(144, 22)
        SearchToolStripMenuItem.Text = "Search Text"
        ' 
        ' SearchNameToolStripMenuItem
        ' 
        SearchNameToolStripMenuItem.Name = "SearchNameToolStripMenuItem"
        SearchNameToolStripMenuItem.Size = New Size(144, 22)
        SearchNameToolStripMenuItem.Text = "Search Name"
        ' 
        ' SubCellularCompartmentsToolStripMenuItem
        ' 
        SubCellularCompartmentsToolStripMenuItem.Name = "SubCellularCompartmentsToolStripMenuItem"
        SubCellularCompartmentsToolStripMenuItem.Size = New Size(261, 22)
        SubCellularCompartmentsToolStripMenuItem.Text = "SubCellular Compartments"
        ' 
        ' FlavorOdorsToolStripMenuItem
        ' 
        FlavorOdorsToolStripMenuItem.Name = "FlavorOdorsToolStripMenuItem"
        FlavorOdorsToolStripMenuItem.Size = New Size(261, 22)
        FlavorOdorsToolStripMenuItem.Text = "Flavor Odors"
        ' 
        ' ReactionEditorToolStripMenuItem
        ' 
        ReactionEditorToolStripMenuItem.Name = "ReactionEditorToolStripMenuItem"
        ReactionEditorToolStripMenuItem.Size = New Size(261, 22)
        ReactionEditorToolStripMenuItem.Text = "Reaction Editor"
        ' 
        ' ToolStripMenuItem2
        ' 
        ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        ToolStripMenuItem2.Size = New Size(258, 6)
        ' 
        ' ExportMetabolitesDatabaseToolStripMenuItem
        ' 
        ExportMetabolitesDatabaseToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ExportKEGGMetaboliteTableToolStripMenuItem})
        ExportMetabolitesDatabaseToolStripMenuItem.Name = "ExportMetabolitesDatabaseToolStripMenuItem"
        ExportMetabolitesDatabaseToolStripMenuItem.Size = New Size(261, 22)
        ExportMetabolitesDatabaseToolStripMenuItem.Text = "Export Metabolites Database"
        ' 
        ' ExportKEGGMetaboliteTableToolStripMenuItem
        ' 
        ExportKEGGMetaboliteTableToolStripMenuItem.Name = "ExportKEGGMetaboliteTableToolStripMenuItem"
        ExportKEGGMetaboliteTableToolStripMenuItem.Size = New Size(230, 22)
        ExportKEGGMetaboliteTableToolStripMenuItem.Text = "Export KEGG Metabolite Table"
        ' 
        ' ExportEnzymeDatabaseToolStripMenuItem
        ' 
        ExportEnzymeDatabaseToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ExportMembraneTransporterToolStripMenuItem})
        ExportEnzymeDatabaseToolStripMenuItem.Name = "ExportEnzymeDatabaseToolStripMenuItem"
        ExportEnzymeDatabaseToolStripMenuItem.Size = New Size(261, 22)
        ExportEnzymeDatabaseToolStripMenuItem.Text = "Export Enzyme Database"
        ' 
        ' ExportMembraneTransporterToolStripMenuItem
        ' 
        ExportMembraneTransporterToolStripMenuItem.Name = "ExportMembraneTransporterToolStripMenuItem"
        ExportMembraneTransporterToolStripMenuItem.Size = New Size(231, 22)
        ExportMembraneTransporterToolStripMenuItem.Text = "Export Membrane Transporter"
        ' 
        ' ExportConservedOperonDatabaseToolStripMenuItem
        ' 
        ExportConservedOperonDatabaseToolStripMenuItem.Name = "ExportConservedOperonDatabaseToolStripMenuItem"
        ExportConservedOperonDatabaseToolStripMenuItem.Size = New Size(261, 22)
        ExportConservedOperonDatabaseToolStripMenuItem.Text = "Export Conserved Operon Database"
        ' 
        ' ExportKEGGIDMappingToolStripMenuItem
        ' 
        ExportKEGGIDMappingToolStripMenuItem.Name = "ExportKEGGIDMappingToolStripMenuItem"
        ExportKEGGIDMappingToolStripMenuItem.Size = New Size(261, 22)
        ExportKEGGIDMappingToolStripMenuItem.Text = "Export KEGG IDMapping"
        ' 
        ' ToolStripMenuItem3
        ' 
        ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        ToolStripMenuItem3.Size = New Size(258, 6)
        ' 
        ' ExportBloodTagToolStripMenuItem
        ' 
        ExportBloodTagToolStripMenuItem.Name = "ExportBloodTagToolStripMenuItem"
        ExportBloodTagToolStripMenuItem.Size = New Size(261, 22)
        ExportBloodTagToolStripMenuItem.Text = "Export Tag Data"
        ' 
        ' ExportAnnotationTableToolStripMenuItem
        ' 
        ExportAnnotationTableToolStripMenuItem.Name = "ExportAnnotationTableToolStripMenuItem"
        ExportAnnotationTableToolStripMenuItem.Size = New Size(261, 22)
        ExportAnnotationTableToolStripMenuItem.Text = "Export Annotation Table"
        ' 
        ' ImportsToolStripMenuItem
        ' 
        ImportsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {GenBankToolStripMenuItem, PubMedKnowledgeToolStripMenuItem})
        ImportsToolStripMenuItem.Name = "ImportsToolStripMenuItem"
        ImportsToolStripMenuItem.Size = New Size(60, 20)
        ImportsToolStripMenuItem.Text = "Imports"
        ' 
        ' GenBankToolStripMenuItem
        ' 
        GenBankToolStripMenuItem.Name = "GenBankToolStripMenuItem"
        GenBankToolStripMenuItem.Size = New Size(181, 22)
        GenBankToolStripMenuItem.Text = "GenBank"
        ' 
        ' PubMedKnowledgeToolStripMenuItem
        ' 
        PubMedKnowledgeToolStripMenuItem.Name = "PubMedKnowledgeToolStripMenuItem"
        PubMedKnowledgeToolStripMenuItem.Size = New Size(181, 22)
        PubMedKnowledgeToolStripMenuItem.Text = "PubMed Knowledge"
        ' 
        ' WindowsToolStripMenuItem
        ' 
        WindowsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {CloseAllToolStripMenuItem})
        WindowsToolStripMenuItem.Name = "WindowsToolStripMenuItem"
        WindowsToolStripMenuItem.Size = New Size(68, 20)
        WindowsToolStripMenuItem.Text = "Windows"
        ' 
        ' CloseAllToolStripMenuItem
        ' 
        CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        CloseAllToolStripMenuItem.Size = New Size(120, 22)
        CloseAllToolStripMenuItem.Text = "Close All"
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {ToolStripStatusLabel1})
        StatusStrip1.Location = New Point(0, 405)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Padding = New Padding(1, 0, 16, 0)
        StatusStrip1.Size = New Size(836, 22)
        StatusStrip1.TabIndex = 3
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New Size(42, 17)
        ToolStripStatusLabel1.Text = "Ready!"
        ' 
        ' m_dockPanel
        ' 
        m_dockPanel.Dock = DockStyle.Fill
        m_dockPanel.Location = New Point(0, 24)
        m_dockPanel.Margin = New Padding(4, 4, 4, 4)
        m_dockPanel.Name = "m_dockPanel"
        m_dockPanel.Size = New Size(836, 381)
        m_dockPanel.TabIndex = 4
        ' 
        ' VisualStudioToolStripExtender1
        ' 
        VisualStudioToolStripExtender1.DefaultRenderer = Nothing
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(836, 427)
        Controls.Add(m_dockPanel)
        Controls.Add(StatusStrip1)
        Controls.Add(MenuStrip1)
        MainMenuStrip = MenuStrip1
        Margin = New Padding(4, 4, 4, 4)
        Name = "FormMain"
        Text = "Registry Tool"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

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
    Friend WithEvents BatchOperationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportConservedOperonDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportKEGGMetaboliteTableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportMembraneTransporterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchNameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReactionEditorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents m_dockPanel As Microsoft.VisualStudio.WinForms.Docking.DockPanel
    Friend WithEvents VisualStudioToolStripExtender1 As Microsoft.VisualStudio.WinForms.Docking.VisualStudioToolStripExtender
End Class
