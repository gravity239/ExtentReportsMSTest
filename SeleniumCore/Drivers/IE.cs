using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using SeleniumCore.DriverManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.Drivers
{
    public class IE : DriverManager
    {
        public override void CreateDriver(string key, string downloadLocation = null)
        {
            var props = GetDriverProperties(key);
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.EnableNativeEvents = true;
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
            options.EnablePersistentHover = true;
            options.RequireWindowFocus = true;
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            options.IgnoreZoomLevel = true;
            options.EnsureCleanSession = true;
            options.AddAdditionalCapability(CapabilityType.IsJavaScriptEnabled, true);

            if (downloadLocation != null)
            {
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main", true);
                if (myKey != null)
                {
                    myKey.SetValue("Default Download Directory", downloadLocation);
                    myKey.Close();
                }

                myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3", true);
                if (myKey != null)
                {
                    myKey.SetValue("1803", 0);
                    myKey.Close();
                }
            }
            IWebDriver _webDriver;
            if (!props.IsRemoteMode())
                _webDriver = new InternetExplorerDriver(options);
            else
            {
                foreach (var cap in props.GetCapabilities())
                    options.AddAdditionalCapability(cap.Key, cap.Value);

                _webDriver = new RemoteWebDriver(new Uri(props.GetRemoteUrl()), options);
            }
            SetWebDriver(key, _webDriver);
        }
    }
}
