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
            this.areaValueNUD = new System.Windows.Forms.NumericUpDown();
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
            this.editStartBytesButton = new System.Windows.Forms.Button();
            this.editObjectMetaDataButton = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.areaValueNUD)).BeginInit();
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
            this.SuspendLayout();
            // 
            // ExportJSONButton
            // 
            this.ExportJSONButton.Location = new System.Drawing.Point(410, 298);
            this.ExportJSONButton.Name = "ExportJSONButton";
            this.ExportJSONButton.Size = new System.Drawing.Size(83, 23);
            this.ExportJSONButton.TabIndex = 0;
            this.ExportJSONButton.Text = "Export JSON";
            this.ExportJSONButton.UseVisualStyleBackColor = true;
            this.ExportJSONButton.Click += new System.EventHandler(this.ExportJSON_Click);
            // 
            // ImportJSONButton
            // 
            this.ImportJSONButton.Location = new System.Drawing.Point(410, 269);
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
            this.setObjectListBox.FormattingEnabled = true;
            this.setObjectListBox.Location = new System.Drawing.Point(4, 37);
            this.setObjectListBox.Name = "setObjectListBox";
            this.setObjectListBox.ScrollAlwaysVisible = true;
            this.setObjectListBox.Size = new System.Drawing.Size(172, 212);
            this.setObjectListBox.TabIndex = 2;
            this.setObjectListBox.SelectedIndexChanged += new System.EventHandler(this.setObjectListBox_SelectedIndexChanged);
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Location = new System.Drawing.Point(3, 0);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(35, 13);
            this.areaLabel.TabIndex = 3;
            this.areaLabel.Text = "Area: ";
            // 
            // areaValueNUD
            // 
            this.areaValueNUD.Location = new System.Drawing.Point(6, 16);
            this.areaValueNUD.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.areaValueNUD.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.areaValueNUD.Name = "areaValueNUD";
            this.areaValueNUD.Size = new System.Drawing.Size(60, 20);
            this.areaValueNUD.TabIndex = 4;
            this.areaValueNUD.ValueChanged += new System.EventHandler(this.areaValueNUD_ValueChanged);
            // 
            // mapListCB
            // 
            this.mapListCB.FormattingEnabled = true;
            this.mapListCB.Location = new System.Drawing.Point(182, 16);
            this.mapListCB.Name = "mapListCB";
            this.mapListCB.Size = new System.Drawing.Size(73, 21);
            this.mapListCB.TabIndex = 5;
            this.mapListCB.SelectedIndexChanged += new System.EventHandler(this.mapListCB_SelectedIndexChanged);
            // 
            // mapListLabel
            // 
            this.mapListLabel.AutoSize = true;
            this.mapListLabel.Location = new System.Drawing.Point(196, 1);
            this.mapListLabel.Name = "mapListLabel";
            this.mapListLabel.Size = new System.Drawing.Size(47, 13);
            this.mapListLabel.TabIndex = 6;
            this.mapListLabel.Text = "Map List";
            // 
            // objectListCB
            // 
            this.objectListCB.FormattingEnabled = true;
            this.objectListCB.Location = new System.Drawing.Point(343, 16);
            this.objectListCB.Name = "objectListCB";
            this.objectListCB.Size = new System.Drawing.Size(73, 21);
            this.objectListCB.TabIndex = 7;
            this.objectListCB.SelectedIndexChanged += new System.EventHandler(this.objectListCB_SelectedIndexChanged);
            // 
            // objectSetLabel
            // 
            this.objectSetLabel.AutoSize = true;
            this.objectSetLabel.Location = new System.Drawing.Point(352, 1);
            this.objectSetLabel.Name = "objectSetLabel";
            this.objectSetLabel.Size = new System.Drawing.Size(57, 13);
            this.objectSetLabel.TabIndex = 8;
            this.objectSetLabel.Text = "Object List";
            // 
            // positionGroupBox
            // 
            this.positionGroupBox.Controls.Add(this.posZUD);
            this.positionGroupBox.Controls.Add(this.posYUD);
            this.positionGroupBox.Controls.Add(this.posXUD);
            this.positionGroupBox.Location = new System.Drawing.Point(182, 123);
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
            this.objIDLabel.Location = new System.Drawing.Point(186, 84);
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
            this.rotationGroupBox.Location = new System.Drawing.Point(182, 174);
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
            this.objIDUD.Location = new System.Drawing.Point(189, 100);
            this.objIDUD.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.objIDUD.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.objIDUD.Name = "objIDUD";
            this.objIDUD.Size = new System.Drawing.Size(61, 20);
            this.objIDUD.TabIndex = 12;
            this.objIDUD.ValueChanged += new System.EventHandler(this.objIDUD_ValueChanged);
            // 
            // unkIntLabel
            // 
            this.unkIntLabel.AutoSize = true;
            this.unkIntLabel.Location = new System.Drawing.Point(258, 84);
            this.unkIntLabel.Name = "unkIntLabel";
            this.unkIntLabel.Size = new System.Drawing.Size(42, 13);
            this.unkIntLabel.TabIndex = 13;
            this.unkIntLabel.Text = "Unk Int";
            // 
            // unkIntUD
            // 
            this.unkIntUD.Location = new System.Drawing.Point(261, 100);
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
            // editStartBytesButton
            // 
            this.editStartBytesButton.Location = new System.Drawing.Point(199, 227);
            this.editStartBytesButton.Name = "editStartBytesButton";
            this.editStartBytesButton.Size = new System.Drawing.Size(128, 23);
            this.editStartBytesButton.TabIndex = 15;
            this.editStartBytesButton.Text = "View Object Start Bytes";
            this.editStartBytesButton.UseVisualStyleBackColor = true;
            this.editStartBytesButton.Click += new System.EventHandler(this.editStartBytesButton_Click);
            // 
            // editObjectMetaDataButton
            // 
            this.editObjectMetaDataButton.Location = new System.Drawing.Point(344, 227);
            this.editObjectMetaDataButton.Name = "editObjectMetaDataButton";
            this.editObjectMetaDataButton.Size = new System.Drawing.Size(128, 23);
            this.editObjectMetaDataButton.TabIndex = 16;
            this.editObjectMetaDataButton.Text = "View Object Meta Data";
            this.editObjectMetaDataButton.UseVisualStyleBackColor = true;
            this.editObjectMetaDataButton.Click += new System.EventHandler(this.editObjectMetaDataButton_Click);
            // 
            // editObjectSetHeaderBytesButton
            // 
            this.editObjectSetHeaderBytesButton.Location = new System.Drawing.Point(182, 43);
            this.editObjectSetHeaderBytesButton.Name = "editObjectSetHeaderBytesButton";
            this.editObjectSetHeaderBytesButton.Size = new System.Drawing.Size(164, 23);
            this.editObjectSetHeaderBytesButton.TabIndex = 17;
            this.editObjectSetHeaderBytesButton.Text = "View Object List Header Bytes";
            this.editObjectSetHeaderBytesButton.UseVisualStyleBackColor = true;
            this.editObjectSetHeaderBytesButton.Click += new System.EventHandler(this.editObjectSetHeaderBytesButton_Click);
            // 
            // addMapListButton
            // 
            this.addMapListButton.Location = new System.Drawing.Point(257, 0);
            this.addMapListButton.Name = "addMapListButton";
            this.addMapListButton.Size = new System.Drawing.Size(84, 21);
            this.addMapListButton.TabIndex = 18;
            this.addMapListButton.Text = "Add List";
            this.addMapListButton.UseVisualStyleBackColor = true;
            this.addMapListButton.Click += new System.EventHandler(this.addMapListButton_Click);
            // 
            // removeMapListButton
            // 
            this.removeMapListButton.Location = new System.Drawing.Point(257, 20);
            this.removeMapListButton.Name = "removeMapListButton";
            this.removeMapListButton.Size = new System.Drawing.Size(84, 21);
            this.removeMapListButton.TabIndex = 19;
            this.removeMapListButton.Text = "Remove List";
            this.removeMapListButton.UseVisualStyleBackColor = true;
            this.removeMapListButton.Click += new System.EventHandler(this.removeMapListButton_Click);
            // 
            // addObjectList
            // 
            this.addObjectList.Location = new System.Drawing.Point(420, 0);
            this.addObjectList.Name = "addObjectList";
            this.addObjectList.Size = new System.Drawing.Size(74, 21);
            this.addObjectList.TabIndex = 20;
            this.addObjectList.Text = "Add List";
            this.addObjectList.UseVisualStyleBackColor = true;
            this.addObjectList.Click += new System.EventHandler(this.addObjectList_Click);
            // 
            // removeObjectList
            // 
            this.removeObjectList.Location = new System.Drawing.Point(420, 20);
            this.removeObjectList.Name = "removeObjectList";
            this.removeObjectList.Size = new System.Drawing.Size(74, 21);
            this.removeObjectList.TabIndex = 21;
            this.removeObjectList.Text = "Remove List";
            this.removeObjectList.UseVisualStyleBackColor = true;
            this.removeObjectList.Click += new System.EventHandler(this.removeObjectList_Click);
            // 
            // addObjectButton
            // 
            this.addObjectButton.Location = new System.Drawing.Point(4, 251);
            this.addObjectButton.Name = "addObjectButton";
            this.addObjectButton.Size = new System.Drawing.Size(75, 23);
            this.addObjectButton.TabIndex = 22;
            this.addObjectButton.Text = "Add";
            this.addObjectButton.UseVisualStyleBackColor = true;
            this.addObjectButton.Click += new System.EventHandler(this.addObjectButton_Click);
            // 
            // removeObjectButton
            // 
            this.removeObjectButton.Location = new System.Drawing.Point(4, 273);
            this.removeObjectButton.Name = "removeObjectButton";
            this.removeObjectButton.Size = new System.Drawing.Size(75, 23);
            this.removeObjectButton.TabIndex = 23;
            this.removeObjectButton.Text = "Remove";
            this.removeObjectButton.UseVisualStyleBackColor = true;
            this.removeObjectButton.Click += new System.EventHandler(this.removeObjectButton_Click);
            // 
            // clearObjectsButton
            // 
            this.clearObjectsButton.Location = new System.Drawing.Point(4, 295);
            this.clearObjectsButton.Name = "clearObjectsButton";
            this.clearObjectsButton.Size = new System.Drawing.Size(75, 23);
            this.clearObjectsButton.TabIndex = 24;
            this.clearObjectsButton.Text = "Clear";
            this.clearObjectsButton.UseVisualStyleBackColor = true;
            this.clearObjectsButton.Click += new System.EventHandler(this.clearObjectsButton_Click);
            // 
            // duplicateObjectButton
            // 
            this.duplicateObjectButton.Location = new System.Drawing.Point(81, 251);
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
            this.mapListNumberLabel.Location = new System.Drawing.Point(119, 1);
            this.mapListNumberLabel.Name = "mapListNumberLabel";
            this.mapListNumberLabel.Size = new System.Drawing.Size(57, 13);
            this.mapListNumberLabel.TabIndex = 26;
            this.mapListNumberLabel.Text = "Map List #";
            // 
            // mapListNumberUD
            // 
            this.mapListNumberUD.Location = new System.Drawing.Point(122, 16);
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
            // SetFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapListNumberUD);
            this.Controls.Add(this.mapListNumberLabel);
            this.Controls.Add(this.duplicateObjectButton);
            this.Controls.Add(this.clearObjectsButton);
            this.Controls.Add(this.removeObjectButton);
            this.Controls.Add(this.addObjectButton);
            this.Controls.Add(this.removeObjectList);
            this.Controls.Add(this.addObjectList);
            this.Controls.Add(this.removeMapListButton);
            this.Controls.Add(this.addMapListButton);
            this.Controls.Add(this.editObjectSetHeaderBytesButton);
            this.Controls.Add(this.editObjectMetaDataButton);
            this.Controls.Add(this.editStartBytesButton);
            this.Controls.Add(this.unkIntUD);
            this.Controls.Add(this.unkIntLabel);
            this.Controls.Add(this.objIDUD);
            this.Controls.Add(this.rotationGroupBox);
            this.Controls.Add(this.objIDLabel);
            this.Controls.Add(this.positionGroupBox);
            this.Controls.Add(this.objectSetLabel);
            this.Controls.Add(this.objectListCB);
            this.Controls.Add(this.mapListLabel);
            this.Controls.Add(this.mapListCB);
            this.Controls.Add(this.areaValueNUD);
            this.Controls.Add(this.areaLabel);
            this.Controls.Add(this.setObjectListBox);
            this.Controls.Add(this.ImportJSONButton);
            this.Controls.Add(this.ExportJSONButton);
            this.Name = "SetFileViewer";
            this.Size = new System.Drawing.Size(496, 324);
            ((System.ComponentModel.ISupportInitialize)(this.areaValueNUD)).EndInit();
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
        private System.Windows.Forms.NumericUpDown areaValueNUD;
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
        private System.Windows.Forms.Button editStartBytesButton;
        private System.Windows.Forms.Button editObjectMetaDataButton;
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
    }
}
