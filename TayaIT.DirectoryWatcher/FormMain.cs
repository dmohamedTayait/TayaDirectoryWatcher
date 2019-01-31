using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Xml;
using TayaIT.Trace.Log;
using System.Diagnostics;
using TayaIT.DirectoryWatcher.Config;
using System.Security.Principal;

namespace TayaIT.DirectoryWatcher
{
    public partial class frmNotifier : Form
    {


        private Settings settings = null;
        ServiceUtility serviceUtility = null;
        private StringBuilder m_Sb;
        private bool m_bDirty;
        private System.IO.FileSystemWatcher m_Watcher_toTmp;
        private System.IO.FileSystemWatcher m_Watcher_toDest;
        private bool m_bIsWatching;
        private WindowsImpersonationContext impContext = null;

        public frmNotifier()
        {


            InitializeComponent();

            m_Sb = new StringBuilder();
            m_bDirty = false;
            m_bIsWatching = false;
            settings = new Settings();
            serviceUtility = new ServiceUtility(settings);
        }

        private void btnWatchFile_Click(object sender, EventArgs e)
        {
            if (m_bIsWatching)
            {
                
                m_bIsWatching = false;
                
                //to tmp
                m_Watcher_toTmp.EnableRaisingEvents = false;
                m_Watcher_toTmp.Dispose();                
                //to dest
                m_Watcher_toDest.EnableRaisingEvents = false;
                m_Watcher_toDest.Dispose();

                btnWatchFile.BackColor = Color.LightSkyBlue;
                btnWatchFile.Text = "Start Watching";

            }
            else
            {
                m_bIsWatching = true;
                btnWatchFile.BackColor = Color.Red;
                btnWatchFile.Text = "Stop Watching";

                //to tmp
                m_Watcher_toTmp = new System.IO.FileSystemWatcher();
                //to dest
                m_Watcher_toDest = new System.IO.FileSystemWatcher();

                if (rdbDir.Checked)
                {
                    //to tmp
                    m_Watcher_toTmp.Filter = "*.*";
                   // m_Watcher_toTmp.Path = txtSrcPath.Text + "\\";
                    PathSetter(ref m_Watcher_toTmp, txtSrcPath.Text , "src");
                    
                    //to dest
                    m_Watcher_toDest.Filter = "*.*";
                    //m_Watcher_toDest.Path = txtTmpDir.Text + "\\";
                    PathSetter(ref m_Watcher_toDest, txtTmpDir.Text , "tmp");
                    //try
                    //{
                    //    m_Watcher_toDest.Path = txtTmpDir.Text + "\\";
                    //}
                    //catch (ArgumentException ex)
                    //{                        
                    //    try
                    //    {
                    //        Directory.CreateDirectory(txtTmpDir.Text);
                    //        m_Watcher_toDest.Path = txtTmpDir.Text + "\\";

                    //    }
                    //    catch (DirectoryNotFoundException ex2)
                    //    {
                    //        MessageBox.Show("Temp Directory Does not Exist. Alternative Folder will be created in " + Environment.CurrentDirectory + "\\tmp");
                    //        Directory.CreateDirectory(Environment.CurrentDirectory + "\\tmp");
                    //        m_Watcher_toDest.Path = Environment.CurrentDirectory + "\\tmp";
                    //        txtTmpDir.Text = Environment.CurrentDirectory + "\\tmp";
                    //        settings.TempFolder = Environment.CurrentDirectory + "\\tmp";
                    //        settings.SaveToDisk();
                    //    }
                       
                    //}
                }
                else
                {
                    //to tmp
                    m_Watcher_toTmp.Filter = txtSrcPath.Text.Substring(txtSrcPath.Text.LastIndexOf('\\') + 1);
                    //m_Watcher_toTmp.Path = txtSrcPath.Text.Substring(0, txtSrcPath.Text.Length - m_Watcher_toTmp.Filter.Length);
                    PathSetter(ref m_Watcher_toTmp, txtSrcPath.Text.Substring(0, txtSrcPath.Text.Length - m_Watcher_toTmp.Filter.Length), "src");
                    //to dest
                    m_Watcher_toDest.Filter = txtTmpDir.Text.Substring(txtTmpDir.Text.LastIndexOf('\\') + 1);
                    //m_Watcher_toDest.Path = txtTmpDir.Text.Substring(0, txtTmpDir.Text.Length - m_Watcher_toDest.Filter.Length);
                    PathSetter(ref m_Watcher_toDest, txtTmpDir.Text.Substring(0, txtTmpDir.Text.Length - m_Watcher_toDest.Filter.Length), "tmp");

                }

                if (chkSubFolder.Checked)
                {
                    //to tmp
                    m_Watcher_toTmp.IncludeSubdirectories = true;

                    //to dest
                    m_Watcher_toDest.IncludeSubdirectories = true;
                }

                //to tmp
                m_Watcher_toTmp.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
                m_Watcher_toTmp.Created += new FileSystemEventHandler(OnChangedToTmp);
                m_Watcher_toTmp.Changed += new FileSystemEventHandler(OnChangedToTmp);

                //to dest
                m_Watcher_toDest.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
                m_Watcher_toDest.Created += new FileSystemEventHandler(OnChangedToDest);
                m_Watcher_toDest.Changed += new FileSystemEventHandler(OnChangedToDest);


                //m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
                //m_Watcher.Deleted += new FileSystemEventHandler(OnChanged);
                //m_Watcher.Renamed += new RenamedEventHandler(OnRenamed);
                //m_Watcher.Renamed += new RenamedEventHandler(OnChanged);
                
                //to tmp
                m_Watcher_toTmp.EnableRaisingEvents = true;
                //to dest
                m_Watcher_toDest.EnableRaisingEvents = true;
            }
        }

