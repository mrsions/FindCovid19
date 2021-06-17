using LitJson;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OVC19
{
    public class Browser
    {
        public static string StripQuotes(string v)
        {
            if (string.IsNullOrWhiteSpace(v)) return v;

            char c = (char)0;
            if (v[0] == '"') c = '"';
            else if (v[0] == '\'') c = '\'';

            if (c != 0)
            {
                int indexOf = v.IndexOf(c, 1);
                if (indexOf == -1) return v;

                return v.Substring(1, indexOf - 1);
            }
            return v;
        }

        public static List<Browser> GetBrowsers()
        {
            RegistryKey browserKeys;
            browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
            if (browserKeys == null)
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            string[] browserNames = browserKeys.GetSubKeyNames();
            List<Browser> browsers = new List<Browser>();
            for (int i = 0; i < browserNames.Length; i++)
            {
                Browser browser = new Browser();
                RegistryKey browserKey = browserKeys.OpenSubKey(browserNames[i]);
                browser.Name = (string)browserKey.GetValue(null);
                RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                browser.Path = StripQuotes(browserKeyPath.GetValue(null).ToString());
                browsers.Add(browser);
                if (browser.Path != null)
                    browser.Version = FileVersionInfo.GetVersionInfo(browser.Path).FileVersion;
                else
                    browser.Version = "unknown";
            }

            Browser edgeBrowser = GetEdgeVersion();
            if (edgeBrowser != null)
            {
                browsers.Add(edgeBrowser);
            }
            return browsers;
        }

        public static Browser GetEdgeVersion()
        {
            RegistryKey edgeKey =
                Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\Schemas");
            if (edgeKey != null)
            {
                string version = StripQuotes(edgeKey.GetValue("PackageFullName").ToString());
                Match result = Regex.Match(version, "(((([0-9.])\\d)+){1})");
                if (result.Success)
                {
                    return new Browser
                    {
                        Name = "MicrosoftEdge",
                        Version = result.Value
                    };
                }
            }
            return null;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
    }
}
