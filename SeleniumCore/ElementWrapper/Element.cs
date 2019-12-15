using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumCore.DriverManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeleniumCore.ElementWrapper
{

    /// <summary>
    ///     Declare some basic functions of the element.
    /// </summary>
    public class Element
    {
        private By _byLocator;
        private readonly string _dynamicLocator;
        private IWebElement _element;
        private IWebElement _parent;
        private string _locator;
        private Dictionary<string, IWebElement> _strategies;

        /// <summary>
        ///     Define the locator for web element
        /// </summary>
        public Element(string locator, IWebElement parent = null)
        {
            _locator = locator;
            _dynamicLocator = locator;
            _parent = parent;
            _strategies = new Dictionary<string, IWebElement> { };
        }



    }
}
