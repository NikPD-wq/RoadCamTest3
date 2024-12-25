using RoadCamTest3.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RoadCamTest3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetWebBrowserCompatiblityLevel();
            this.Loaded += MainWindow_Loaded;
            DataContext = new MainViewModel();
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string htmlFilePath = "C:/Users/PahomovND/source/repos/RoadCamTest3/RoadCamTest3/Map.html";

            
            await webView2.EnsureCoreWebView2Async(null);

            
            webView2.Source = new Uri(htmlFilePath);

            
            webView2.CoreWebView2.WebMessageReceived += (webSender, args) =>
            {
                
                MessageBox.Show("WebView2 message: " + args.WebMessageAsJson);
            };
        }


        private static void SetWebBrowserCompatiblityLevel()
        {
            string appName = "RoadMaps1";
            int lvl = 1000 * GetBrowserVersion();

            WriteCompatiblityLevel("HKEY_CURRENT_USER", appName + ".exe", lvl);
            WriteCompatiblityLevel("HKEY_CURRENT_USER", appName + ".vshost.exe", lvl);
        }

        private static void WriteCompatiblityLevel(string root, string appName, int lvl)
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(root + @"\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, lvl);
            }
            catch (Exception)
            {
            }
        }

        public static int GetBrowserVersion()
        {
            string strKeyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer";
            string[] ls = new string[] { "svcVersion", "svcUpdateVersion", "Version", "W2kVersion" };

            int maxVer = 0;
            for (int i = 0; i < ls.Length; ++i)
            {
                object objVal = Microsoft.Win32.Registry.GetValue(strKeyPath, ls[i], "0");
                string strVal = Convert.ToString(objVal);
                if (strVal != null)
                {
                    int iPos = strVal.IndexOf('.');
                    if (iPos > 0)
                        strVal = strVal.Substring(0, iPos);

                    int res = 0;
                    if (int.TryParse(strVal, out res))
                        maxVer = Math.Max(maxVer, res);
                }
            }

            return maxVer;
        }
    }
}