using LitJson;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OVC19
{
    public class BrowserSystem : IDisposable
    {
        private ChromeDriverService service;
        private ChromeOptions options;
        private ChromeDriver driver;
        private string lastUrl;

        public BrowserSystem()
        {
            service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;

            options = new ChromeOptions();
            options.AddArgument("disable-gpu");
            //options.AddArgument("headless");
            options.AddArgument(@"--profile-directory=Profile 1");

            driver = new ChromeDriver(service, options);
        }

        public bool IsRun()
        {
            try
            {
                lastUrl = driver.Url;
                return true;
            }
            catch
            {
                try { driver?.Dispose(); } catch { }
                return false;
            }
        }

        public ChromeDriver Open(string url)
        {
            driver.Navigate().GoToUrl(url);
            return driver;
        }

        public void Dispose()
        {
            try { driver?.Dispose(); } catch { }
            try { service?.Dispose(); } catch { }
        }
    }
}
