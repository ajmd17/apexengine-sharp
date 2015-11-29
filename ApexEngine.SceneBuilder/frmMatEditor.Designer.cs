namespace ApexEditor
{
    partial class frmMatEditor
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
            this.pnlMtl = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.blendModeBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.specTechnique = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.shininessLevel = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPPL = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.colorDialog3 = new System.Windows.Forms.ColorDialog();
            this.castsShadows = new System.Windows.Forms.CheckBox();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shininessLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMtl
            // 
            this.pnlMtl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMtl.Location = new System.Drawing.Point(12, 12);
            this.pnlMtl.Name = "pnlMtl";
            this.pnlMtl.Size = new System.Drawing.Size(256, 256);
            this.pnlMtl.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Gainsboro;
            this.button1.Location = new System.Drawing.Point(464, 559);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(129, 390);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(10, 2);
            this.listBox1.TabIndex = 6;
            this.listBox1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.trackBar3);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.trackBar2);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.blendModeBox);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.specTechnique);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.shininessLevel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.ForeColor = System.Drawing.Color.Gainsboro;
            this.panel1.Location = new System.Drawing.Point(274, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 532);
            this.panel1.TabIndex = 7;
            // 
            // trackBar2
            // 
            this.trackBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar2.Location = new System.Drawing.Point(6, 146);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(256, 45);
            this.trackBar2.TabIndex = 16;
            this.trackBar2.TickFrequency = 5;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar2.Value = 45;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "Roughness:";
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(6, 80);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(256, 45);
            this.trackBar1.TabIndex = 14;
            this.trackBar1.TickFrequency = 5;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar1.Value = 45;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "Specular Exponent:";
            // 
            // blendModeBox
            // 
            this.blendModeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blendModeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.blendModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blendModeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blendModeBox.ForeColor = System.Drawing.Color.Gainsboro;
            this.blendModeBox.FormattingEnabled = true;
            this.blendModeBox.Items.AddRange(new object[] {
            "Opaque",
            "Alpha Blend"});
            this.blendModeBox.Location = new System.Drawing.Point(6, 466);
            this.blendModeBox.Name = "blendModeBox";
            this.blendModeBox.Size = new System.Drawing.Size(256, 23);
            this.blendModeBox.TabIndex = 12;
            this.blendModeBox.SelectedIndexChanged += new System.EventHandler(this.blendModeBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 448);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Blend Mode:";
            // 
            // specTechnique
            // 
            this.specTechnique.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.specTechnique.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.specTechnique.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specTechnique.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.specTechnique.ForeColor = System.Drawing.Color.Gainsboro;
            this.specTechnique.FormattingEnabled = true;
            this.specTechnique.Items.AddRange(new object[] {
            "Default",
            "Simple"});
            this.specTechnique.Location = new System.Drawing.Point(6, 422);
            this.specTechnique.Name = "specTechnique";
            this.specTechnique.Size = new System.Drawing.Size(256, 23);
            this.specTechnique.TabIndex = 9;
            this.specTechnique.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 404);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Specular Technique:";
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(6, 278);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(256, 27);
            this.button4.TabIndex = 7;
            this.button4.Text = "Pick color...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ambient Color:";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(6, 374);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(256, 27);
            this.button3.TabIndex = 5;
            this.button3.Text = "Pick color...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Specular Color:";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(6, 326);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(256, 27);
            this.button2.TabIndex = 3;
            this.button2.Text = "Pick color...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Diffuse Color:";
            // 
            // shininessLevel
            // 
            this.shininessLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shininessLevel.Location = new System.Drawing.Point(6, 18);
            this.shininessLevel.Maximum = 100;
            this.shininessLevel.Name = "shininessLevel";
            this.shininessLevel.Size = new System.Drawing.Size(256, 45);
            this.shininessLevel.TabIndex = 1;
            this.shininessLevel.TickFrequency = 5;
            this.shininessLevel.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.shininessLevel.Value = 50;
            this.shininessLevel.Scroll += new System.EventHandler(this.shininessLevel_Scroll);
            this.shininessLevel.ValueChanged += new System.EventHandler(this.shininessLevel_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shininess:";
            // 
            // cbPPL
            // 
            this.cbPPL.AutoSize = true;
            this.cbPPL.Checked = true;
            this.cbPPL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPPL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbPPL.Location = new System.Drawing.Point(12, 274);
            this.cbPPL.Name = "cbPPL";
            this.cbPPL.Size = new System.Drawing.Size(114, 19);
            this.cbPPL.TabIndex = 10;
            this.cbPPL.Text = "Per-pixel lighting";
            this.cbPPL.UseVisualStyleBackColor = true;
            this.cbPPL.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // castsShadows
            // 
            this.castsShadows.AutoSize = true;
            this.castsShadows.Checked = true;
            this.castsShadows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.castsShadows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.castsShadows.Location = new System.Drawing.Point(12, 299);
            this.castsShadows.Name = "castsShadows";
            this.castsShadows.Size = new System.Drawing.Size(110, 19);
            this.castsShadows.TabIndex = 13;
            this.castsShadows.Text = "Casts shadows";
            this.castsShadows.UseVisualStyleBackColor = true;
            this.castsShadows.CheckedChanged += new System.EventHandler(this.castsShadows_CheckedChanged);
            // 
            // trackBar3
            // 
            this.trackBar3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar3.Location = new System.Drawing.Point(6, 212);
            this.trackBar3.Maximum = 100;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(256, 45);
            this.trackBar3.TabIndex = 18;
            this.trackBar3.TickFrequency = 5;
            this.trackBar3.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar3.Value = 45;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 15);
            this.label9.TabIndex = 17;
            this.label9.Text = "Metalness:";
            // 
            // frmMatEditor
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(551, 594);
            this.Controls.Add(this.castsShadows);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbPPL);
            this.Controls.Add(this.pnlMtl);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 357);
            this.Name = "frmMatEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Material";
            this.Load += new System.EventHandler(this.frmMatEditor_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shininessLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMtl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar shininessLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.ColorDialog colorDialog3;
        private System.Windows.Forms.ComboBox specTechnique;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbPPL;
        private System.Windows.Forms.ComboBox blendModeBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox castsShadows;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.Label label9;
    }
}