        public void PathSetter(ref FileSystemWatcher fsw, string requiredPath, string altFolderName)
        {
            
            try
            {
                fsw.Path = requiredPath;
            }
            catch (ArgumentException ex)
            {
                try
                {
                    Directory.CreateDirectory(requiredPath);
                    fsw.Path = requiredPath + "\\";
                }
                catch (DirectoryNotFoundException ex2)
                {
                    string altPath = Environment.CurrentDirectory + "\\" + altFolderName;
                    MessageBox.Show(requiredPath + " Directory Does not Exist. Alternative Folder will be created in " + altPath);
                    Directory.CreateDirectory(altPath);
                    fsw.Path = altPath;
                    
                    
                    switch (altFolderName)
                    {
                        case "src":
                            txtSrcPath.Text = altPath;
                            settings.SourceFolder = altPath;
                            break;
                        case "tmp":
                            txtTmpDir.Text = altPath;
                            settings.TempFolder = altPath;
                            break;
                        case "dest":
                            txtDestPath.Text = altPath;
                            settings.TargetFolder = altPath;
                            break;
                    }

                    
                    settings.SaveToDisk();
                }

            }
        }

        private string _appRootDir;
        public string AppRootDir
        {
            get
            {
                if (string.IsNullOrEmpty(_appRootDir))
                {
                    string codebase = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                    _appRootDir = (Path.GetDirectoryName(codebase) + "\\").Replace("file:\\", "");
                }
                return _appRootDir;
            }
        }

        public bool IsFolder(string path)
        {
            return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
        }


        public void RunAsAdmin()
        {
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.UseShellExecute = true;
            procInfo.WorkingDirectory = Environment.CurrentDirectory;
            procInfo.FileName = Application.ExecutablePath;
            procInfo.Verb = "runas";

            try
            {
                Process.Start(procInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Process");
            }
        }

        bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        bool IsUser()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.User);
        }

