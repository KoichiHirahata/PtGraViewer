namespace PtGraViewer
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.label2 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.sansho_Bt = new System.Windows.Forms.Button();
            this.tbSaveDir = new System.Windows.Forms.TextBox();
            this.cbBtOpenFolderVisible = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.Name = "btCancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btSave
            // 
            resources.ApplyResources(this.btSave, "btSave");
            this.btSave.Name = "btSave";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // sansho_Bt
            // 
            resources.ApplyResources(this.sansho_Bt, "sansho_Bt");
            this.sansho_Bt.Name = "sansho_Bt";
            this.sansho_Bt.UseVisualStyleBackColor = true;
            this.sansho_Bt.Click += new System.EventHandler(this.sansho_Bt_Click_1);
            // 
            // tbSaveDir
            // 
            resources.ApplyResources(this.tbSaveDir, "tbSaveDir");
            this.tbSaveDir.Name = "tbSaveDir";
            // 
            // cbBtOpenFolderVisible
            // 
            resources.ApplyResources(this.cbBtOpenFolderVisible, "cbBtOpenFolderVisible");
            this.cbBtOpenFolderVisible.Name = "cbBtOpenFolderVisible";
            this.cbBtOpenFolderVisible.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbBtOpenFolderVisible);
            this.Controls.Add(this.tbSaveDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.sansho_Bt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button sansho_Bt;
        private System.Windows.Forms.TextBox tbSaveDir;
        private System.Windows.Forms.CheckBox cbBtOpenFolderVisible;
    }
}