using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Diagnostics;


namespace TayaIT.DirectoryWatcher.WindowsService
{
    [RunInstaller(true)]
    public partial class DWServiceInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;
        private EventLogInstaller customEventLogInstaller;


        public DWServiceInstaller()
        {
            InitializeComponent();
            Config.Settings sett = new Config.Settings();

            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // Service will run under system account
            //check the account in config
            processInstaller.Account = sett.ServiceAccount;

            if (processInstaller.Account == ServiceAccount.User)
            {
                processInstaller.Username = sett.ServiceAccountUserName;
                processInstaller.Password = sett.ServiceAccountPassword;
            }
            

            // Service will have Start Type of Manual
            serviceInstaller.StartType = ServiceStartMode.Manual;
            //string ServiceName = Settings.ServiceName;
            this.serviceInstaller.DisplayName = sett.ServiceName;
            this.serviceInstaller.ServiceName = sett.ServiceName;

            //serviceInstaller.ServiceName = "TayaIT.DirectoryWatcher.WindowsService";

            // Create an instance of 'EventLogInstaller'.
            customEventLogInstaller = new EventLogInstaller();
            // Set the 'Source' of the event log, to be created.
            customEventLogInstaller.Source = "customLog";
            // Set the 'Event Log' that the source is created in.
            customEventLogInstaller.Log = "Application";
            // Add myEventLogInstaller to 'InstallerCollection'.
            
            Installers.Add(customEventLogInstaller);   
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);


        }

        private EventLogInstaller FindInstaller(InstallerCollection installers)
        {
            foreach (Installer installer in installers)
            {
                if (installer is EventLogInstaller)
                {
                    return (EventLogInstaller)installer;
                }

                EventLogInstaller eventLogInstaller = FindInstaller(installer.Installers);
                if (eventLogInstaller != null)
                {
                    return eventLogInstaller;
                }
            }
            return null;
        }

    }
}
