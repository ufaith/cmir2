namespace Server.MirForms.VisualMapInfo
{
    partial class VForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VForm));
            this.Tool = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.SelectButton = new System.Windows.Forms.ToolStripButton();
            this.AddButton = new System.Windows.Forms.ToolStripButton();
            this.MoveButton = new System.Windows.Forms.ToolStripButton();
            this.ResizeButton = new System.Windows.Forms.ToolStripButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.RespawnPanel = new System.Windows.Forms.Panel();
            this.RespawnTools = new System.Windows.Forms.ToolStrip();
            this.RespawnsSelectAll = new System.Windows.Forms.ToolStripButton();
            this.RespawnsSelectNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RespawnsRemoveSelected = new System.Windows.Forms.ToolStripButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.MiningPanel = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.MiningSelectAll = new System.Windows.Forms.ToolStripButton();
            this.MiningSelectNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MiningRemoveSelected = new System.Windows.Forms.ToolStripButton();
            this.RegionTabs = new System.Windows.Forms.TabControl();
            this.mapContainer1 = new Server.MirForms.Control.MapContainer();
            this.MapImage = new System.Windows.Forms.PictureBox();
            this.Tool.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.RespawnTools.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.RegionTabs.SuspendLayout();
            this.mapContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapImage)).BeginInit();
            this.SuspendLayout();
            // 
            // Tool
            // 
            this.Tool.AutoSize = false;
            this.Tool.Dock = System.Windows.Forms.DockStyle.Left;
            this.Tool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Tool.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.Tool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.SelectButton,
            this.AddButton,
            this.MoveButton,
            this.ResizeButton});
            this.Tool.Location = new System.Drawing.Point(0, 0);
            this.Tool.Name = "Tool";
            this.Tool.Size = new System.Drawing.Size(45, 493);
            this.Tool.TabIndex = 1;
            this.Tool.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 15);
            this.toolStripLabel1.Text = " ";
            // 
            // SelectButton
            // 
            this.SelectButton.AutoSize = false;
            this.SelectButton.Checked = true;
            this.SelectButton.CheckOnClick = true;
            this.SelectButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectButton.Image")));
            this.SelectButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SelectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(28, 28);
            this.SelectButton.Text = "Select Region";
            this.SelectButton.Click += new System.EventHandler(this.ToolSelectedChanged);
            // 
            // AddButton
            // 
            this.AddButton.AutoSize = false;
            this.AddButton.CheckOnClick = true;
            this.AddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddButton.Image = ((System.Drawing.Image)(resources.GetObject("AddButton.Image")));
            this.AddButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AddButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(28, 28);
            this.AddButton.Text = "Add Region";
            this.AddButton.Click += new System.EventHandler(this.ToolSelectedChanged);
            // 
            // MoveButton
            // 
            this.MoveButton.AutoSize = false;
            this.MoveButton.CheckOnClick = true;
            this.MoveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveButton.Image")));
            this.MoveButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.MoveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(28, 28);
            this.MoveButton.Text = "Move Region";
            this.MoveButton.Click += new System.EventHandler(this.ToolSelectedChanged);
            // 
            // ResizeButton
            // 
            this.ResizeButton.AutoSize = false;
            this.ResizeButton.CheckOnClick = true;
            this.ResizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ResizeButton.Image = ((System.Drawing.Image)(resources.GetObject("ResizeButton.Image")));
            this.ResizeButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ResizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResizeButton.Name = "ResizeButton";
            this.ResizeButton.Size = new System.Drawing.Size(28, 28);
            this.ResizeButton.Text = "Resize Region";
            this.ResizeButton.Click += new System.EventHandler(this.ToolSelectedChanged);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(691, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 493);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.RespawnPanel);
            this.tabPage2.Controls.Add(this.RespawnTools);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(242, 445);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Respawns";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // RespawnPanel
            // 
            this.RespawnPanel.AutoScroll = true;
            this.RespawnPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RespawnPanel.Location = new System.Drawing.Point(0, 25);
            this.RespawnPanel.Name = "RespawnPanel";
            this.RespawnPanel.Size = new System.Drawing.Size(242, 420);
            this.RespawnPanel.TabIndex = 3;
            // 
            // RespawnTools
            // 
            this.RespawnTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.RespawnTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RespawnsSelectAll,
            this.RespawnsSelectNone,
            this.toolStripSeparator3,
            this.RespawnsRemoveSelected});
            this.RespawnTools.Location = new System.Drawing.Point(0, 0);
            this.RespawnTools.Name = "RespawnTools";
            this.RespawnTools.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.RespawnTools.Size = new System.Drawing.Size(242, 25);
            this.RespawnTools.TabIndex = 2;
            this.RespawnTools.Text = "toolStrip1";
            // 
            // RespawnsSelectAll
            // 
            this.RespawnsSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RespawnsSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("RespawnsSelectAll.Image")));
            this.RespawnsSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RespawnsSelectAll.Name = "RespawnsSelectAll";
            this.RespawnsSelectAll.Size = new System.Drawing.Size(23, 22);
            this.RespawnsSelectAll.Text = "Select All";
            this.RespawnsSelectAll.Click += new System.EventHandler(this.RespawnsSelectAll_Click);
            // 
            // RespawnsSelectNone
            // 
            this.RespawnsSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RespawnsSelectNone.Image = ((System.Drawing.Image)(resources.GetObject("RespawnsSelectNone.Image")));
            this.RespawnsSelectNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RespawnsSelectNone.Name = "RespawnsSelectNone";
            this.RespawnsSelectNone.Size = new System.Drawing.Size(23, 22);
            this.RespawnsSelectNone.Text = "Select None";
            this.RespawnsSelectNone.Click += new System.EventHandler(this.RespawnsSelectNone_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // RespawnsRemoveSelected
            // 
            this.RespawnsRemoveSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RespawnsRemoveSelected.Image = ((System.Drawing.Image)(resources.GetObject("RespawnsRemoveSelected.Image")));
            this.RespawnsRemoveSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RespawnsRemoveSelected.Name = "RespawnsRemoveSelected";
            this.RespawnsRemoveSelected.Size = new System.Drawing.Size(23, 22);
            this.RespawnsRemoveSelected.Text = "Remove Selected";
            this.RespawnsRemoveSelected.Click += new System.EventHandler(this.RespawnsRemoveSelected_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.MiningPanel);
            this.tabPage4.Controls.Add(this.toolStrip2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(242, 467);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Mining";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // MiningPanel
            // 
            this.MiningPanel.AutoScroll = true;
            this.MiningPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MiningPanel.Location = new System.Drawing.Point(0, 25);
            this.MiningPanel.Name = "MiningPanel";
            this.MiningPanel.Size = new System.Drawing.Size(242, 442);
            this.MiningPanel.TabIndex = 3;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MiningSelectAll,
            this.MiningSelectNone,
            this.toolStripSeparator1,
            this.MiningRemoveSelected});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip2.Size = new System.Drawing.Size(242, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip1";
            // 
            // MiningSelectAll
            // 
            this.MiningSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MiningSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("MiningSelectAll.Image")));
            this.MiningSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MiningSelectAll.Name = "MiningSelectAll";
            this.MiningSelectAll.Size = new System.Drawing.Size(23, 22);
            this.MiningSelectAll.Text = "Select All";
            this.MiningSelectAll.Click += new System.EventHandler(this.MiningSelectAll_Click);
            // 
            // MiningSelectNone
            // 
            this.MiningSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MiningSelectNone.Image = ((System.Drawing.Image)(resources.GetObject("MiningSelectNone.Image")));
            this.MiningSelectNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MiningSelectNone.Name = "MiningSelectNone";
            this.MiningSelectNone.Size = new System.Drawing.Size(23, 22);
            this.MiningSelectNone.Text = "Select None";
            this.MiningSelectNone.Click += new System.EventHandler(this.MiningSelectNone_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MiningRemoveSelected
            // 
            this.MiningRemoveSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MiningRemoveSelected.Image = ((System.Drawing.Image)(resources.GetObject("MiningRemoveSelected.Image")));
            this.MiningRemoveSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MiningRemoveSelected.Name = "MiningRemoveSelected";
            this.MiningRemoveSelected.Size = new System.Drawing.Size(23, 22);
            this.MiningRemoveSelected.Text = "Remove Selected";
            this.MiningRemoveSelected.Click += new System.EventHandler(this.MiningRemoveSelected_Click);
            // 
            // RegionTabs
            // 
            this.RegionTabs.Controls.Add(this.tabPage4);
            this.RegionTabs.Controls.Add(this.tabPage2);
            this.RegionTabs.Dock = System.Windows.Forms.DockStyle.Right;
            this.RegionTabs.Location = new System.Drawing.Point(694, 0);
            this.RegionTabs.Multiline = true;
            this.RegionTabs.Name = "RegionTabs";
            this.RegionTabs.SelectedIndex = 0;
            this.RegionTabs.Size = new System.Drawing.Size(250, 493);
            this.RegionTabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.RegionTabs.TabIndex = 3;
            this.RegionTabs.SelectedIndexChanged += new System.EventHandler(this.RegionTabs_SelectedIndexChanged);
            // 
            // mapContainer1
            // 
            this.mapContainer1.AutoScroll = true;
            this.mapContainer1.Controls.Add(this.MapImage);
            this.mapContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapContainer1.Location = new System.Drawing.Point(45, 0);
            this.mapContainer1.Name = "mapContainer1";
            this.mapContainer1.Size = new System.Drawing.Size(646, 493);
            this.mapContainer1.TabIndex = 2;
            // 
            // MapImage
            // 
            this.MapImage.Location = new System.Drawing.Point(0, 0);
            this.MapImage.Name = "MapImage";
            this.MapImage.Size = new System.Drawing.Size(0, 0);
            this.MapImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.MapImage.TabIndex = 0;
            this.MapImage.TabStop = false;
            this.MapImage.Click += new System.EventHandler(this.MapImage_Click);
            this.MapImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapImage_MouseDown);
            // 
            // VForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 493);
            this.Controls.Add(this.mapContainer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.RegionTabs);
            this.Controls.Add(this.Tool);
            this.DoubleBuffered = true;
            this.Name = "VForm";
            this.Text = "Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VForm_FormClosing);
            this.Load += new System.EventHandler(this.VForm_Load);
            this.Tool.ResumeLayout(false);
            this.Tool.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.RespawnTools.ResumeLayout(false);
            this.RespawnTools.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.RegionTabs.ResumeLayout(false);
            this.mapContainer1.ResumeLayout(false);
            this.mapContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip Tool;
        private System.Windows.Forms.ToolStripButton SelectButton;
        private System.Windows.Forms.ToolStripButton AddButton;
        private System.Windows.Forms.ToolStripButton MoveButton;
        private System.Windows.Forms.ToolStripButton ResizeButton;
        private Server.MirForms.Control.MapContainer mapContainer1;
        private System.Windows.Forms.PictureBox MapImage;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel RespawnPanel;
        private System.Windows.Forms.ToolStrip RespawnTools;
        private System.Windows.Forms.ToolStripButton RespawnsSelectAll;
        private System.Windows.Forms.ToolStripButton RespawnsSelectNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton RespawnsRemoveSelected;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel MiningPanel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton MiningSelectAll;
        private System.Windows.Forms.ToolStripButton MiningSelectNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton MiningRemoveSelected;
        private System.Windows.Forms.TabControl RegionTabs;
    }
}