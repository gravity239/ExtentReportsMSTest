using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using SeleniumCore.DriverManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.Drivers
{
    public class Chrome : DriverManager
    {
        public override void CreateDriver(string key, string downloadLocation = null)
        {
            var props = GetDriverProperties(key);
            ChromeOptions options = new ChromeOptions();
            options.AddArguments(props.GetArguments());
            foreach (var item in props.GetUserProfilePreference())
                options.AddUserProfilePreference(item.Key, item.Value);
            if(downloadLocation != null)
                options.AddUserProfilePreference("download.default_directory", downloadLocation);
            IWebDriver _webDriver;
            if (!props.IsRemoteMode())
                _webDriver = new ChromeDriver(options);
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
