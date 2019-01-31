namespace TayaIT.DirectoryWatcher
{
    partial class frmNotifier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        

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
            this.components = new System.ComponentModel.Container();
            this.txtSrcPath = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.btnWatchFile = new System.Windows.Forms.Button();
            this.lstNotification = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tmrEditNotify = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSubFolder = new System.Windows.Forms.CheckBox();
            this.rdbDir = new System.Windows.Forms.RadioButton();
            this.chkDeleteAfterCopy = new System.Windows.Forms.CheckBox();
            this.btnBrowseSrcPath = new System.Windows.Forms.Button();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.dlgOpenDir = new System.Windows.Forms.FolderBrowserDialog();
            this.btnLog = new System.Windows.Forms.Button();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDestPath = new System.Windows.Forms.TextBox();
            this.btnBrowseDestPath = new System.Windows.Forms.Button();
            this.txtTmpDir = new System.Windows.Forms.TextBox();
            this.btnBrowseTmpFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnApplyChanges = new System.Windows.Forms.Button();
            this.btnDeployService = new System.Windows.Forms.Button();
            this.btnRemoveService = new System.Windows.Forms.Button();
            this.btnStartService = new System.Windows.Forms.Button();
            this.btnStopService = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSrcPath
            // 
            this.txtSrcPath.Location = new System.Drawing.Point(182, 83);
            this.txtSrcPath.Name = "txtSrcPath";
            this.txtSrcPath.Size = new System.Drawing.Size(268, 20);
            this.txtSrcPath.TabIndex = 0;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFile.Location = new System.Drawing.Point(11, 90);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(154, 13);
            this.lblFile.TabIndex = 1;
            this.lblFile.Text = "Select Directory to Watch";
            // 
            // btnWatchFile
            // 
            this.btnWatchFile.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnWatchFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnWatchFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnWatchFile.Location = new System.Drawing.Point(537, 13);
            this.btnWatchFile.Name = "btnWatchFile";
            this.btnWatchFile.Size = new System.Drawing.Size(94, 294);
            this.btnWatchFile.TabIndex = 4;
            this.btnWatchFile.Text = "Start Watching";
            this.btnWatchFile.UseVisualStyleBackColor = false;
            this.btnWatchFile.Click += new System.EventHandler(this.btnWatchFile_Click);
            // 
            // lstNotification
            // 
            this.lstNotification.FormattingEnabled = true;
            this.lstNotification.Location = new System.Drawing.Point(11, 339);
            this.lstNotification.Name = "lstNotification";
            this.lstNotification.Size = new System.Drawing.Size(461, 277);
            this.lstNotification.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 323);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Change Notifications";
            // 
            // tmrEditNotify
            // 
            this.tmrEditNotify.Enabled = true;
            this.tmrEditNotify.Tick += new System.EventHandler(this.tmrEditNotify_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSubFolder);
            this.groupBox1.Controls.Add(this.rdbDir);
            this.groupBox1.Controls.Add(this.chkDeleteAfterCopy);
            this.groupBox1.Location = new System.Drawing.Point(15, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 47);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode";
            // 
            // chkSubFolder
            // 
            this.chkSubFolder.AutoSize = true;
            this.chkSubFolder.Checked = true;
            this.chkSubFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubFolder.Location = new System.Drawing.Point(185, 19);
            this.chkSubFolder.Name = "chkSubFolder";
            this.chkSubFolder.Size = new System.Drawing.Size(114, 17);
            this.chkSubFolder.TabIndex = 2;
            this.chkSubFolder.Text = "Include Subfolders";
            this.chkSubFolder.UseVisualStyleBackColor = true;
            // 
            // rdbDir
            // 
            this.rdbDir.AutoSize = true;
            this.rdbDir.Checked = true;
            this.rdbDir.Location = new System.Drawing.Point(6, 16);
            this.rdbDir.Name = "rdbDir";
            this.rdbDir.Size = new System.Drawing.Size(102, 17);
            this.rdbDir.TabIndex = 1;
            this.rdbDir.TabStop = true;
            this.rdbDir.Text = "Watch Directory";
            this.rdbDir.UseVisualStyleBackColor = true;
            this.rdbDir.CheckedChanged += new System.EventHandler(this.rdbDir_CheckedChanged);
            // 
            // chkDeleteAfterCopy
            // 
            this.chkDeleteAfterCopy.AutoSize = true;
            this.chkDeleteAfterCopy.Location = new System.Drawing.Point(332, 19);
            this.chkDeleteAfterCopy.Name = "chkDeleteAfterCopy";
            this.chkDeleteAfterCopy.Size = new System.Drawing.Size(109, 17);
            this.chkDeleteAfterCopy.TabIndex = 13;
            this.chkDeleteAfterCopy.Text = "Delete After Copy";
            this.chkDeleteAfterCopy.UseVisualStyleBackColor = true;
            // 
            // btnBrowseSrcPath
            // 
            this.btnBrowseSrcPath.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBrowseSrcPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseSrcPath.Location = new System.Drawing.Point(456, 80);
            this.btnBrowseSrcPath.Name = "btnBrowseSrcPath";
            this.btnBrowseSrcPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSrcPath.TabIndex = 8;
            this.btnBrowseSrcPath.Text = "Browse";
            this.btnBrowseSrcPath.UseVisualStyleBackColor = false;
            this.btnBrowseSrcPath.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // dlgOpenDir
            // 
            this.dlgOpenDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // btnLog
            // 
            this.btnLog.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnLog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLog.Location = new System.Drawing.Point(12, 584);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(119, 23);
            this.btnLog.TabIndex = 9;
            this.btnLog.Text = "Dump To Log";
            this.btnLog.UseVisualStyleBackColor = false;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // dlgSaveFile
            // 
            this.dlgSaveFile.DefaultExt = "log";
            this.dlgSaveFile.Filter = "LogFiles|*.log";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Select Directory to Copy To";
            // 
            // txtDestPath
            // 
            this.txtDestPath.Location = new System.Drawing.Point(182, 177);
            this.txtDestPath.Name = "txtDestPath";
            this.txtDestPath.Size = new System.Drawing.Size(268, 20);
            this.txtDestPath.TabIndex = 10;
            // 
            // btnBrowseDestPath
            // 
            this.btnBrowseDestPath.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBrowseDestPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseDestPath.Location = new System.Drawing.Point(456, 174);
            this.btnBrowseDestPath.Name = "btnBrowseDestPath";
            this.btnBrowseDestPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDestPath.TabIndex = 12;
            this.btnBrowseDestPath.Text = "Browse";
            this.btnBrowseDestPath.UseVisualStyleBackColor = false;
            this.btnBrowseDestPath.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtTmpDir
            // 
            this.txtTmpDir.Location = new System.Drawing.Point(182, 132);
            this.txtTmpDir.Name = "txtTmpDir";
            this.txtTmpDir.Size = new System.Drawing.Size(268, 20);
            this.txtTmpDir.TabIndex = 14;
            // 
            // btnBrowseTmpFile
            // 
            this.btnBrowseTmpFile.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBrowseTmpFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseTmpFile.Location = new System.Drawing.Point(456, 129);
            this.btnBrowseTmpFile.Name = "btnBrowseTmpFile";
            this.btnBrowseTmpFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTmpFile.TabIndex = 15;
            this.btnBrowseTmpFile.Text = "Browse";
            this.btnBrowseTmpFile.UseVisualStyleBackColor = false;
            this.btnBrowseTmpFile.Click += new System.EventHandler(this.btnBrowseTmpFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Select Temporary Directory";
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnApplyChanges.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnApplyChanges.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnApplyChanges.Location = new System.Drawing.Point(15, 212);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(516, 37);
            this.btnApplyChanges.TabIndex = 17;
            this.btnApplyChanges.Text = "Apply Changes";
            this.btnApplyChanges.UseVisualStyleBackColor = false;
            this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyChanges_Click);
            // 
            // btnDeployService
            // 
            this.btnDeployService.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnDeployService.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeployService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeployService.Location = new System.Drawing.Point(15, 255);
            this.btnDeployService.Name = "btnDeployService";
            this.btnDeployService.Size = new System.Drawing.Size(239, 23);
            this.btnDeployService.TabIndex = 18;
            this.btnDeployService.Text = "Deploy and start Service";
            this.btnDeployService.UseVisualStyleBackColor = false;
            this.btnDeployService.Click += new System.EventHandler(this.btnDeployService_Click);
            // 
            // btnRemoveService
            // 
            this.btnRemoveService.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnRemoveService.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRemoveService.Location = new System.Drawing.Point(15, 284);
            this.btnRemoveService.Name = "btnRemoveService";
            this.btnRemoveService.Size = new System.Drawing.Size(239, 23);
            this.btnRemoveService.TabIndex = 19;
            this.btnRemoveService.Text = "Remove Service";
            this.btnRemoveService.UseVisualStyleBackColor = false;
            this.btnRemoveService.Click += new System.EventHandler(this.btnRemoveService_Click);
            // 
            // btnStartService
            // 
            this.btnStartService.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnStartService.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStartService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStartService.Location = new System.Drawing.Point(260, 255);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(271, 23);
            this.btnStartService.TabIndex = 20;
            this.btnStartService.Text = "Start Service";
            this.btnStartService.UseVisualStyleBackColor = false;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // btnStopService
            // 
            this.btnStopService.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnStopService.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStopService.Location = new System.Drawing.Point(260, 284);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(271, 23);
            this.btnStopService.TabIndex = 21;
            this.btnStopService.Text = "Stop Service";
            this.btnStopService.UseVisualStyleBackColor = false;
            this.btnStopService.Click += new System.EventHandler(this.btnStopService_Click);
            // 
            // frmNotifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 311);
            this.Controls.Add(this.btnStopService);
            this.Controls.Add(this.btnStartService);
            this.Controls.Add(this.btnRemoveService);
            this.Controls.Add(this.btnDeployService);
            this.Controls.Add(this.btnApplyChanges);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTmpDir);
            this.Controls.Add(this.btnBrowseTmpFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDestPath);
            this.Controls.Add(this.btnBrowseDestPath);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstNotification);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.txtSrcPath);
            this.Controls.Add(this.btnWatchFile);
            this.Controls.Add(this.btnBrowseSrcPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmNotifier";
            this.Text = "Directory Watcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNotifier_FormClosing);
            this.Load += new System.EventHandler(this.frmNotifier_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSrcPath;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Button btnWatchFile;
        private System.Windows.Forms.ListBox lstNotification;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer tmrEditNotify;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbDir;
        private System.Windows.Forms.Button btnBrowseSrcPath;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.FolderBrowserDialog dlgOpenDir;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.SaveFileDialog dlgSaveFile;
        private System.Windows.Forms.CheckBox chkSubFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDestPath;
        private System.Windows.Forms.Button btnBrowseDestPath;
        private System.Windows.Forms.CheckBox chkDeleteAfterCopy;
        private System.Windows.Forms.TextBox txtTmpDir;
        private System.Windows.Forms.Button btnBrowseTmpFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnApplyChanges;
        private System.Windows.Forms.Button btnDeployService;
        private System.Windows.Forms.Button btnRemoveService;
        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.Button btnStopService;
        private System.ComponentModel.IContainer components;
    }
}

