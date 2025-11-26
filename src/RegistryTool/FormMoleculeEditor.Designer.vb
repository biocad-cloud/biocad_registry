Imports Galaxy.Workbench.DockDocument

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMoleculeEditor
    Inherits DocumentWindow

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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        DataGridView1 = New DataGridView()
        Column1 = New DataGridViewTextBoxColumn()
        Column2 = New DataGridViewTextBoxColumn()
        ContextMenuStrip3 = New ContextMenuStrip(components)
        EditToolStripMenuItem = New ToolStripMenuItem()
        SearchByThisDbXrefToolStripMenuItem = New ToolStripMenuItem()
        Label2 = New Label()
        TextBox1 = New TextBox()
        Button1 = New Button()
        WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        ListBox1 = New ListBox()
        ContextMenuStrip1 = New ContextMenuStrip(components)
        EditToolStripMenuItem1 = New ToolStripMenuItem()
        CopyToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem1 = New ToolStripSeparator()
        SetAsDisplayNameToolStripMenuItem = New ToolStripMenuItem()
        ChineseNameTranslationToolStripMenuItem = New ToolStripMenuItem()
        Label4 = New Label()
        ComboBox1 = New ComboBox()
        Button2 = New Button()
        Label5 = New Label()
        TextBox2 = New TextBox()
        Label6 = New Label()
        TextBox3 = New TextBox()
        Label7 = New Label()
        Label8 = New Label()
        TextBox4 = New TextBox()
        Button3 = New Button()
        Button4 = New Button()
        GroupBox1 = New GroupBox()
        Panel3 = New Panel()
        GroupBox2 = New GroupBox()
        Button8 = New Button()
        TextBox6 = New TextBox()
        ComboBox3 = New ComboBox()
        Button5 = New Button()
        GroupBox3 = New GroupBox()
        TextBox5 = New TextBox()
        Label1 = New Label()
        GroupBox4 = New GroupBox()
        Button7 = New Button()
        GroupBox5 = New GroupBox()
        ComboBox2 = New ComboBox()
        Button6 = New Button()
        ListBox2 = New ListBox()
        ContextMenuStrip2 = New ContextMenuStrip(components)
        ClearThisTagToolStripMenuItem = New ToolStripMenuItem()
        GroupBox6 = New GroupBox()
        ListBox3 = New ListBox()
        ContextMenuStrip4 = New ContextMenuStrip(components)
        ListSourceToolStripMenuItem = New ToolStripMenuItem()
        DataGridView2 = New DataGridView()
        Column3 = New DataGridViewTextBoxColumn()
        Column4 = New DataGridViewTextBoxColumn()
        SplitContainer1 = New SplitContainer()
        TextBox7 = New TextBox()
        DataGridView3 = New DataGridView()
        Column6 = New DataGridViewTextBoxColumn()
        Column11 = New DataGridViewTextBoxColumn()
        Column7 = New DataGridViewTextBoxColumn()
        Column8 = New DataGridViewTextBoxColumn()
        Column9 = New DataGridViewTextBoxColumn()
        Column10 = New DataGridViewTextBoxColumn()
        ContextMenuStrip5 = New ContextMenuStrip(components)
        OpenMoleculeDataToolStripMenuItem = New ToolStripMenuItem()
        ToolStrip1 = New ToolStrip()
        ToolStripLabel1 = New ToolStripLabel()
        LinkLabel1 = New ToolStripLabel()
        Panel1 = New Panel()
        Panel2 = New Panel()
        TabControl1 = New TabControl()
        TabPage1 = New TabPage()
        TabPage2 = New TabPage()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip3.SuspendLayout()
        CType(WebView21, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        GroupBox1.SuspendLayout()
        Panel3.SuspendLayout()
        GroupBox2.SuspendLayout()
        GroupBox3.SuspendLayout()
        GroupBox4.SuspendLayout()
        GroupBox5.SuspendLayout()
        ContextMenuStrip2.SuspendLayout()
        GroupBox6.SuspendLayout()
        ContextMenuStrip4.SuspendLayout()
        CType(DataGridView2, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(DataGridView3, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip5.SuspendLayout()
        ToolStrip1.SuspendLayout()
        Panel1.SuspendLayout()
        Panel2.SuspendLayout()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        TabPage2.SuspendLayout()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        DataGridView1.BackgroundColor = Color.LightGray
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Columns.AddRange(New DataGridViewColumn() {Column1, Column2})
        DataGridView1.ContextMenuStrip = ContextMenuStrip3
        DataGridView1.Location = New Point(15, 20)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowTemplate.Height = 23
        DataGridView1.ScrollBars = ScrollBars.Vertical
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(396, 351)
        DataGridView1.TabIndex = 0
        ' 
        ' Column1
        ' 
        Column1.HeaderText = "Database"
        Column1.Name = "Column1"
        Column1.ReadOnly = True
        ' 
        ' Column2
        ' 
        Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column2.HeaderText = "Xref ID"
        Column2.Name = "Column2"
        Column2.ReadOnly = True
        ' 
        ' ContextMenuStrip3
        ' 
        ContextMenuStrip3.Items.AddRange(New ToolStripItem() {EditToolStripMenuItem, SearchByThisDbXrefToolStripMenuItem})
        ContextMenuStrip3.Name = "ContextMenuStrip3"
        ContextMenuStrip3.Size = New Size(208, 48)
        ' 
        ' EditToolStripMenuItem
        ' 
        EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        EditToolStripMenuItem.Size = New Size(207, 22)
        EditToolStripMenuItem.Text = "Edit"
        ' 
        ' SearchByThisDbXrefToolStripMenuItem
        ' 
        SearchByThisDbXrefToolStripMenuItem.Name = "SearchByThisDbXrefToolStripMenuItem"
        SearchByThisDbXrefToolStripMenuItem.Size = New Size(207, 22)
        SearchByThisDbXrefToolStripMenuItem.Text = "Search By This Db_Xref ID"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(4, 392)
        Label2.Name = "Label2"
        Label2.Size = New Size(48, 15)
        Label2.TabIndex = 2
        Label2.Text = "SMILES:"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(6, 412)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(441, 23)
        TextBox1.TabIndex = 3
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(453, 412)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 4
        Button1.Text = "Save"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' WebView21
        ' 
        WebView21.AllowExternalDrop = True
        WebView21.CreationProperties = Nothing
        WebView21.DefaultBackgroundColor = Color.White
        WebView21.Location = New Point(6, 19)
        WebView21.Name = "WebView21"
        WebView21.Size = New Size(540, 370)
        WebView21.TabIndex = 5
        WebView21.ZoomFactor = 1R
        ' 
        ' ListBox1
        ' 
        ListBox1.ContextMenuStrip = ContextMenuStrip1
        ListBox1.Dock = DockStyle.Fill
        ListBox1.FormattingEnabled = True
        ListBox1.ItemHeight = 15
        ListBox1.Location = New Point(3, 55)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(1666, 352)
        ListBox1.TabIndex = 7
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {EditToolStripMenuItem1, CopyToolStripMenuItem, ToolStripMenuItem1, SetAsDisplayNameToolStripMenuItem, ChineseNameTranslationToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(212, 98)
        ' 
        ' EditToolStripMenuItem1
        ' 
        EditToolStripMenuItem1.Name = "EditToolStripMenuItem1"
        EditToolStripMenuItem1.Size = New Size(211, 22)
        EditToolStripMenuItem1.Text = "Edit"
        ' 
        ' CopyToolStripMenuItem
        ' 
        CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        CopyToolStripMenuItem.Size = New Size(211, 22)
        CopyToolStripMenuItem.Text = "Copy"
        ' 
        ' ToolStripMenuItem1
        ' 
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New Size(208, 6)
        ' 
        ' SetAsDisplayNameToolStripMenuItem
        ' 
        SetAsDisplayNameToolStripMenuItem.Name = "SetAsDisplayNameToolStripMenuItem"
        SetAsDisplayNameToolStripMenuItem.Size = New Size(211, 22)
        SetAsDisplayNameToolStripMenuItem.Text = "Set As Display Name"
        ' 
        ' ChineseNameTranslationToolStripMenuItem
        ' 
        ChineseNameTranslationToolStripMenuItem.Name = "ChineseNameTranslationToolStripMenuItem"
        ChineseNameTranslationToolStripMenuItem.Size = New Size(211, 22)
        ChineseNameTranslationToolStripMenuItem.Text = "Chinese Name Translation"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(7, 9)
        Label4.Name = "Label4"
        Label4.Size = New Size(96, 15)
        Label4.TabIndex = 8
        Label4.Text = "Filter Languages:"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.FormattingEnabled = True
        ComboBox1.Items.AddRange(New Object() {"*", "en", "zh"})
        ComboBox1.Location = New Point(109, 6)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(152, 23)
        ComboBox1.TabIndex = 9
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(419, 30)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 23)
        Button2.TabIndex = 10
        Button2.Text = "Save"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(13, 35)
        Label5.Name = "Label5"
        Label5.Size = New Size(42, 15)
        Label5.TabIndex = 11
        Label5.Text = "Name:"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(72, 30)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(339, 23)
        TextBox2.TabIndex = 12
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(13, 68)
        Label6.Name = "Label6"
        Label6.Size = New Size(54, 15)
        Label6.TabIndex = 13
        Label6.Text = "Formula:"
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(72, 63)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(103, 23)
        TextBox3.TabIndex = 14
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(191, 68)
        Label7.Name = "Label7"
        Label7.Size = New Size(65, 15)
        Label7.TabIndex = 15
        Label7.Text = "Exact Mass"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(13, 104)
        Label8.Name = "Label8"
        Label8.Size = New Size(99, 15)
        Label8.TabIndex = 16
        Label8.Text = "Description Note:"
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(15, 124)
        TextBox4.Multiline = True
        TextBox4.Name = "TextBox4"
        TextBox4.ScrollBars = ScrollBars.Vertical
        TextBox4.Size = New Size(396, 390)
        TextBox4.TabIndex = 17
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(419, 63)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 23)
        Button3.TabIndex = 18
        Button3.Text = "Save"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(422, 131)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 23)
        Button4.TabIndex = 19
        Button4.Text = "Save"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(ListBox1)
        GroupBox1.Controls.Add(Panel3)
        GroupBox1.Dock = DockStyle.Fill
        GroupBox1.Location = New Point(510, 0)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(1672, 410)
        GroupBox1.TabIndex = 20
        GroupBox1.TabStop = False
        GroupBox1.Text = "Synonym Names"
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(ComboBox1)
        Panel3.Controls.Add(Label4)
        Panel3.Dock = DockStyle.Top
        Panel3.Location = New Point(3, 19)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(1666, 36)
        Panel3.TabIndex = 28
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(Button8)
        GroupBox2.Controls.Add(TextBox6)
        GroupBox2.Controls.Add(ComboBox3)
        GroupBox2.Controls.Add(Button5)
        GroupBox2.Controls.Add(DataGridView1)
        GroupBox2.Dock = DockStyle.Left
        GroupBox2.Location = New Point(0, 0)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(510, 410)
        GroupBox2.TabIndex = 21
        GroupBox2.TabStop = False
        GroupBox2.Text = "Database CrossReference"
        ' 
        ' Button8
        ' 
        Button8.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Button8.Location = New Point(338, 378)
        Button8.Name = "Button8"
        Button8.Size = New Size(73, 23)
        Button8.TabIndex = 24
        Button8.Text = "Add"
        Button8.UseVisualStyleBackColor = True
        ' 
        ' TextBox6
        ' 
        TextBox6.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        TextBox6.Location = New Point(114, 379)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(218, 23)
        TextBox6.TabIndex = 23
        ' 
        ' ComboBox3
        ' 
        ComboBox3.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.FormattingEnabled = True
        ComboBox3.Location = New Point(15, 379)
        ComboBox3.Name = "ComboBox3"
        ComboBox3.Size = New Size(93, 23)
        ComboBox3.TabIndex = 21
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(422, 20)
        Button5.Name = "Button5"
        Button5.Size = New Size(75, 23)
        Button5.TabIndex = 20
        Button5.Text = "Save"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(TextBox5)
        GroupBox3.Controls.Add(Label1)
        GroupBox3.Controls.Add(WebView21)
        GroupBox3.Controls.Add(Label2)
        GroupBox3.Controls.Add(TextBox1)
        GroupBox3.Controls.Add(Button1)
        GroupBox3.Location = New Point(521, 9)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(552, 527)
        GroupBox3.TabIndex = 22
        GroupBox3.TabStop = False
        GroupBox3.Text = "Molecular Structure Data"
        ' 
        ' TextBox5
        ' 
        TextBox5.Location = New Point(6, 454)
        TextBox5.Multiline = True
        TextBox5.Name = "TextBox5"
        TextBox5.ReadOnly = True
        TextBox5.Size = New Size(522, 60)
        TextBox5.TabIndex = 7
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(4, 439)
        Label1.Name = "Label1"
        Label1.Size = New Size(73, 15)
        Label1.TabIndex = 6
        Label1.Text = "Fingerprints:"
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Controls.Add(Button7)
        GroupBox4.Controls.Add(TextBox4)
        GroupBox4.Controls.Add(Button2)
        GroupBox4.Controls.Add(Label5)
        GroupBox4.Controls.Add(TextBox2)
        GroupBox4.Controls.Add(Button4)
        GroupBox4.Controls.Add(Label6)
        GroupBox4.Controls.Add(Button3)
        GroupBox4.Controls.Add(TextBox3)
        GroupBox4.Controls.Add(Label7)
        GroupBox4.Controls.Add(Label8)
        GroupBox4.Location = New Point(5, 9)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(510, 527)
        GroupBox4.TabIndex = 23
        GroupBox4.TabStop = False
        GroupBox4.Text = "Molecular Information"
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(422, 170)
        Button7.Name = "Button7"
        Button7.Size = New Size(75, 23)
        Button7.TabIndex = 20
        Button7.Text = "LLMs Talk"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' GroupBox5
        ' 
        GroupBox5.Controls.Add(ComboBox2)
        GroupBox5.Controls.Add(Button6)
        GroupBox5.Controls.Add(ListBox2)
        GroupBox5.Location = New Point(1079, 9)
        GroupBox5.Name = "GroupBox5"
        GroupBox5.Size = New Size(367, 521)
        GroupBox5.TabIndex = 26
        GroupBox5.TabStop = False
        GroupBox5.Text = "Topic Tags"
        ' 
        ' ComboBox2
        ' 
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(6, 485)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(274, 23)
        ComboBox2.TabIndex = 2
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(286, 483)
        Button6.Name = "Button6"
        Button6.Size = New Size(75, 23)
        Button6.TabIndex = 1
        Button6.Text = "Add"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' ListBox2
        ' 
        ListBox2.ContextMenuStrip = ContextMenuStrip2
        ListBox2.FormattingEnabled = True
        ListBox2.ItemHeight = 15
        ListBox2.Location = New Point(6, 20)
        ListBox2.Name = "ListBox2"
        ListBox2.Size = New Size(355, 454)
        ListBox2.TabIndex = 0
        ' 
        ' ContextMenuStrip2
        ' 
        ContextMenuStrip2.Items.AddRange(New ToolStripItem() {ClearThisTagToolStripMenuItem})
        ContextMenuStrip2.Name = "ContextMenuStrip2"
        ContextMenuStrip2.Size = New Size(147, 26)
        ' 
        ' ClearThisTagToolStripMenuItem
        ' 
        ClearThisTagToolStripMenuItem.Name = "ClearThisTagToolStripMenuItem"
        ClearThisTagToolStripMenuItem.Size = New Size(146, 22)
        ClearThisTagToolStripMenuItem.Text = "Clear This Tag"
        ' 
        ' GroupBox6
        ' 
        GroupBox6.Controls.Add(ListBox3)
        GroupBox6.Location = New Point(1452, 9)
        GroupBox6.Name = "GroupBox6"
        GroupBox6.Size = New Size(367, 521)
        GroupBox6.TabIndex = 27
        GroupBox6.TabStop = False
        GroupBox6.Text = "Organism Source"
        ' 
        ' ListBox3
        ' 
        ListBox3.ContextMenuStrip = ContextMenuStrip4
        ListBox3.Dock = DockStyle.Fill
        ListBox3.FormattingEnabled = True
        ListBox3.ItemHeight = 15
        ListBox3.Location = New Point(3, 19)
        ListBox3.Name = "ListBox3"
        ListBox3.Size = New Size(361, 499)
        ListBox3.TabIndex = 0
        ' 
        ' ContextMenuStrip4
        ' 
        ContextMenuStrip4.Items.AddRange(New ToolStripItem() {ListSourceToolStripMenuItem})
        ContextMenuStrip4.Name = "ContextMenuStrip4"
        ContextMenuStrip4.Size = New Size(132, 26)
        ' 
        ' ListSourceToolStripMenuItem
        ' 
        ListSourceToolStripMenuItem.Name = "ListSourceToolStripMenuItem"
        ListSourceToolStripMenuItem.Size = New Size(131, 22)
        ListSourceToolStripMenuItem.Text = "List Source"
        ' 
        ' DataGridView2
        ' 
        DataGridView2.AllowUserToAddRows = False
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView2.Columns.AddRange(New DataGridViewColumn() {Column3, Column4})
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = SystemColors.Window
        DataGridViewCellStyle1.Font = New Font("Cambria", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.False
        DataGridView2.DefaultCellStyle = DataGridViewCellStyle1
        DataGridView2.Dock = DockStyle.Fill
        DataGridView2.Location = New Point(0, 0)
        DataGridView2.Name = "DataGridView2"
        DataGridView2.RowTemplate.Height = 23
        DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView2.Size = New Size(2182, 518)
        DataGridView2.TabIndex = 28
        ' 
        ' Column3
        ' 
        Column3.HeaderText = "name"
        Column3.Name = "Column3"
        Column3.ReadOnly = True
        Column3.Width = 300
        ' 
        ' Column4
        ' 
        Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column4.HeaderText = "equation"
        Column4.Name = "Column4"
        Column4.ReadOnly = True
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(3, 3)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(DataGridView2)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(DataGridView3)
        SplitContainer1.Panel2.Controls.Add(TextBox7)
        SplitContainer1.Size = New Size(2182, 949)
        SplitContainer1.SplitterDistance = 518
        SplitContainer1.TabIndex = 30
        ' 
        ' TextBox7
        ' 
        TextBox7.BorderStyle = BorderStyle.FixedSingle
        TextBox7.Dock = DockStyle.Top
        TextBox7.Location = New Point(0, 0)
        TextBox7.Multiline = True
        TextBox7.Name = "TextBox7"
        TextBox7.ReadOnly = True
        TextBox7.Size = New Size(2182, 90)
        TextBox7.TabIndex = 30
        ' 
        ' DataGridView3
        ' 
        DataGridView3.AllowUserToAddRows = False
        DataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView3.Columns.AddRange(New DataGridViewColumn() {Column6, Column11, Column7, Column8, Column9, Column10})
        DataGridView3.ContextMenuStrip = ContextMenuStrip5
        DataGridView3.Dock = DockStyle.Fill
        DataGridView3.Location = New Point(0, 90)
        DataGridView3.Name = "DataGridView3"
        DataGridView3.RowTemplate.Height = 23
        DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView3.Size = New Size(2182, 337)
        DataGridView3.TabIndex = 29
        ' 
        ' Column6
        ' 
        Column6.HeaderText = "registry_id"
        Column6.Name = "Column6"
        Column6.ReadOnly = True
        ' 
        ' Column11
        ' 
        Column11.HeaderText = "db_xref"
        Column11.Name = "Column11"
        Column11.ReadOnly = True
        ' 
        ' Column7
        ' 
        Column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column7.HeaderText = "name"
        Column7.Name = "Column7"
        Column7.ReadOnly = True
        ' 
        ' Column8
        ' 
        Column8.HeaderText = "formula"
        Column8.Name = "Column8"
        Column8.ReadOnly = True
        ' 
        ' Column9
        ' 
        Column9.HeaderText = "exact_mass"
        Column9.Name = "Column9"
        Column9.ReadOnly = True
        ' 
        ' Column10
        ' 
        Column10.HeaderText = "side"
        Column10.Name = "Column10"
        Column10.ReadOnly = True
        ' 
        ' ContextMenuStrip5
        ' 
        ContextMenuStrip5.Items.AddRange(New ToolStripItem() {OpenMoleculeDataToolStripMenuItem})
        ContextMenuStrip5.Name = "ContextMenuStrip5"
        ContextMenuStrip5.Size = New Size(183, 26)
        ' 
        ' OpenMoleculeDataToolStripMenuItem
        ' 
        OpenMoleculeDataToolStripMenuItem.Name = "OpenMoleculeDataToolStripMenuItem"
        OpenMoleculeDataToolStripMenuItem.Size = New Size(182, 22)
        OpenMoleculeDataToolStripMenuItem.Text = "Open Molecule Data"
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.Items.AddRange(New ToolStripItem() {ToolStripLabel1, LinkLabel1})
        ToolStrip1.Location = New Point(0, 0)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(2196, 25)
        ToolStrip1.TabIndex = 30
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' ToolStripLabel1
        ' 
        ToolStripLabel1.Name = "ToolStripLabel1"
        ToolStripLabel1.Size = New Size(83, 22)
        ToolStripLabel1.Text = "Visit Site Page:"
        ' 
        ' LinkLabel1
        ' 
        LinkLabel1.IsLink = True
        LinkLabel1.Name = "LinkLabel1"
        LinkLabel1.Size = New Size(340, 22)
        LinkLabel1.Text = "http://biocad.innovation.ac.cn/molecule/BioCAD00000000000/"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(GroupBox4)
        Panel1.Controls.Add(GroupBox3)
        Panel1.Controls.Add(GroupBox5)
        Panel1.Controls.Add(GroupBox6)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(3, 3)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(2182, 539)
        Panel1.TabIndex = 31
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(GroupBox1)
        Panel2.Controls.Add(GroupBox2)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 542)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(2182, 410)
        Panel2.TabIndex = 32
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(TabPage1)
        TabControl1.Controls.Add(TabPage2)
        TabControl1.Dock = DockStyle.Fill
        TabControl1.Location = New Point(0, 25)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(2196, 983)
        TabControl1.TabIndex = 33
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(Panel2)
        TabPage1.Controls.Add(Panel1)
        TabPage1.Location = New Point(4, 24)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(2188, 955)
        TabPage1.TabIndex = 0
        TabPage1.Text = "Molecule Information"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(SplitContainer1)
        TabPage2.Location = New Point(4, 24)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3)
        TabPage2.Size = New Size(2188, 955)
        TabPage2.TabIndex = 1
        TabPage2.Text = "Related Metabolic Network"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' FormMoleculeEditor
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(2196, 1008)
        Controls.Add(TabControl1)
        Controls.Add(ToolStrip1)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "FormMoleculeEditor"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        TabPageContextMenuStrip = DockContextMenuStrip1
        Text = "Molecule Editor"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip3.ResumeLayout(False)
        CType(WebView21, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip1.ResumeLayout(False)
        GroupBox1.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        GroupBox4.ResumeLayout(False)
        GroupBox4.PerformLayout()
        GroupBox5.ResumeLayout(False)
        ContextMenuStrip2.ResumeLayout(False)
        GroupBox6.ResumeLayout(False)
        ContextMenuStrip4.ResumeLayout(False)
        CType(DataGridView2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        SplitContainer1.Panel2.PerformLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        CType(DataGridView3, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip5.ResumeLayout(False)
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel2.ResumeLayout(False)
        TabControl1.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        TabPage2.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Button5 As Button
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SetAsDisplayNameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents ClearThisTagToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip3 As ContextMenuStrip
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Button6 As Button
    Friend WithEvents EditToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents Button7 As Button
    Friend WithEvents ChineseNameTranslationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Button8 As Button
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents ComboBox3 As ComboBox
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents ListBox3 As ListBox
    Friend WithEvents ContextMenuStrip4 As ContextMenuStrip
    Friend WithEvents ListSourceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchByThisDbXrefToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents DataGridView3 As DataGridView
    Friend WithEvents ContextMenuStrip5 As ContextMenuStrip
    Friend WithEvents OpenMoleculeDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column11 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents Column8 As DataGridViewTextBoxColumn
    Friend WithEvents Column9 As DataGridViewTextBoxColumn
    Friend WithEvents Column10 As DataGridViewTextBoxColumn
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents LinkLabel1 As ToolStripLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Panel3 As Panel
End Class
