using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Configuration.Install;
using TayaIT.Trace.Log;
using TayaIT.DirectoryWatcher.Config;
using System.Diagnostics;
using System.ComponentModel;

namespace TayaIT.DirectoryWatcher
{
    [RunInstaller(true)]
    public class ServiceUtility
    {
        private Settings _settings = null;
        public ServiceUtility(Settings settings)
        {
            _settings = settings;
        }

        public bool StartService()
        {
            ServiceController service = new ServiceController(_settings.ServiceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromSeconds(_settings.ServiceTimeout);               
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.LogException(ex, "ServiceUtility.StartService", LogType.Watcher);
                return false;
            }
        }

        public bool StopService()
        {
            ServiceController service = new ServiceController(_settings.ServiceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromSeconds(_settings.ServiceTimeout);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "ServiceUtility.StopService", LogType.Watcher);
                return false;
            }
        }

        public bool RestartService()
        {
            ServiceController service = new ServiceController(_settings.ServiceName);
            try
            {
                int timeOutinMilli = _settings.ServiceTimeout * 1000;
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeOutinMilli);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeOutinMilli - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "ServiceUtility.RestartService", LogType.Watcher);
                return false;
            }
        }

        public ServiceTools.ServiceState GetServiceStatus() 
        {
            return ServiceTools.ServiceInstaller.GetServiceStatus(_settings.ServiceName);
        }

        
        public bool DeployService() 
        {
            try
            {
                Installer installer = new Installer();
                

                ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
                EventLogInstaller customEventLogInstaller;

               processInstaller.Account = _settings.ServiceAccount;

                if(processInstaller.Account == ServiceAccount.User)
                {                 
                    processInstaller.Username = _settings.ServiceAccountUserName;
                    processInstaller.Password = _settings.ServiceAccountPassword;
                }

                ServiceInstaller serviceInstaller = new ServiceInstaller();
                InstallContext Context = new System.Configuration.Install.InstallContext();
                String path = String.Format("/assemblypath={0}", _settings.ServiceExecutalePath);//String.Format("/assemblypath={0}", @"<<path of executable of window service>>");
                String[] cmdline = { path };

                Context = new System.Configuration.Install.InstallContext("", cmdline);
                serviceInstaller.Context = Context;
                serviceInstaller.DisplayName = _settings.ServiceName;
                serviceInstaller.ServiceName = _settings.ServiceName;
                serviceInstaller.Description = _settings.ServiceDescription;
                serviceInstaller.StartType = _settings.ServiceStartMode;

                //usama
                serviceInstaller.Parent = processInstaller;

                // Create an instance of 'EventLogInstaller'.
                customEventLogInstaller = new EventLogInstaller();
                // Set the 'Source' of the event log, to be created.
                customEventLogInstaller.Source = "customTayaLog";
                // Set the 'Event Log' that the source is created in.
                customEventLogInstaller.Log = "TayaApplication";
                // Add myEventLogInstaller to 'InstallerCollection'.

                //serviceInstaller.Installers.Add(customEventLogInstaller);                
                System.Collections.Specialized.ListDictionary state =  new System.Collections.Specialized.ListDictionary();
                serviceInstaller.Install(state);
                
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "ServiceUtility.DeployService", LogType.Watcher);
                return false;
            }
        }

        public bool RemoveService()
        {
            try
            {
                ServiceTools.ServiceInstaller.Uninstall(_settings.ServiceName);
                ManagedInstallerClass.InstallHelper(new string[] { "/u", _settings.ServiceExecutalePath });                
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "ServiceUtility.RemoveService", LogType.Watcher);
                return false;
            }
        }
    }
}
