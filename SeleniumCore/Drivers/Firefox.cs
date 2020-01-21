using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using SeleniumCore.DriverManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.Drivers
{
    public class Firefox : DriverManager
    {
        public override void CreateDriver(string key, string locationDownload = null)
        {
            var props = GetDriverProperties(key);
            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments(props.GetArguments());
            FirefoxProfile profile = new FirefoxProfile();
            foreach (var item in props.GetUserProfilePreference())
                profile.SetPreference(item.Key, item.Value.ToString());
            options.Profile = profile;
            IWebDriver _webDriver;
            if (!props.IsRemoteMode())
                _webDriver = new FirefoxDriver(options);
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
