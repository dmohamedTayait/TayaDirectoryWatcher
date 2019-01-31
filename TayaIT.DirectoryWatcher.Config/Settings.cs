using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ServiceProcess;

namespace TayaIT.DirectoryWatcher.Config
{

    public class Settings
    {
        public Settings() 
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SettingsFilePath);

            AppRootDir = doc.SelectSingleNode("//Config/AppRootDir").InnerText;
            SourceFolder = doc.SelectSingleNode("//Config/SourceFolder").InnerText;
            TempFolder = doc.SelectSingleNode("//Config/TempFolder").InnerText;
            TargetFolder = doc.SelectSingleNode("//Config/TargetFolder").InnerText;
            DeleteAfterCopy =bool.Parse(doc.SelectSingleNode("//Config/DeleteAfterCopy").InnerText);
            IncludeSubFolders = bool.Parse(doc.SelectSingleNode("//Config/IncludeSubFolders").InnerText);

            ServiceAccount = (ServiceAccount)(Enum.Parse(typeof(ServiceAccount), doc.SelectSingleNode("//Config/Service/Account").InnerText, true));//(doc.SelectSingleNode("//Config/Service/Account").InnerText);
            ServiceName = doc.SelectSingleNode("//Config/Service/Name").InnerText;
            ServiceDescription = doc.SelectSingleNode("//Config/Service/Description").InnerText;
            ServiceAccountUserName = doc.SelectSingleNode("//Config/Service/AccountUserName").InnerText;
            ServiceAccountPassword = doc.SelectSingleNode("//Config/Service/AccountPassword").InnerText;
            ServiceRunEachInHours = double.Parse(doc.SelectSingleNode("//Config/Service/RunEachInHours").InnerText);
            ServiceTimeout = int.Parse(doc.SelectSingleNode("//Config/Service/Timeout").InnerText);
            ServiceStartMode = (ServiceStartMode)(Enum.Parse(typeof(ServiceStartMode), doc.SelectSingleNode("//Config/Service/StartMode").InnerText, true));
        }

        public void SaveToDisk()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SettingsFilePath);
            doc.SelectSingleNode("//Config/AppRootDir").InnerText = AppRootDir;
            doc.SelectSingleNode("//Config/SourceFolder").InnerText = SourceFolder;
            doc.SelectSingleNode("//Config/TempFolder").InnerText = TempFolder;
            doc.SelectSingleNode("//Config/TargetFolder").InnerText = TargetFolder;
            doc.SelectSingleNode("//Config/DeleteAfterCopy").InnerText = DeleteAfterCopy.ToString();
            doc.SelectSingleNode("//Config/IncludeSubFolders").InnerText = IncludeSubFolders.ToString();
            doc.Save(SettingsFilePath);
        }

        public string AppRootDir { set; get; }
        public string TempFolder { set; get; }
        public string SourceFolder { set; get; }
        public string TargetFolder { set; get; }
        public bool DeleteAfterCopy { set; get; }
        public bool IncludeSubFolders { set; get; }

        public ServiceAccount ServiceAccount { set; get; }
        public string ServiceName { set; get; }
        public string ServiceDescription { set; get; }

        public ServiceStartMode ServiceStartMode { set; get; }
        public string ServiceAccountUserName { set; get; }
        public string ServiceAccountPassword { set; get; }
        public double ServiceRunEachInHours { set; get; }
        public int ServiceTimeout { set; get; }

        private string _serviceExecutalePath;
        public string ServiceExecutalePath
        {
            get
            {
                if (string.IsNullOrEmpty(_serviceExecutalePath))
                    _serviceExecutalePath = CurrentDir + "\\" + ServiceName + ".exe";//Settings.xml
                return _serviceExecutalePath;
            }
        }

        private string _settingsFilePath;
        public string SettingsFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_settingsFilePath))
                    _settingsFilePath = CurrentDir + "\\" + "Settings.xml";
                return _settingsFilePath;
            }
        }



        private string _currentDir;
        public string CurrentDir 
        {
            get 
            {
                if (!string.IsNullOrEmpty(_currentDir))
                    return _currentDir;

                string codebase = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                _currentDir = (System.IO.Path.GetDirectoryName(codebase)).Replace("file:\\", "");
                return _currentDir;
            }
        }
    }
}
