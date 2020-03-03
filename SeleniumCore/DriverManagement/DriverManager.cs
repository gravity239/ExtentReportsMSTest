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
        private readonly Dictionary<string, int> _elementTimeOut = new Dictionary<string, int>();
        private readonly int _defaultElementTimeOut = 5;

        /// <summary>
        ///     Set driver properties
        /// </summary>
        /// <param name="key"></param>
        /// <param name="properties"></param>
        public void SetDriverProperties(string key, DriverProperties properties)
        {
            if (!_driverProperties.Keys.Contains(key))
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
        public abstract void CreateDriver(string key, string downloadLocation = null);


        /// <summary>
        ///     Keep web driver instance by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="driver"></param>
        public void SetWebDriver(string key, IWebDriver driver)
        {
            SetElementTimeOut(key, _defaultElementTimeOut);
            if (!_webDrivers.Keys.Contains(key))
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

        /// <summary>
        ///     Get time out for finding web element
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetElementTimeOut(string key)
        {
            return _elementTimeOut[key];
        }

        /// <summary>
        ///     Set time out for finding web element
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        public void SetElementTimeOut(string key, int seconds)
        {
            if (!_elementTimeOut.Keys.Contains(key))
                _elementTimeOut.Add(key, seconds);
        }
    }
}