        private void OnChangedToTmp(object sender, FileSystemEventArgs e)
        {
            if (IsFolder(e.FullPath))
                return;
            bool fileCopied = false;
            string copiedFilePath = "";
            if (e.ChangeType.ToString().Equals("Created") || e.ChangeType.ToString().Equals("Changed"))
            //if (e.ChangeType.ToString().Equals("Created"))
            {
                try
                {
                    string contents = "";
                    string fileName = AppRootDir + "CopiedFilesToTmpLog.txt";
                    if (File.Exists(fileName))
                    {
                        StreamReader sr = new StreamReader(fileName);
                        contents = sr.ReadToEnd();
                        sr.Close();
                    }

                    string destFileName = txtTmpDir.Text + "\\" + Path.GetFileName(e.FullPath);
                    List<string> lstFilesInDestFolder = new List<string>();
                    lstFilesInDestFolder.AddRange(Directory.GetFiles(txtTmpDir.Text));


                    bool isFile = File.Exists(e.FullPath);


                    if ((isFile && (!File.Exists(destFileName)) || !lstFilesInDestFolder.Contains(destFileName)))//(!contents.Contains(text + "\\" + Path.GetFileName(e.FullPath)))
                    {
                        WaitReady(e.FullPath);

                        File.Copy(e.FullPath, destFileName);
                        fileCopied = true;
                        copiedFilePath = destFileName;
                        StreamWriter sw = new StreamWriter(fileName, true);
                        sw.WriteLine(destFileName);
                        sw.Close();

                        if (chkDeleteAfterCopy.Checked)
                            File.Delete(e.FullPath);
                    }
                }
                catch (Exception ex)
                {

                    LogHelper.LogException(ex, "FileChangeNotifier.frmNotifier.OnChangedToTmp", LogType.Watcher);
                }
            }
            m_Sb.Remove(0, m_Sb.Length);
            m_Sb.Append(e.FullPath);
            m_Sb.Append(" ");
            m_Sb.Append(e.ChangeType.ToString());
            m_Sb.Append("    ");
            m_Sb.Append(DateTime.Now.ToString());

            if (fileCopied)
            {
                m_Sb.Append(" : Copied, Size = " + new FileInfo(copiedFilePath).Length + "Bytes");
                m_Sb.Append("    ");
            }
            m_bDirty = true;
            //}
        }



        private void OnChangedToDest(object sender, FileSystemEventArgs e)
        {
            if (IsFolder(e.FullPath))
                return;

            bool fileCopied = false;
            string copiedFilePath = "";
            if (e.ChangeType.ToString().Equals("Created") || e.ChangeType.ToString().Equals("Changed"))
            //if (e.ChangeType.ToString().Equals("Created"))
            {
                try
                {
                    string contents = "";
                    string fileName = AppRootDir + "CopiedFilesToDestLog.txt";
                    if (File.Exists(fileName))
                    {
                        StreamReader sr = new StreamReader(fileName);
                        contents = sr.ReadToEnd();
                        sr.Close();
                    }

                    string destFileName = txtDestPath.Text + "\\" + Path.GetFileName(e.FullPath);
                    List<string> lstFilesInDestFolder = new List<string>();

                    if (CheckFolder(settings.TargetFolder))
                    {
                        lstFilesInDestFolder.AddRange(Directory.GetFiles(txtDestPath.Text));
                    }
                    else
                    {
                        bool isFile = File.Exists(e.FullPath);


                        if ((isFile && (!File.Exists(destFileName)) || !lstFilesInDestFolder.Contains(destFileName)))//(!contents.Contains(text + "\\" + Path.GetFileName(e.FullPath)))
                        {
                            WaitReady(e.FullPath);

                            File.Copy(e.FullPath, destFileName);
                            fileCopied = true;
                            copiedFilePath = destFileName;
                            StreamWriter sw = new StreamWriter(fileName, true);
                            sw.WriteLine(destFileName);
                            sw.Close();

                            if (chkDeleteAfterCopy.Checked)
                                File.Delete(e.FullPath);
                        }
                    }


                }
                catch (Exception ex)
                {

                    LogHelper.LogException(ex, "FileChangeNotifier.frmNotifier.OnChangedToDest", LogType.Watcher);
                }
            }
            m_Sb.Remove(0, m_Sb.Length);
            m_Sb.Append(e.FullPath);
            m_Sb.Append(" ");
            m_Sb.Append(e.ChangeType.ToString());
            m_Sb.Append("    ");
            m_Sb.Append(DateTime.Now.ToString());

            if (fileCopied)
            {
                m_Sb.Append(" : Copied, Size = " + new FileInfo(copiedFilePath).Length + "Bytes");
                m_Sb.Append("    ");
            }
            m_bDirty = true;
            //}
        }

