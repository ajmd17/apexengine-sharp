namespace ApexEditor
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.topPanel = new System.Windows.Forms.Panel();
            this.metroMenuStrip1 = new ModernUISample.metro.MetroMenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAsSeperateSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.metroMenuStrip3 = new ModernUISample.metro.MetroMenuStrip();
            this.pROJECTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pnlGameView = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.metroMenuStrip4 = new ModernUISample.metro.MetroMenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.metroMenuStrip2 = new ModernUISample.metro.MetroMenuStrip();
            this.aSSETSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.topPanel.SuspendLayout();
            this.metroMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.metroMenuStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.metroMenuStrip4.SuspendLayout();
            this.metroMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.metroMenuStrip1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(976, 30);
            this.topPanel.TabIndex = 1;
            // 
            // metroMenuStrip1
            // 
            this.metroMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.metroMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.metroMenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.metroMenuStrip1.Name = "metroMenuStrip1";
            this.metroMenuStrip1.Size = new System.Drawing.Size(976, 24);
            this.metroMenuStrip1.TabIndex = 0;
            this.metroMenuStrip1.Text = "metroMenuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Arial", 10F);
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.fileToolStripMenuItem.Text = "FILE";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.newToolStripMenuItem.Text = "&New...";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openAsSeperateSceneToolStripMenuItem,
            this.addToSceneToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openAsSeperateSceneToolStripMenuItem
            // 
            this.openAsSeperateSceneToolStripMenuItem.Name = "openAsSeperateSceneToolStripMenuItem";
            this.openAsSeperateSceneToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.openAsSeperateSceneToolStripMenuItem.Text = "Open as seperate scene...";
            this.openAsSeperateSceneToolStripMenuItem.Click += new System.EventHandler(this.openAsSeperateSceneToolStripMenuItem_Click);
            // 
            // addToSceneToolStripMenuItem
            // 
            this.addToSceneToolStripMenuItem.Name = "addToSceneToolStripMenuItem";
            this.addToSceneToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.addToSceneToolStripMenuItem.Text = "Add to scene...";
            this.addToSceneToolStripMenuItem.Click += new System.EventHandler(this.addToSceneToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.exitToolStripMenuItem.Text = "&Exit...";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 30);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.metroMenuStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(976, 414);
            this.splitContainer1.SplitterDistance = 255;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            this.splitContainer2.Panel1.Controls.Add(this.metroMenuStrip3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(976, 255);
            this.splitContainer2.SplitterDistance = 260;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.LightGray;
            this.treeView1.Location = new System.Drawing.Point(0, 24);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(260, 231);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // metroMenuStrip3
            // 
            this.metroMenuStrip3.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.metroMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pROJECTToolStripMenuItem});
            this.metroMenuStrip3.Location = new System.Drawing.Point(0, 0);
            this.metroMenuStrip3.Name = "metroMenuStrip3";
            this.metroMenuStrip3.Padding = new System.Windows.Forms.Padding(0);
            this.metroMenuStrip3.Size = new System.Drawing.Size(260, 24);
            this.metroMenuStrip3.TabIndex = 0;
            this.metroMenuStrip3.Text = "metroMenuStrip3";
            // 
            // pROJECTToolStripMenuItem
            // 
            this.pROJECTToolStripMenuItem.Enabled = false;
            this.pROJECTToolStripMenuItem.Font = new System.Drawing.Font("Arial", 10F);
            this.pROJECTToolStripMenuItem.Name = "pROJECTToolStripMenuItem";
            this.pROJECTToolStripMenuItem.Size = new System.Drawing.Size(98, 24);
            this.pROJECTToolStripMenuItem.Text = "HIERARCHY";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.pnlGameView);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer3.Panel2.Controls.Add(this.metroMenuStrip4);
            this.splitContainer3.Size = new System.Drawing.Size(712, 255);
            this.splitContainer3.SplitterDistance = 485;
            this.splitContainer3.TabIndex = 0;
            // 
            // pnlGameView
            // 
            this.pnlGameView.BackColor = System.Drawing.Color.White;
            this.pnlGameView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGameView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGameView.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlGameView.ForeColor = System.Drawing.Color.Gainsboro;
            this.pnlGameView.Location = new System.Drawing.Point(0, 0);
            this.pnlGameView.Name = "pnlGameView";
            this.pnlGameView.Size = new System.Drawing.Size(485, 255);
            this.pnlGameView.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 24);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(223, 231);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // metroMenuStrip4
            // 
            this.metroMenuStrip4.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.metroMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.metroMenuStrip4.Location = new System.Drawing.Point(0, 0);
            this.metroMenuStrip4.Name = "metroMenuStrip4";
            this.metroMenuStrip4.Padding = new System.Windows.Forms.Padding(0);
            this.metroMenuStrip4.Size = new System.Drawing.Size(223, 24);
            this.metroMenuStrip4.TabIndex = 1;
            this.metroMenuStrip4.Text = "metroMenuStrip4";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Arial", 10F);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(108, 24);
            this.toolStripMenuItem1.Text = "PROPERTIES";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // metroMenuStrip2
            // 
            this.metroMenuStrip2.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.metroMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aSSETSToolStripMenuItem});
            this.metroMenuStrip2.Location = new System.Drawing.Point(0, 0);
            this.metroMenuStrip2.Name = "metroMenuStrip2";
            this.metroMenuStrip2.Size = new System.Drawing.Size(976, 24);
            this.metroMenuStrip2.TabIndex = 0;
            this.metroMenuStrip2.Text = "metroMenuStrip2";
            // 
            // aSSETSToolStripMenuItem
            // 
            this.aSSETSToolStripMenuItem.Enabled = false;
            this.aSSETSToolStripMenuItem.Font = new System.Drawing.Font("Arial", 10F);
            this.aSSETSToolStripMenuItem.Name = "aSSETSToolStripMenuItem";
            this.aSSETSToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.aSSETSToolStripMenuItem.Text = "ASSETS";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Apx files (*.apx)|*.txt|Obj models (*.obj)|*.obj|All files (*.*)|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "apx";
            this.saveFileDialog1.Filter = "Apx files|*.apx";
            this.saveFileDialog1.Title = "Export model as APX";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 444);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.topPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.metroMenuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Apex3D Scene Builder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.metroMenuStrip1.ResumeLayout(false);
            this.metroMenuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.metroMenuStrip3.ResumeLayout(false);
            this.metroMenuStrip3.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.metroMenuStrip4.ResumeLayout(false);
            this.metroMenuStrip4.PerformLayout();
            this.metroMenuStrip2.ResumeLayout(false);
            this.metroMenuStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private ModernUISample.metro.MetroMenuStrip metroMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private ModernUISample.metro.MetroMenuStrip metroMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem aSSETSToolStripMenuItem;
        private ModernUISample.metro.MetroMenuStrip metroMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem pROJECTToolStripMenuItem;
        private System.Windows.Forms.Panel pnlGameView;
        private ModernUISample.metro.MetroMenuStrip metroMenuStrip4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripMenuItem openAsSeperateSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToSceneToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        //  private ApexEngineControl apexEngineControl1;
    }
}