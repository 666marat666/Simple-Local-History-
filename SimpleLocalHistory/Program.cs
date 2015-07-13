using SimpleLocalHistory.Providers;
using SimpleLocalHistory.Service.Core;
using SimpleLocalHistory.Service.Implementation;
using SimpleLocalHistory.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Windows.Forms;

namespace SimpleLocalHistory
{
    static class Program
    {
        public static SettingsProvider settings;
        static WebServiceHost host;
        public static RepositoryProvider repoProvider;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            #region Setting initialization
            settings = new SettingsProvider("settings.info");
            settings.AddSetting("PathToDiffEditor", @"D:\Program Files (x86)\WinMerge\WinMergeU.exe");
            #endregion

            #region Repository initialization
            repoProvider = new RepositoryProvider(SimpleLocalHistory.Program.settings);
            #endregion

            #region Service functionality

            host = new WebServiceHost(typeof(RepositoryService),
                                                     new Uri(settings.GetStrSettingByName("ServiceUri", "http://localhost:8181/")));

            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
            //DEBUG
            ServiceDebugBehavior sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = true;

            host.Open();
            #endregion

            #region Main App run

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainApp = new MainForm();

            mainApp.FormClosing += mainApp_FormClosing;

            Application.Run(mainApp);

            #endregion

        }

        static void mainApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            host.Close();
        }
    }
}