        /// <summary>
        /// Waits until a file can be opened with write permission
        /// </summary>
        public static void WaitReady(string fileName)
        {
            while (true)
            {
                try
                {
                    using (Stream stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (stream != null)
                        {
                            System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} ready.", fileName));
                            break;
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                    LogHelper.LogMessage(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message),
                        "FileChangeNotifier.frmNotifier.WaitReady", LogType.Watcher, TraceEventType.Error);


                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                    LogHelper.LogMessage(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message),
                        "FileChangeNotifier.frmNotifier.WaitReady", LogType.Watcher, TraceEventType.Error);

                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                    LogHelper.LogMessage(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message),
                        "FileChangeNotifier.frmNotifier.WaitReady", LogType.Watcher, TraceEventType.Error);

                }
                Thread.Sleep(2000);
            }
        }


        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            //if (!m_bDirty)
            //{
            bool fileCopied = false;
            string copiedFilePath = "";
            if (e.ChangeType.ToString().Equals("Created") || e.ChangeType.ToString().Equals("Changed"))
            {
                //string ss = Path.GetFileName(e.FullPath);
                //if (!Path.GetDirectoryName(e.FullPath).Equals(Path.GetFileName(e.FullPath)))
                try
                {
                    string temp = txtDestPath.Text + "\\" + Path.GetFileName(e.FullPath);
                    string contents = "";
                    string codebase = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                    string fileName = (Path.GetDirectoryName(codebase) + "\\CopiedFiles.txt").Replace("file:\\", "");
                    string text = txtDestPath.Text;
                    if (File.Exists(fileName))
                    {
                        StreamReader sr = new StreamReader(fileName);
                        contents = sr.ReadToEnd();
                        sr.Close();
                    }
                    if (!contents.Contains(text + "\\" + Path.GetFileName(e.FullPath)))
                    {

                        File.Copy(e.FullPath, text + "\\" + Path.GetFileName(e.FullPath));
                        fileCopied = true;
                        copiedFilePath = text + "\\" + Path.GetFileName(e.FullPath);
                        StreamWriter sw = new StreamWriter(fileName, true);
                        sw.WriteLine(text + "\\" + Path.GetFileName(e.FullPath));
                        sw.Close();
                        if (chkDeleteAfterCopy.Checked)
                            File.Delete(e.FullPath);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, "FileChangeNotifier.frmNotifier.OnRenamed", LogType.Watcher);
                }
            }
            m_Sb.Remove(0, m_Sb.Length);
            m_Sb.Append(e.FullPath);
            m_Sb.Append(" ");
            m_Sb.Append(e.ChangeType.ToString());
            m_Sb.Append("    ");
            m_Sb.Append(DateTime.Now.ToString());

            if (fileCopied)
            {
                m_Sb.Append(" : Copied, Size = " + new FileInfo(copiedFilePath).Length + "Bytes");
                m_Sb.Append("    ");
            }
            m_bDirty = true;
        }

        private void tmrEditNotify_Tick(object sender, EventArgs e)
        {
            if (m_bDirty)
            {
                lstNotification.BeginUpdate();
                lstNotification.Items.Add(m_Sb.ToString());
                lstNotification.EndUpdate();
                m_bDirty = false;
            }
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            if (rdbDir.Checked)
            {
                DialogResult resDialog = dlgOpenDir.ShowDialog();
                if (resDialog.ToString() == "OK")
                {
                    txtSrcPath.Text = dlgOpenDir.SelectedPath;
                }
            }
            //else
            //{
            //    DialogResult resDialog = dlgOpenFile.ShowDialog();
            //    if (resDialog.ToString() == "OK")
            //    {
            //        txtFile.Text = dlgOpenFile.FileName;
            //    }
            //}
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            DialogResult resDialog = dlgSaveFile.ShowDialog();
            if (resDialog.ToString() == "OK")
            {
                FileInfo fi = new FileInfo(dlgSaveFile.FileName);
                StreamWriter sw = fi.CreateText();
                foreach (string sItem in lstNotification.Items)
                {
                    sw.WriteLine(sItem);
                }
                sw.Close();
            }
        }

        private void rdbFile_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdbFile.Checked == true)
            //{
            //    chkSubFolder.Enabled = false;
            //    chkSubFolder.Checked = false;
            //}
        }

        private void rdbDir_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDir.Checked == true)
            {
                chkSubFolder.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rdbDir.Checked)
            {
                DialogResult resDialog = dlgOpenDir.ShowDialog();
                if (resDialog.ToString() == "OK")
                {
                    txtDestPath.Text = dlgOpenDir.SelectedPath;
                }
            }
        }

        private void LoadSettingsForm() 
        {
            txtSrcPath.Text = settings.SourceFolder;
            txtTmpDir.Text = settings.TempFolder;
            txtDestPath.Text = settings.TargetFolder;
            chkDeleteAfterCopy.Checked = settings.DeleteAfterCopy;
            chkSubFolder.Checked = settings.IncludeSubFolders;
        }

        private void frmNotifier_Load(object sender, EventArgs e)
        {

            settings.AppRootDir = AppRootDir;
            settings.SaveToDisk();
            ////// Here we check if the user is Administrator or basic user. 
            //if (!IsAdmin())
            //{
            //    // if the user isn't Admin then call the "RunAsAdmin()" function, to restart the app in elevation mode. 
            //    this.RunAsAdmin();
            //}
            //else
            //{
            //    // If the application is already running in elevation mode, then call the "installer" function.
            //    try
            //    {
            //        //this.StartInstallApplication();
            //        //MessageBox.Show("Register key Added Successfully");
            //        Application.Exit();
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Couldn't run as Administrator ...!");
            //    }
            //}

            rdbDir.Checked = true;
            chkSubFolder.Checked = true;
            chkSubFolder.Enabled = true;

            LoadSettingsForm();

            btnRemoveService.Enabled = false;
            btnStartService.Enabled = false;
            btnStopService.Enabled = false;

            if (serviceUtility.GetServiceStatus() != ServiceTools.ServiceState.NotFound)
            {
                btnDeployService.Enabled = false;

            }

                if (serviceUtility.GetServiceStatus() == ServiceTools.ServiceState.Stop)
                {
                    btnRemoveService.Enabled = true;
                    btnStartService.Enabled = true;
                }
                if (serviceUtility.GetServiceStatus() == ServiceTools.ServiceState.Starting
                    || serviceUtility.GetServiceStatus() == ServiceTools.ServiceState.Run)
                {
                    btnStartService.Enabled = false;
                    btnStopService.Enabled = true;
                    btnRemoveService.Enabled = true;
                }

        }

        private void btnBrowseTmpFile_Click(object sender, EventArgs e)
        {
            if (rdbDir.Checked)
            {
                DialogResult resDialog = dlgOpenDir.ShowDialog();
                if (resDialog.ToString() == "OK")
                {
                    txtTmpDir.Text = dlgOpenDir.SelectedPath;
                }
            }
        }

        private void frmNotifier_FormClosing(object sender, FormClosingEventArgs e)
        {
           // ApplyChanges();
        }

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private bool CheckFolder(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Directory: " + path + " Does not exist. " + Environment.NewLine + " No Changes Will be applied !!");
                return false;
            }
        }

        private void ApplyChanges() 
        {

            if (!CheckFolder(txtSrcPath.Text) || !CheckFolder(txtTmpDir.Text) || !CheckFolder(txtDestPath.Text))
            {
                return;
            }

            
            settings.SourceFolder = txtSrcPath.Text;
            settings.TempFolder = txtTmpDir.Text;
            settings.TargetFolder = txtDestPath.Text;
            settings.DeleteAfterCopy = chkDeleteAfterCopy.Checked;
            settings.IncludeSubFolders = chkSubFolder.Checked;

            settings.SaveToDisk();

            ServiceTools.ServiceState status = serviceUtility.GetServiceStatus() ;

            if (status == ServiceTools.ServiceState.Stop)
            {
                serviceUtility.StartService();
                MessageBox.Show("Apply Successfully");
            }
            else if ((status == ServiceTools.ServiceState.Run) || (status == ServiceTools.ServiceState.Starting))
            {
                if (serviceUtility.RestartService())
                {
                    MessageBox.Show("Apply Successfully");
                }
                else
                {
                    MessageBox.Show("NOT Apply Successfully");
                }
            }
            
            #if (!DEBUG)   
#endif 
            
        
                   
        }

        private void btnDeployService_Click(object sender, EventArgs e)
        {
#if (!DEBUG)

#else
#endif
            if (serviceUtility.GetServiceStatus() == ServiceTools.ServiceState.NotFound)
            {

                if (settings.ServiceAccount == System.ServiceProcess.ServiceAccount.User)
                {
                    long result = LsaUtility.SetRight(settings.ServiceAccountUserName, "SeServiceLogonRight");
                    if (result == 0)
                    {                       
                        LogHelper.LogMessage("Privilege added for user " + settings.ServiceAccountUserName, "FormMain", LogType.Watcher, TraceEventType.Information);
                    }
                    else
                    {
                        MessageBox.Show("Privilege not added error code:" + result);
                        LogHelper.LogMessage("Privilege not added error code:" + result, "FormMain", LogType.Watcher, TraceEventType.Error);
                    }
                }

                if (serviceUtility.DeployService())
                {
                    btnRemoveService.Enabled = true;
                    if (serviceUtility.StartService())
                    {
                        MessageBox.Show("Deployed and Started Successfully");
                        btnStartService.Enabled = false;
                        btnRemoveService.Enabled = true;
                        btnDeployService.Enabled = false;
                        btnStopService.Enabled = true;
                        settings = new Settings();
                        LoadSettingsForm();
                    }
                    else
                    {
                        MessageBox.Show("Deployed Successfully .. Not Started Successfully ..");
                        btnStartService.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("NOT Deployed Successfully");
                    btnRemoveService.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Service Already Exist");
                if (serviceUtility.GetServiceStatus() == ServiceTools.ServiceState.Stop)
                {
                    serviceUtility.StartService();
                    MessageBox.Show("Apply Successfully");
                }
            }

        }
           

        private void btnRemoveService_Click(object sender, EventArgs e)
        {
            if (serviceUtility.GetServiceStatus() != ServiceTools.ServiceState.NotFound)
            {

                if (serviceUtility.RemoveService())
                {
                    MessageBox.Show("Removed Successfully .. the application will restart to complete the removal.");
                    Application.Restart();
                }
                else
                {
                    MessageBox.Show("NOT Removed Successfully");
                }
            }
            else
            {
                MessageBox.Show("Service Does Not Exist .. cant delete till it exist !");
            }
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {

            if (serviceUtility.StartService())
            {
                MessageBox.Show("Started Successfully");
                btnStopService.Enabled = true;
                btnStartService.Enabled = false;
                settings = new Settings();
                LoadSettingsForm();
            }
            else
                MessageBox.Show("Not Started Successfully ..");
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            if (serviceUtility.StopService())
            {
                MessageBox.Show("Stoped Successfully");
                btnStopService.Enabled = false;
                btnStartService.Enabled = true;
            }
            else
                MessageBox.Show("Not Stoped Successfully ..");
        }

   
    }
}