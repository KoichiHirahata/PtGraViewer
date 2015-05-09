namespace PtGraViewer
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btShowAll = new System.Windows.Forms.Button();
            this.left_load_label = new System.Windows.Forms.Label();
            this.LvParent = new System.Windows.Forms.ListView();
            this.right_load_label = new System.Windows.Forms.Label();
            this.LvItems = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbPtID = new System.Windows.Forms.TextBox();
            this.LbPtID = new System.Windows.Forms.Label();
            this.BtView = new System.Windows.Forms.Button();
            this.ilParent = new System.Windows.Forms.ImageList(this.components);
            this.ilItems = new System.Windows.Forms.ImageList(this.components);
            this.btOpenFolder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.btShowAll);
            this.splitContainer1.Panel1.Controls.Add(this.left_load_label);
            this.splitContainer1.Panel1.Controls.Add(this.LvParent);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.right_load_label);
            this.splitContainer1.Panel2.Controls.Add(this.LvItems);
            // 
            // btShowAll
            // 
            resources.ApplyResources(this.btShowAll, "btShowAll");
            this.btShowAll.Name = "btShowAll";
            this.btShowAll.UseVisualStyleBackColor = true;
            this.btShowAll.Click += new System.EventHandler(this.btShowAll_Click);
            // 
            // left_load_label
            // 
            resources.ApplyResources(this.left_load_label, "left_load_label");
            this.left_load_label.BackColor = System.Drawing.Color.LightBlue;
            this.left_load_label.Name = "left_load_label";
            // 
            // LvParent
            // 
            resources.ApplyResources(this.LvParent, "LvParent");
            this.LvParent.Name = "LvParent";
            this.LvParent.UseCompatibleStateImageBehavior = false;
            this.LvParent.DoubleClick += new System.EventHandler(this.LvParent_DoubleClick);
            this.LvParent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvParent_KeyDown);
            // 
            // right_load_label
            // 
            resources.ApplyResources(this.right_load_label, "right_load_label");
            this.right_load_label.BackColor = System.Drawing.Color.LightBlue;
            this.right_load_label.Name = "right_load_label";
            // 
            // LvItems
            // 
            resources.ApplyResources(this.LvItems, "LvItems");
            this.LvItems.Name = "LvItems";
            this.LvItems.UseCompatibleStateImageBehavior = false;
            this.LvItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvItems_KeyDown);
            this.LvItems.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvItems_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // tbPtID
            // 
            resources.ApplyResources(this.tbPtID, "tbPtID");
            this.tbPtID.Name = "tbPtID";
            this.tbPtID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPtID_KeyDown);
            // 
            // LbPtID
            // 
            resources.ApplyResources(this.LbPtID, "LbPtID");
            this.LbPtID.Name = "LbPtID";
            // 
            // BtView
            // 
            resources.ApplyResources(this.BtView, "BtView");
            this.BtView.Name = "BtView";
            this.BtView.UseVisualStyleBackColor = true;
            this.BtView.Click += new System.EventHandler(this.BtView_Click);
            // 
            // ilParent
            // 
            this.ilParent.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            resources.ApplyResources(this.ilParent, "ilParent");
            this.ilParent.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ilItems
            // 
            this.ilItems.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            resources.ApplyResources(this.ilItems, "ilItems");
            this.ilItems.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btOpenFolder
            // 
            resources.ApplyResources(this.btOpenFolder, "btOpenFolder");
            this.btOpenFolder.Name = "btOpenFolder";
            this.btOpenFolder.UseVisualStyleBackColor = true;
            this.btOpenFolder.Click += new System.EventHandler(this.btOpenFolder_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btOpenFolder);
            this.Controls.Add(this.BtView);
            this.Controls.Add(this.LbPtID);
            this.Controls.Add(this.tbPtID);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView LvParent;
        private System.Windows.Forms.ListView LvItems;
        private System.Windows.Forms.TextBox tbPtID;
        private System.Windows.Forms.Label LbPtID;
        private System.Windows.Forms.Button BtView;
        private System.Windows.Forms.ImageList ilParent;
        private System.Windows.Forms.ImageList ilItems;
        private System.Windows.Forms.Button btOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label left_load_label;
        private System.Windows.Forms.Label right_load_label;
        private System.Windows.Forms.Button btShowAll;
    }
}

