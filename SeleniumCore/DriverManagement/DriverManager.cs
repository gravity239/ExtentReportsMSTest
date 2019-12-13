using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.DriverManagement
{
    public abstract class DriverManager
    {
        private readonly Dictionary<string, IWebDriver> _webDrivers = new Dictionary<string, IWebDriver>();
        private readonly Dictionary<string, DriverProperties> _driverProperties =
           new Dictionary<string, DriverProperties>();

        /// <summary>
        ///     Set driver properties
        /// </summary>
        /// <param name="key"></param>
        /// <param name="properties"></param>
        public void SetDriverProperties(string key, DriverProperties properties)
        {
            _driverProperties.Add(key, properties);
        }

        /// <summary>
        ///     Get driver properties
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DriverProperties GetDriverProperties(string key)
        {
            return _driverProperties[key];
        }

        /// <summary>
        ///     Create WebDriver
        /// </summary>
        /// <param name="key"></param>
        public abstract void CreateDriver(string key);


        /// <summary>
        ///     Keep web driver instance by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="driver"></param>
        public void SetWebDriver(string key, IWebDriver driver)
        {
            _webDrivers.Add(key, driver);
        }

        /// <summary>
        ///     Get web driver instance by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(string key)
        {
            return _webDrivers[key];
        }

        /// <summary>
        ///     Quit all web driver instance
        /// </summary>
        public void CloseDrivers()
        {
            foreach (var item in _webDrivers.Values) item.Quit();
            //Remote all
            _webDrivers.Clear();
            _driverProperties.Clear();
        }
    }
}
