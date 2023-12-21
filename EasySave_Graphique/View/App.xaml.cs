using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace EasySave_Graphique
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly object _lock = new object();
        private static Thread Remote;
        private static Thread Stop;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Remote = new Thread(RemoteAccess_vm.ServerConnection);
            try
            {
                Remote.Start();
            }
            catch (Exception)
            {
                Remote.Start();
            }
            // Load the language setting from the config.json file
            //_remoteAccess.ServerPart();
            string language = LoadLanguageFromConfigFile();
            
            // Set the CultureInfo of the current thread to the loaded language
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);

            // Set the Culture property of the Resources class
            EasySave_Graphique.language.Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            
            //kill the thread when the app is closed
            this.Exit += (s, e) =>
            {
                try
                {
                    Remote.Abort();
                }
                catch (Exception)
                {
                    // ignored
                }
            };

        }

        private string LoadLanguageFromConfigFile()
        {
            lock (_lock)
            {
                string configJson = File.ReadAllText("../../../config.json");
                JArray config = JArray.Parse(configJson);
                JObject languageItem = config.Children<JObject>()
                    .FirstOrDefault(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "Lang");
                if (languageItem != null)
                {
                    return languageItem["Lang"].ToString();
                }
                return "en"; // default language
            }
        }
    }
}