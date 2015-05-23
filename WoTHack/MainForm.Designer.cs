namespace WoTHack
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.processesComboBox = new System.Windows.Forms.ComboBox();
            this.noTreesCheckBox = new System.Windows.Forms.CheckBox();
            this.inBothModesCheckBox = new System.Windows.Forms.CheckBox();
            this.startStopButton = new System.Windows.Forms.Button();
            this.treeToggleKeyComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // processesComboBox
            // 
            this.processesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processesComboBox.FormattingEnabled = true;
            this.processesComboBox.Location = new System.Drawing.Point(141, 12);
            this.processesComboBox.Name = "processesComboBox";
            this.processesComboBox.Size = new System.Drawing.Size(140, 21);
            this.processesComboBox.TabIndex = 0;
            this.processesComboBox.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            this.processesComboBox.SelectionChangeCommitted += new System.EventHandler(this.processesComboBox_SelectionChangeCommitted);
            // 
            // noTreesCheckBox
            // 
            this.noTreesCheckBox.AutoSize = true;
            this.noTreesCheckBox.Location = new System.Drawing.Point(12, 39);
            this.noTreesCheckBox.Name = "noTreesCheckBox";
            this.noTreesCheckBox.Size = new System.Drawing.Size(66, 17);
            this.noTreesCheckBox.TabIndex = 1;
            this.noTreesCheckBox.Text = "No trees";
            this.noTreesCheckBox.UseVisualStyleBackColor = true;
            this.noTreesCheckBox.CheckedChanged += new System.EventHandler(this.noTreesCheckBox_CheckedChanged);
            // 
            // inBothModesCheckBox
            // 
            this.inBothModesCheckBox.AutoSize = true;
            this.inBothModesCheckBox.Enabled = false;
            this.inBothModesCheckBox.Location = new System.Drawing.Point(12, 62);
            this.inBothModesCheckBox.Name = "inBothModesCheckBox";
            this.inBothModesCheckBox.Size = new System.Drawing.Size(93, 17);
            this.inBothModesCheckBox.TabIndex = 2;
            this.inBothModesCheckBox.Text = "In both modes";
            this.inBothModesCheckBox.UseVisualStyleBackColor = true;
            this.inBothModesCheckBox.CheckedChanged += new System.EventHandler(this.inBothModesCheckBox_CheckedChanged);
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(206, 39);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(75, 23);
            this.startStopButton.TabIndex = 3;
            this.startStopButton.Text = "w";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // treeToggleKeyComboBox
            // 
            this.treeToggleKeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.treeToggleKeyComboBox.FormattingEnabled = true;
            this.treeToggleKeyComboBox.Location = new System.Drawing.Point(12, 106);
            this.treeToggleKeyComboBox.Name = "treeToggleKeyComboBox";
            this.treeToggleKeyComboBox.Size = new System.Drawing.Size(77, 21);
            this.treeToggleKeyComboBox.TabIndex = 4;
            this.treeToggleKeyComboBox.SelectionChangeCommitted += new System.EventHandler(this.treeToggleKeyComboBox_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tree Toggle Key";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 139);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeToggleKeyComboBox);
            this.Controls.Add(this.startStopButton);
            this.Controls.Add(this.inBothModesCheckBox);
            this.Controls.Add(this.noTreesCheckBox);
            this.Controls.Add(this.processesComboBox);
            this.Name = "MainForm";
            this.Text = "WoTHack";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox processesComboBox;
        private System.Windows.Forms.CheckBox noTreesCheckBox;
        private System.Windows.Forms.CheckBox inBothModesCheckBox;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.ComboBox treeToggleKeyComboBox;
        private System.Windows.Forms.Label label1;
    }
}

