using System;

namespace psu_generic_parser
{
    partial class SetFileViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExportJSONButton = new System.Windows.Forms.Button();
            this.ImportJSONButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.setObjectListBox = new System.Windows.Forms.ListBox();
            this.areaLabel = new System.Windows.Forms.Label();
            this.mapListCB = new System.Windows.Forms.ComboBox();
            this.mapListLabel = new System.Windows.Forms.Label();
            this.objectListCB = new System.Windows.Forms.ComboBox();
            this.objectSetLabel = new System.Windows.Forms.Label();
            this.positionGroupBox = new System.Windows.Forms.GroupBox();
            this.posZUD = new System.Windows.Forms.NumericUpDown();
            this.posYUD = new System.Windows.Forms.NumericUpDown();
            this.posXUD = new System.Windows.Forms.NumericUpDown();
            this.objIDLabel = new System.Windows.Forms.Label();
            this.rotationGroupBox = new System.Windows.Forms.GroupBox();
            this.rotZUD = new System.Windows.Forms.NumericUpDown();
            this.rotYUD = new System.Windows.Forms.NumericUpDown();
            this.rotXUD = new System.Windows.Forms.NumericUpDown();
            this.objIDUD = new System.Windows.Forms.NumericUpDown();
            this.unkIntLabel = new System.Windows.Forms.Label();
            this.unkIntUD = new System.Windows.Forms.NumericUpDown();
            this.editObjectSetHeaderBytesButton = new System.Windows.Forms.Button();
            this.addMapListButton = new System.Windows.Forms.Button();
            this.removeMapListButton = new System.Windows.Forms.Button();
            this.addObjectList = new System.Windows.Forms.Button();
            this.removeObjectList = new System.Windows.Forms.Button();
            this.addObjectButton = new System.Windows.Forms.Button();
            this.removeObjectButton = new System.Windows.Forms.Button();
            this.clearObjectsButton = new System.Windows.Forms.Button();
            this.duplicateObjectButton = new System.Windows.Forms.Button();
            this.mapListNumberLabel = new System.Windows.Forms.Label();
            this.mapListNumberUD = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.metadataGroupBox = new System.Windows.Forms.GroupBox();
            this.headerInt3UD = new System.Windows.Forms.NumericUpDown();
            this.headerInt2UD = new System.Windows.Forms.NumericUpDown();
            this.headerShort1UD = new System.Windows.Forms.NumericUpDown();
            this.headerInt1UD = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.areaIdComboBox = new System.Windows.Forms.ComboBox();
            this.objectNameLabel = new System.Windows.Forms.Label();
            this.positionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.posZUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posYUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posXUD)).BeginInit();
            this.rotationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rotZUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotYUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotXUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objIDUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unkIntUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapListNumberUD)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerInt3UD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerInt2UD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerShort1UD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerInt1UD)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExportJSONButton
            // 
            this.ExportJSONButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportJSONButton.Location = new System.Drawing.Point(425, 549);
            this.ExportJSONButton.Name = "ExportJSONButton";
            this.ExportJSONButton.Size = new System.Drawing.Size(83, 23);
            this.ExportJSONButton.TabIndex = 0;
            this.ExportJSONButton.Text = "Export JSON";
            this.ExportJSONButton.UseVisualStyleBackColor = true;
            this.ExportJSONButton.Click += new System.EventHandler(this.ExportJSON_Click);
            // 
            // ImportJSONButton
            // 
            this.ImportJSONButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportJSONButton.Location = new System.Drawing.Point(336, 549);
            this.ImportJSONButton.Name = "ImportJSONButton";
            this.ImportJSONButton.Size = new System.Drawing.Size(83, 23);
            this.ImportJSONButton.TabIndex = 1;
            this.ImportJSONButton.Text = "Import JSON";
            this.ImportJSONButton.UseVisualStyleBackColor = true;
            this.ImportJSONButton.Click += new System.EventHandler(this.ImportJSON_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // setObjectListBox
            // 
            this.setObjectListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setObjectListBox.FormattingEnabled = true;
            this.setObjectListBox.Location = new System.Drawing.Point(8, 49);
            this.setObjectListBox.Name = "setObjectListBox";
            this.setObjectListBox.ScrollAlwaysVisible = true;
            this.setObjectListBox.Size = new System.Drawing.Size(155, 316);
            this.setObjectListBox.TabIndex = 2;
            this.setObjectListBox.SelectedIndexChanged += new System.EventHandler(this.setObjectListBox_SelectedIndexChanged);
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Location = new System.Drawing.Point(3, 0);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(74, 13);
            this.areaLabel.TabIndex = 3;
            this.areaLabel.Text = "Zone Area ID:";
            // 
            // mapListCB
            // 
            this.mapListCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapListCB.FormattingEnabled = true;
            this.mapListCB.Location = new System.Drawing.Point(118, 15);
            this.mapListCB.Name = "mapListCB";
            this.mapListCB.Size = new System.Drawing.Size(73, 21);
            this.mapListCB.TabIndex = 5;
            this.mapListCB.SelectedIndexChanged += new System.EventHandler(this.mapListCB_SelectedIndexChanged);
            // 
            // mapListLabel
            // 
            this.mapListLabel.AutoSize = true;
            this.mapListLabel.Location = new System.Drawing.Point(122, 0);
            this.mapListLabel.Name = "mapListLabel";
            this.mapListLabel.Size = new System.Drawing.Size(68, 13);
            this.mapListLabel.TabIndex = 6;
            this.mapListLabel.Text = "Current Map:";
            // 
            // objectListCB
            // 
            this.objectListCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectListCB.FormattingEnabled = true;
            this.objectListCB.Location = new System.Drawing.Point(287, 15);
            this.objectListCB.Name = "objectListCB";
            this.objectListCB.Size = new System.Drawing.Size(73, 21);
            this.objectListCB.TabIndex = 7;
            this.objectListCB.SelectedIndexChanged += new System.EventHandler(this.objectListCB_SelectedIndexChanged);
            // 
            // objectSetLabel
            // 
            this.objectSetLabel.AutoSize = true;
            this.objectSetLabel.Location = new System.Drawing.Point(284, -1);
            this.objectSetLabel.Name = "objectSetLabel";
            this.objectSetLabel.Size = new System.Drawing.Size(97, 13);
            this.objectSetLabel.TabIndex = 8;
            this.objectSetLabel.Text = "Current Object List:";
            // 
            // positionGroupBox
            // 
            this.positionGroupBox.Controls.Add(this.posZUD);
            this.positionGroupBox.Controls.Add(this.posYUD);
            this.positionGroupBox.Controls.Add(this.posXUD);
            this.positionGroupBox.Location = new System.Drawing.Point(3, 55);
            this.positionGroupBox.Name = "positionGroupBox";
            this.positionGroupBox.Size = new System.Drawing.Size(311, 47);
            this.positionGroupBox.TabIndex = 9;
            this.positionGroupBox.TabStop = false;
            this.positionGroupBox.Text = "Position (X, Y, Z)";
            // 
            // posZUD
            // 
            this.posZUD.DecimalPlaces = 8;
            this.posZUD.Location = new System.Drawing.Point(211, 17);
            this.posZUD.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.posZUD.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.posZUD.Name = "posZUD";
            this.posZUD.Size = new System.Drawing.Size(90, 20);
            this.posZUD.TabIndex = 2;
            this.posZUD.ValueChanged += new System.EventHandler(this.posZUD_ValueChanged);
            // 
            // posYUD
            // 
            this.posYUD.DecimalPlaces = 8;
            this.posYUD.Location = new System.Drawing.Point(109, 17);
            this.posYUD.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.posYUD.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.posYUD.Name = "posYUD";
            this.posYUD.Size = new System.Drawing.Size(90, 20);
            this.posYUD.TabIndex = 1;
            this.posYUD.ValueChanged += new System.EventHandler(this.posYUD_ValueChanged);
            // 
            // posXUD
            // 
            this.posXUD.DecimalPlaces = 8;
            this.posXUD.Location = new System.Drawing.Point(7, 17);
            this.posXUD.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.posXUD.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.posXUD.Name = "posXUD";
            this.posXUD.Size = new System.Drawing.Size(90, 20);
            this.posXUD.TabIndex = 0;
            this.posXUD.ValueChanged += new System.EventHandler(this.posXUD_ValueChanged);
            // 
            // objIDLabel
            // 
            this.objIDLabel.AutoSize = true;
            this.objIDLabel.Location = new System.Drawing.Point(7, 16);
            this.objIDLabel.Name = "objIDLabel";
            this.objIDLabel.Size = new System.Drawing.Size(52, 13);
            this.objIDLabel.TabIndex = 10;
            this.objIDLabel.Text = "Object ID";
            // 
            // rotationGroupBox
            // 
            this.rotationGroupBox.Controls.Add(this.rotZUD);
            this.rotationGroupBox.Controls.Add(this.rotYUD);
            this.rotationGroupBox.Controls.Add(this.rotXUD);
            this.rotationGroupBox.Location = new System.Drawing.Point(3, 106);
            this.rotationGroupBox.Name = "rotationGroupBox";
            this.rotationGroupBox.Size = new System.Drawing.Size(311, 47);
            this.rotationGroupBox.TabIndex = 11;
            this.rotationGroupBox.TabStop = false;
            this.rotationGroupBox.Text = "Rotation (X, Y, Z)";
            // 
            // rotZUD
            // 
            this.rotZUD.DecimalPlaces = 8;
            this.rotZUD.Location = new System.Drawing.Point(211, 17);
            this.rotZUD.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.rotZUD.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.rotZUD.Name = "rotZUD";
            this.rotZUD.Size = new System.Drawing.Size(90, 20);
            this.rotZUD.TabIndex = 2;
            this.rotZUD.ValueChanged += new System.EventHandler(this.rotZUD_ValueChanged);
            // 
            // rotYUD
            // 
            this.rotYUD.DecimalPlaces = 8;
            this.rotYUD.Location = new System.Drawing.Point(109, 17);
            this.rotYUD.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.rotYUD.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.rotYUD.Name = "rotYUD";
            this.rotYUD.Size = new System.Drawing.Size(90, 20);
            this.rotYUD.TabIndex = 1;
            this.rotYUD.ValueChanged += new System.EventHandler(this.rotYUD_ValueChanged);
            // 
            // rotXUD
            // 
            this.rotXUD.DecimalPlaces = 8;
            this.rotXUD.Location = new System.Drawing.Point(7, 17);
            this.rotXUD.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.rotXUD.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.rotXUD.Name = "rotXUD";
            this.rotXUD.Size = new System.Drawing.Size(90, 20);
            this.rotXUD.TabIndex = 0;
            this.rotXUD.ValueChanged += new System.EventHandler(this.rotXUD_ValueChanged);
            // 
            // objIDUD
            // 
            this.objIDUD.Location = new System.Drawing.Point(10, 32);
            this.objIDUD.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.objIDUD.Name = "objIDUD";
            this.objIDUD.Size = new System.Drawing.Size(61, 20);
            this.objIDUD.TabIndex = 12;
            this.objIDUD.ValueChanged += new System.EventHandler(this.objIDUD_ValueChanged);
            // 
            // unkIntLabel
            // 
            this.unkIntLabel.AutoSize = true;
            this.unkIntLabel.Location = new System.Drawing.Point(79, 16);
            this.unkIntLabel.Name = "unkIntLabel";
            this.unkIntLabel.Size = new System.Drawing.Size(42, 13);
            this.unkIntLabel.TabIndex = 13;
            this.unkIntLabel.Text = "Unk Int";
            // 
            // unkIntUD
            // 
            this.unkIntUD.Location = new System.Drawing.Point(82, 32);
            this.unkIntUD.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.unkIntUD.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.unkIntUD.Name = "unkIntUD";
            this.unkIntUD.Size = new System.Drawing.Size(61, 20);
            this.unkIntUD.TabIndex = 14;
            this.unkIntUD.ValueChanged += new System.EventHandler(this.unkIntUD_ValueChanged);
            // 
            // editObjectSetHeaderBytesButton
            // 
            this.editObjectSetHeaderBytesButton.Location = new System.Drawing.Point(367, 13);
            this.editObjectSetHeaderBytesButton.Name = "editObjectSetHeaderBytesButton";
            this.editObjectSetHeaderBytesButton.Size = new System.Drawing.Size(118, 23);
            this.editObjectSetHeaderBytesButton.TabIndex = 17;
            this.editObjectSetHeaderBytesButton.Text = "View List Header";
            this.editObjectSetHeaderBytesButton.UseVisualStyleBackColor = true;
            this.editObjectSetHeaderBytesButton.Click += new System.EventHandler(this.editObjectSetHeaderBytesButton_Click);
            // 
            // addMapListButton
            // 
            this.addMapListButton.Location = new System.Drawing.Point(118, 42);
            this.addMapListButton.Name = "addMapListButton";
            this.addMapListButton.Size = new System.Drawing.Size(73, 21);
            this.addMapListButton.TabIndex = 18;
            this.addMapListButton.Text = "Add Map";
            this.addMapListButton.UseVisualStyleBackColor = true;
            this.addMapListButton.Click += new System.EventHandler(this.addMapListButton_Click);
            // 
            // removeMapListButton
            // 
            this.removeMapListButton.Location = new System.Drawing.Point(191, 42);
            this.removeMapListButton.Name = "removeMapListButton";
            this.removeMapListButton.Size = new System.Drawing.Size(90, 21);
            this.removeMapListButton.TabIndex = 19;
            this.removeMapListButton.Text = "Remove Map";
            this.removeMapListButton.UseVisualStyleBackColor = true;
            this.removeMapListButton.Click += new System.EventHandler(this.removeMapListButton_Click);
            // 
            // addObjectList
            // 
            this.addObjectList.Location = new System.Drawing.Point(287, 42);
            this.addObjectList.Name = "addObjectList";
            this.addObjectList.Size = new System.Drawing.Size(74, 21);
            this.addObjectList.TabIndex = 20;
            this.addObjectList.Text = "Add List";
            this.addObjectList.UseVisualStyleBackColor = true;
            this.addObjectList.Click += new System.EventHandler(this.addObjectList_Click);
            // 
            // removeObjectList
            // 
            this.removeObjectList.Location = new System.Drawing.Point(367, 42);
            this.removeObjectList.Name = "removeObjectList";
            this.removeObjectList.Size = new System.Drawing.Size(74, 21);
            this.removeObjectList.TabIndex = 21;
            this.removeObjectList.Text = "Remove List";
            this.removeObjectList.UseVisualStyleBackColor = true;
            this.removeObjectList.Click += new System.EventHandler(this.removeObjectList_Click);
            // 
            // addObjectButton
            // 
            this.addObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addObjectButton.Location = new System.Drawing.Point(8, 381);
            this.addObjectButton.Name = "addObjectButton";
            this.addObjectButton.Size = new System.Drawing.Size(75, 23);
            this.addObjectButton.TabIndex = 22;
            this.addObjectButton.Text = "Add";
            this.addObjectButton.UseVisualStyleBackColor = true;
            this.addObjectButton.Click += new System.EventHandler(this.addObjectButton_Click);
            // 
            // removeObjectButton
            // 
            this.removeObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeObjectButton.Location = new System.Drawing.Point(8, 403);
            this.removeObjectButton.Name = "removeObjectButton";
            this.removeObjectButton.Size = new System.Drawing.Size(75, 23);
            this.removeObjectButton.TabIndex = 23;
            this.removeObjectButton.Text = "Remove";
            this.removeObjectButton.UseVisualStyleBackColor = true;
            this.removeObjectButton.Click += new System.EventHandler(this.removeObjectButton_Click);
            // 
            // clearObjectsButton
            // 
            this.clearObjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearObjectsButton.Location = new System.Drawing.Point(8, 425);
            this.clearObjectsButton.Name = "clearObjectsButton";
            this.clearObjectsButton.Size = new System.Drawing.Size(75, 23);
            this.clearObjectsButton.TabIndex = 24;
            this.clearObjectsButton.Text = "Clear";
            this.clearObjectsButton.UseVisualStyleBackColor = true;
            this.clearObjectsButton.Click += new System.EventHandler(this.clearObjectsButton_Click);
            // 
            // duplicateObjectButton
            // 
            this.duplicateObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duplicateObjectButton.Location = new System.Drawing.Point(85, 381);
            this.duplicateObjectButton.Name = "duplicateObjectButton";
            this.duplicateObjectButton.Size = new System.Drawing.Size(75, 23);
            this.duplicateObjectButton.TabIndex = 25;
            this.duplicateObjectButton.Text = "Duplicate";
            this.duplicateObjectButton.UseVisualStyleBackColor = true;
            this.duplicateObjectButton.Click += new System.EventHandler(this.duplicateObjectButton_Click);
            // 
            // mapListNumberLabel
            // 
            this.mapListNumberLabel.AutoSize = true;
            this.mapListNumberLabel.Location = new System.Drawing.Point(8, 8);
            this.mapListNumberLabel.Name = "mapListNumberLabel";
            this.mapListNumberLabel.Size = new System.Drawing.Size(38, 13);
            this.mapListNumberLabel.TabIndex = 26;
            this.mapListNumberLabel.Text = "Map #";
            // 
            // mapListNumberUD
            // 
            this.mapListNumberUD.Location = new System.Drawing.Point(11, 23);
            this.mapListNumberUD.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.mapListNumberUD.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.mapListNumberUD.Name = "mapListNumberUD";
            this.mapListNumberUD.Size = new System.Drawing.Size(54, 20);
            this.mapListNumberUD.TabIndex = 27;
            this.mapListNumberUD.ValueChanged += new System.EventHandler(this.mapListNumberUD_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.objectNameLabel);
            this.groupBox1.Controls.Add(this.metadataGroupBox);
            this.groupBox1.Controls.Add(this.headerInt3UD);
            this.groupBox1.Controls.Add(this.headerInt2UD);
            this.groupBox1.Controls.Add(this.headerShort1UD);
            this.groupBox1.Controls.Add(this.headerInt1UD);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.objIDLabel);
            this.groupBox1.Controls.Add(this.positionGroupBox);
            this.groupBox1.Controls.Add(this.rotationGroupBox);
            this.groupBox1.Controls.Add(this.objIDUD);
            this.groupBox1.Controls.Add(this.unkIntLabel);
            this.groupBox1.Controls.Add(this.unkIntUD);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 451);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Object";
            // 
            // metadataGroupBox
            // 
            this.metadataGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metadataGroupBox.Location = new System.Drawing.Point(6, 229);
            this.metadataGroupBox.Name = "metadataGroupBox";
            this.metadataGroupBox.Size = new System.Drawing.Size(312, 216);
            this.metadataGroupBox.TabIndex = 39;
            this.metadataGroupBox.TabStop = false;
            this.metadataGroupBox.Text = "Metadata";
            // 
            // headerInt3UD
            // 
            this.headerInt3UD.Location = new System.Drawing.Point(214, 177);
            this.headerInt3UD.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.headerInt3UD.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.headerInt3UD.Name = "headerInt3UD";
            this.headerInt3UD.Size = new System.Drawing.Size(90, 20);
            this.headerInt3UD.TabIndex = 38;
            // 
            // headerInt2UD
            // 
            this.headerInt2UD.Location = new System.Drawing.Point(112, 177);
            this.headerInt2UD.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.headerInt2UD.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.headerInt2UD.Name = "headerInt2UD";
            this.headerInt2UD.Size = new System.Drawing.Size(90, 20);
            this.headerInt2UD.TabIndex = 37;
            // 
            // headerShort1UD
            // 
            this.headerShort1UD.Location = new System.Drawing.Point(9, 203);
            this.headerShort1UD.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.headerShort1UD.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.headerShort1UD.Name = "headerShort1UD";
            this.headerShort1UD.Size = new System.Drawing.Size(62, 20);
            this.headerShort1UD.TabIndex = 36;
            // 
            // headerInt1UD
            // 
            this.headerInt1UD.Location = new System.Drawing.Point(10, 177);
            this.headerInt1UD.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.headerInt1UD.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.headerInt1UD.Name = "headerInt1UD";
            this.headerInt1UD.Size = new System.Drawing.Size(90, 20);
            this.headerInt1UD.TabIndex = 33;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Header Mystery Data:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.splitContainer1);
            this.groupBox2.Location = new System.Drawing.Point(6, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(502, 470);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current Map";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.setObjectListBox);
            this.splitContainer1.Panel1.Controls.Add(this.mapListNumberLabel);
            this.splitContainer1.Panel1.Controls.Add(this.duplicateObjectButton);
            this.splitContainer1.Panel1.Controls.Add(this.mapListNumberUD);
            this.splitContainer1.Panel1.Controls.Add(this.clearObjectsButton);
            this.splitContainer1.Panel1.Controls.Add(this.removeObjectButton);
            this.splitContainer1.Panel1.Controls.Add(this.addObjectButton);
            this.splitContainer1.Panel1MinSize = 168;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.AutoScrollMinSize = new System.Drawing.Size(0, 451);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(496, 451);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 0;
            // 
            // areaIdComboBox
            // 
            this.areaIdComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.areaIdComboBox.FormattingEnabled = true;
            this.areaIdComboBox.Items.AddRange(new object[] {
            "00: 99 / Empty",
            "01: 00 / Test maps",
            "02: cg / Colony",
            "03: cx / Parum",
            "04: cy / Neudaiz",
            "05: cz / Moatoob",
            "06: sn / Linear Line",
            "07: sp / Endrum/Il Cabo",
            "08: ss / Lobby areas (combat)",
            "09: sb / HIVE",
            "0A: mf / Raffon Lakeshore",
            "0B: sf / Raffon Meadow",
            "0C: sc / Train/PSO areas",
            "0D: mc / Habirao",
            "0E: sl / Rozenom",
            "0F: sk / Neudaiz forest",
            "10: si / Neudaiz islands",
            "11: st / Temple",
            "12: sm / Moatoob caves",
            "13: sv / Canyon",
            "14: sd / Desert",
            "15: sr / Relics",
            "16: cm / My room",
            "17: bs / Boss arenas"});
            this.areaIdComboBox.Location = new System.Drawing.Point(6, 15);
            this.areaIdComboBox.Name = "areaIdComboBox";
            this.areaIdComboBox.Size = new System.Drawing.Size(106, 21);
            this.areaIdComboBox.TabIndex = 32;
            this.areaIdComboBox.SelectedIndexChanged += new System.EventHandler(this.areaIdComboBox_SelectedIndexChanged);
            // 
            // objectNameLabel
            // 
            this.objectNameLabel.AutoSize = true;
            this.objectNameLabel.Location = new System.Drawing.Point(152, 34);
            this.objectNameLabel.Name = "objectNameLabel";
            this.objectNameLabel.Size = new System.Drawing.Size(72, 13);
            this.objectNameLabel.TabIndex = 40;
            this.objectNameLabel.Text = "current object";
            // 
            // SetFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.areaIdComboBox);
            this.Controls.Add(this.removeMapListButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.removeObjectList);
            this.Controls.Add(this.addObjectList);
            this.Controls.Add(this.addMapListButton);
            this.Controls.Add(this.editObjectSetHeaderBytesButton);
            this.Controls.Add(this.objectSetLabel);
            this.Controls.Add(this.objectListCB);
            this.Controls.Add(this.mapListLabel);
            this.Controls.Add(this.mapListCB);
            this.Controls.Add(this.areaLabel);
            this.Controls.Add(this.ImportJSONButton);
            this.Controls.Add(this.ExportJSONButton);
            this.Name = "SetFileViewer";
            this.Size = new System.Drawing.Size(518, 575);
            this.positionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.posZUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posYUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posXUD)).EndInit();
            this.rotationGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rotZUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotYUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotXUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objIDUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unkIntUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapListNumberUD)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerInt3UD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerInt2UD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerShort1UD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerInt1UD)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExportJSONButton;
        private System.Windows.Forms.Button ImportJSONButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox setObjectListBox;
        private System.Windows.Forms.Label areaLabel;
        private System.Windows.Forms.ComboBox mapListCB;
        private System.Windows.Forms.Label mapListLabel;
        private System.Windows.Forms.ComboBox objectListCB;
        private System.Windows.Forms.Label objectSetLabel;
        private System.Windows.Forms.GroupBox positionGroupBox;
        private System.Windows.Forms.NumericUpDown posZUD;
        private System.Windows.Forms.NumericUpDown posYUD;
        private System.Windows.Forms.NumericUpDown posXUD;
        private System.Windows.Forms.Label objIDLabel;
        private System.Windows.Forms.GroupBox rotationGroupBox;
        private System.Windows.Forms.NumericUpDown rotZUD;
        private System.Windows.Forms.NumericUpDown rotYUD;
        private System.Windows.Forms.NumericUpDown rotXUD;
        private System.Windows.Forms.NumericUpDown objIDUD;
        private System.Windows.Forms.Label unkIntLabel;
        private System.Windows.Forms.NumericUpDown unkIntUD;
        private System.Windows.Forms.Button editObjectSetHeaderBytesButton;
        private System.Windows.Forms.Button addMapListButton;
        private System.Windows.Forms.Button removeMapListButton;
        private System.Windows.Forms.Button addObjectList;
        private System.Windows.Forms.Button removeObjectList;
        private System.Windows.Forms.Button addObjectButton;
        private System.Windows.Forms.Button removeObjectButton;
        private System.Windows.Forms.Button clearObjectsButton;
        private System.Windows.Forms.Button duplicateObjectButton;
        private System.Windows.Forms.Label mapListNumberLabel;
        private System.Windows.Forms.NumericUpDown mapListNumberUD;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox areaIdComboBox;
        private System.Windows.Forms.NumericUpDown headerShort1UD;
        private System.Windows.Forms.NumericUpDown headerInt1UD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown headerInt3UD;
        private System.Windows.Forms.NumericUpDown headerInt2UD;
        private System.Windows.Forms.GroupBox metadataGroupBox;
        private System.Windows.Forms.Label objectNameLabel;
    }
}
