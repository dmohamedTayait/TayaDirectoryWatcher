using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Xml;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using TayaIT.DirectoryWatcher.Config;
using TayaIT.Trace.Log;
using System.Threading;

namespace TayaIT.DirectoryWatcher.WindowsService
{
    public partial class DWManager : ServiceBase
    {

        private System.Timers.Timer _timer;
        private DateTime _lastRun = DateTime.Now;
        Settings sett = new Settings();

        private System.IO.FileSystemWatcher m_Watcher_toTmp;
        private System.IO.FileSystemWatcher m_Watcher_toDest;

        public DWManager()
        {
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            

            _timer = new System.Timers.Timer();
            
            _timer.Enabled = true;
            _timer.AutoReset = true;
            _timer.Interval = sett.ServiceRunEachInHours * 60 * 60 * 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            StartWatch();
            LogHelper.LogMessage("service started", "DWManager", LogType.Watcher, TraceEventType.Information);
        }


        public void OnStartCallTest() 
        {
            string [] args = {""};
            OnStart(args); 
        }

        public void StartWatch() 
        {
            string srcAltFolderName = "src";
            string tmpAltFolderName = "tmp";
            string destAltFolderName = "dest";

            if (!CheckFolder(sett.SourceFolder))
            {
                sett.SourceFolder = sett.AppRootDir + srcAltFolderName;
                Directory.CreateDirectory(sett.SourceFolder);
                sett.SaveToDisk();         
            }
            if(!CheckFolder(sett.TempFolder))
            {
                sett.TempFolder = sett.AppRootDir + tmpAltFolderName;
                Directory.CreateDirectory(sett.TempFolder);
                sett.SaveToDisk();
            }
            if(!CheckFolder(sett.TargetFolder))
            {
                sett.TargetFolder = sett.AppRootDir + destAltFolderName;
                Directory.CreateDirectory(sett.TargetFolder);
                sett.SaveToDisk();
            }

            //to tmp
            m_Watcher_toTmp = new System.IO.FileSystemWatcher();
            //to dest
            m_Watcher_toDest = new System.IO.FileSystemWatcher();

            if (sett.IncludeSubFolders)
            {
                //to tmp
                m_Watcher_toTmp.Filter = "*.*";
                //m_Watcher_toTmp.Path = sett.SourceFolder + "\\";
                PathSetter(ref m_Watcher_toTmp, sett.SourceFolder + "\\", "src");

                //to dest
                m_Watcher_toDest.Filter = "*.*";
                //m_Watcher_toDest.Path = sett.TempFolder + "\\";
                PathSetter(ref m_Watcher_toDest, sett.TempFolder + "\\", "tmp");
            }
            else
            {
                //to tmp
                m_Watcher_toTmp.Filter = sett.SourceFolder.Substring(sett.SourceFolder.LastIndexOf('\\') + 1);
                //m_Watcher_toTmp.Path = sett.SourceFolder.Substring(0, sett.SourceFolder.Length - m_Watcher_toTmp.Filter.Length);
                PathSetter(ref m_Watcher_toTmp, sett.SourceFolder.Substring(0, sett.SourceFolder.Length - m_Watcher_toTmp.Filter.Length), "src");

                //to dest
                m_Watcher_toDest.Filter = sett.TempFolder.Substring(sett.TempFolder.LastIndexOf('\\') + 1);
               // m_Watcher_toDest.Path = sett.TempFolder.Substring(0, sett.TempFolder.Length - m_Watcher_toDest.Filter.Length);
                PathSetter(ref m_Watcher_toDest, sett.TempFolder.Substring(0, sett.TempFolder.Length - m_Watcher_toDest.Filter.Length), "tmp");

            }

            if (sett.IncludeSubFolders)
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
                    string altPath = sett.AppRootDir + altFolderName;
                    
                    
                    LogHelper.LogMessage(requiredPath + " Directory Does not Exist. Alternative Folder will be created in " + altPath, "DWManager", LogType.Watcher, TraceEventType.Error);
                    Directory.CreateDirectory(altPath);
                    fsw.Path = altPath;


                    switch (altFolderName)
                    {
                        case "src":
                            
                            sett.SourceFolder = altPath;
                            break;
                        case "tmp":
                            
                            sett.TempFolder = altPath;
                            break;
                        case "dest":
                            
                            sett.TargetFolder = altPath;
                            break;
                    }


                    sett.SaveToDisk();
                }

            }
        }

        private bool CheckFolder(string path)
        {
            if (Directory.Exists(path))
            {                
                return true;
            }
            else
            {
                LogHelper.LogMessage("Directory: " + path + " Does not exist. " + Environment.NewLine + " Changes Will be applied to alternate location ", "DWManager", LogType.Watcher, TraceEventType.Error);             
                return false;
            }
        }

        //private string _appRootDir;
        //public string AppRootDir
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_appRootDir))
        //        {
        //            string codebase = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        //            _appRootDir = (Path.GetDirectoryName(codebase) + "\\").Replace("file:\\", "");
        //        }
        //        return _appRootDir;
        //    }
        //}

        private void OnChangedToTmp(object sender, FileSystemEventArgs e)
        {
            if (IsFolder(e.FullPath))
            {
                return;
            }
            string copiedFilePath = "";
             //if (e.ChangeType.ToString().Equals("Created") || e.ChangeType.ToString().Equals("Changed"))
            if (e.ChangeType.ToString().Equals("Created"))
            {
                try
                {
                    string contents = "";
                    string fileName = sett.AppRootDir + "CopiedFilesToTmpLog.txt";
                    if (File.Exists(fileName))
                    {
                        StreamReader sr = new StreamReader(fileName);
                        contents = sr.ReadToEnd();
                        sr.Close();
                    }

                    string destFileName = sett.TempFolder + "\\" + Path.GetFileName(e.FullPath);
                    List<string> lstFilesInDestFolder = new List<string>();
                    lstFilesInDestFolder.AddRange(Directory.GetFiles(sett.TempFolder));


                    bool isFile = File.Exists(e.FullPath);


                    if ((isFile && (!File.Exists(destFileName)) || !lstFilesInDestFolder.Contains(destFileName)))//(!contents.Contains(text + "\\" + Path.GetFileName(e.FullPath)))
                    {
                        WaitReady(e.FullPath);

                        File.Copy(e.FullPath, destFileName);
  //                      fileCopied = true;
                        copiedFilePath = destFileName;
                        StreamWriter sw = new StreamWriter(fileName, true);
                        sw.WriteLine(destFileName);
                        sw.Close();

                        if (sett.DeleteAfterCopy)
                            File.Delete(e.FullPath);
                    }
                }
                catch (Exception ex)
                {

                    LogHelper.LogException(ex, "FileChangeNotifier.frmNotifier.OnChangedToTmp", LogType.Watcher);
                }
            }

            //}
        }

        private void OnChangedToDest(object sender, FileSystemEventArgs e)
        {
            if (IsFolder(e.FullPath))
            {
                return;
            }
            string copiedFilePath = "";
            //if (e.ChangeType.ToString().Equals("Created") || e.ChangeType.ToString().Equals("Changed"))
            if (e.ChangeType.ToString().Equals("Created"))
            {
                try
                {
                    string contents = "";
                    string fileName = sett.AppRootDir + "CopiedFilesToDestLog.txt";
                    if (File.Exists(fileName))
                    {
                        StreamReader sr = new StreamReader(fileName);
                        contents = sr.ReadToEnd();
                        sr.Close();
                    }

                    string destFileName = sett.TargetFolder + "\\" + Path.GetFileName(e.FullPath);
                    List<string> lstFilesInDestFolder = new List<string>();
                    lstFilesInDestFolder.AddRange(Directory.GetFiles(sett.TargetFolder));


                    bool isFile = File.Exists(e.FullPath);


                    if ((isFile && (!File.Exists(destFileName)) || !lstFilesInDestFolder.Contains(destFileName)))//(!contents.Contains(text + "\\" + Path.GetFileName(e.FullPath)))
                    {
                        WaitReady(e.FullPath);

                        File.Copy(e.FullPath, destFileName);
//                        fileCopied = true;
                        copiedFilePath = destFileName;
                        StreamWriter sw = new StreamWriter(fileName, true);
                        sw.WriteLine(destFileName);
                        sw.Close();

                        //if (sett.DeleteAfterCopy)
                        File.Delete(e.FullPath);
                    }
                }
                catch (Exception ex)
                {

                    LogHelper.LogException(ex, "FileChangeNotifier.frmNotifier.OnChangedToDest", LogType.Watcher);
                }
            }
            //}
        }

        public static bool IsFolder(string path)
        {
            return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
        }

        /// <summary>
        /// Waits until a file can be opened with write permission
        /// </summary>
        public static void WaitReady(string fileName)
        {

            while (true)
            {
                if (!IsFolder(fileName))
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
                        //System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                        //LogHelper.LogMessage(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message),
                        //    "FileChangeNotifier.frmNotifier.WaitReady", LogType.Watcher, TraceEventType.Error);


                    }
                    catch (IOException ex)
                    {
                        //System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                        //LogHelper.LogMessage(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message),
                        //    "FileChangeNotifier.frmNotifier.WaitReady", LogType.Watcher, TraceEventType.Error);

                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        //System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                        //LogHelper.LogMessage(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message),
                        //    "FileChangeNotifier.frmNotifier.WaitReady", LogType.Watcher, TraceEventType.Error);

                    }
                    Thread.Sleep(500);
                }


            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //DOsOMEtHING();
            }
            catch (Exception exce)
            {
                LogHelper.LogException(exce, "Warcher", LogType.Watcher);
            }

            // ignore the time, just compare the date
            if (_lastRun.Date < DateTime.Now.Date)
            {
                // stop the timer while we are running the cleanup task
                _timer.Stop();
                //
                // do cleanup stuff
                //
                _lastRun = DateTime.Now;
                _timer.Start();
            }
        }


        protected override void OnStop()
        {
            //AddToFile("stopping service");
        }
        
    }    
}
