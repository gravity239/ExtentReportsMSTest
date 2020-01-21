using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumCore.DriverManagement;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private IWebElement _parent;
        private string _locator;

        /// <summary>
        ///     Define the locator for web element
        /// </summary>
        public Element(string locator, IWebElement parent = null)
        {
            _locator = locator;
            _dynamicLocator = locator;
            _byLocator = GetByLocator();
            _parent = parent;
        }

        /// <summary>
        ///     Find web element using By locator.
        /// </summary>
        public Element(By locator, IWebElement parent = null)
        {
            _byLocator = locator;
            _parent = parent;
        }

        private By GetByLocator()
        {
            var body = Regex.Replace(_locator, "[\\w\\s]*=(.*)", "$1").Trim();
            var type = Regex.Replace(_locator, "([\\w\\s]*)=.*", "$1").Trim();
            switch (type)
            {
                case "css":
                    return By.CssSelector(body);
                case "id":
                    return By.Id(body);
                case "link":
                    return By.LinkText(body);
                case "xpath":
                    return By.XPath(body);
                case "text":
                    return By.XPath(string.Format("//*[contains(text(), '%s')]", body));
                case "name":
                    return By.Name(body);
                default:
                    return By.XPath(_locator);
            }
        }

        /// <summary>
        ///     Return the locator of the element.
        /// </summary>
        public By GetLocator()
        {
            return _byLocator;
        }

        /// <summary>
        ///     Formats the generic element locator definition
        /// </summary>
        /// <param name="parameters"></param>
        /// <example>
        ///     How we can replace parts of defined locator:
        ///     <code>
        /// private Element menuLink = new Element("//*[@title='{0}' and @ms.title='{1}']");
        /// menuLink.Format("info","news").Text;
        /// </code>
        /// </example>
        public Element Format(params object[] parameters)
        {
            _locator = string.Format(_dynamicLocator, parameters);
            _byLocator = GetByLocator();
            return this;
        }

        /// <summary>
        ///     Return the current web element.
        /// </summary>
        public IWebElement GetElement()
        {
            try
            {
                var wait = new WebDriverWait(DriverUtils.GetDriver(),
                    TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
                var element = _parent == null ? wait.Until(ExpectedConditions.ElementIsVisible(_byLocator)) : _parent.FindElement(_byLocator);
                return element;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("Element: '{0}' is no longer attached to the DOM", _locator);
                return GetElement();
            }

            catch(NoSuchElementException)
            {
                Console.WriteLine("Element: does not exist in the DOM");
                return GetElement();
            }
        }

        /// <summary>
        ///     Get List of web element
        /// </summary>
        /// <returns></returns>
        public List<IWebElement> GetElements()
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(),
                TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            return new List<IWebElement>(wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(_byLocator)));
        }

        /// <summary>
        ///     Click on the element.
        /// </summary>
        public void Click()
        {
            GetElement().Click();
        }

        /// <summary>
        ///     Click on the element using JavaScript executor.
        /// </summary>
        public void ClickByJs()
        {
            var js = "arguments[0].click();";
            DriverUtils.ExecuteScript(js, GetElement());
        }


        /// <summary>
        ///     Click on the element using the X and Y coordinate of the element.
        /// </summary>
        public void Click(int x, int y)
        {
            var action = new Actions(DriverUtils.GetDriver());
            action.MoveToElement(GetElement(), x, y).Click().Build().Perform();
        }

        /// <summary>
        ///     Click on the element twice.
        /// </summary>
        public void DoubleClick()
        {
            var actions = new Actions(DriverUtils.GetDriver());
            actions.DoubleClick(GetElement()).Build().Perform();
        }

        /// <summary>
        ///     Send keystrokes to the active web.
        /// </summary>
        public void SendKeys(string keys)
        {
            var element = GetElement();
            element.Clear();
            element.SendKeys(keys);
        }

        /// <summary>
        ///     Hit the enter key to submit.
        /// </summary>
        public void Submit()
        {
            GetElement().Submit();
        }

        /// <summary>
        ///     Return the inner Text of the element.
        /// </summary>
        public string GetText()
        {
            return GetElement().Text;
        }

        /// <summary>
        ///     Return the value of the specific attribute for the element.
        /// </summary>
        public string GetAttribute(string attributeName)
        {
            return GetElement().GetAttribute(attributeName);
        }


        /// <summary>
        ///     Return the value of the element.
        /// </summary>
        public string GetValue()
        {
            return GetAttribute("value");
        }

        /// <summary>
        ///     Return the Size object containing the height and width of the element.
        /// </summary>
        public Size GetSize()
        {
            return GetElement().Size;
        }

        /// <summary>
        ///     Return the width of the element.
        /// </summary>
        public int GetWidth()
        {
            return GetSize().Width;
        }

        /// <summary>
        ///     Return the height of the element.
        /// </summary>
        public int GetHeight()
        {
            return GetSize().Height;
        }

        /// <summary>
        ///     Return a Point object containing the coordinates of the upper-left corner of the element
        ///     relative to the upper-left corner of the page.
        /// </summary>
        public Point GetLocation()
        {
            return GetElement().Location;
        }

        /// <summary>
        ///     Return the x coordinate of the element.
        /// </summary>
        public int GetPointX()
        {
            return GetLocation().X;
        }

        /// <summary>
        ///     Return the y coordinate of the element.
        /// </summary>
        public int GetPointY()
        {
            return GetLocation().Y;
        }

        /// <summary>
        ///     Return the value of a CSS property of the element.
        /// </summary>
        public string GetCssValue(string propertyName)
        {
            return GetElement().GetCssValue(propertyName);
        }

        /// <summary>
        ///     Return the tag name of the element.
        /// </summary>
        public string GetTagName()
        {
            return GetElement().TagName;
        }

        /// <summary>
        ///     Set a value for the element.
        /// </summary>
        public void SetValue(string value)
        {
            try
            {
                var js = string.Format("arguments[0].value='{0}';", value);
                DriverUtils.ExecuteScript(js, GetElement());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Set a value for the specific attribute of the element.
        /// </summary>
        public void SetAttribute(string attributeName, string value)
        {
            try
            {
                var js = string.Format("arguments[0].setAttribute({0}, {1});", attributeName, value);
                DriverUtils.ExecuteScript(js, GetElement());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Returns a Boolean value indicating whether the element is displayed or not.
        /// </summary>
        public bool IsDisplayed()
        {
            try
            {
                return GetElement().Displayed;
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException || ex is NoSuchElementException || ex is ElementNotVisibleException)
                    return false;
                throw;
            }
        }

        /// <summary>
        ///     Returns a Boolean value indicating whether the element is displayed or not.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool IsDisplayed(int timeout)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns a Boolean value indicating whether the element is enabled or not.
        /// </summary>
        public bool IsEnabled()
        {
            return GetElement().Enabled;
        }

        /// <summary>
        ///     Wait until element is visible
        /// </summary>
        /// <param name="timeout"></param>
        public void WaitForVisible(int timeout)
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(), TimeSpan.FromSeconds(timeout));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(_byLocator));
        }

        /// <summary>
        ///     Wait until element is visible
        /// </summary>
        public void WaitForVisible()
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(),
                TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(_byLocator));
        }

        /// <summary>
        ///     Returns a Boolean value indicating whether the element is selected or not.
        /// </summary>
        public bool IsSelected()
        {
            return GetElement().Selected;
        }

        /// <summary>
        ///     Drags the element and drops it to the target element.
        /// </summary>
        public void DragTo(Element targetElement)
        {
            var action = new Actions(DriverUtils.GetDriver());
            action.DragAndDrop(GetElement(), targetElement.GetElement());
            action.Build().Perform();
        }

        /// <summary>
        ///     Find the element and move mouse to the middle of it.
        /// </summary>
        public void MoveToElement()
        {
            var action = new Actions(DriverUtils.GetDriver());
            action.MoveToElement(GetElement()).Build().Perform();
        }

        /// <summary>
        ///     Focus on the element.
        /// </summary>
        public void Focus()
        {
            var js = "arguments[0].focus();";
            DriverUtils.ExecuteScript(js, GetElement());
        }

        /// <summary>
        ///     Scroll the page till the element is found.
        /// </summary>
        public void ScrollToView()
        {
            var js = "arguments[0].scrollIntoView(true);";
            DriverUtils.ExecuteScript(js, GetElement());
        }

        /// <summary>
        /// Wait for element enabled, specified for Button type element
        /// </summary>
        public void WaitForElementEnable()
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(),
                TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            wait.Until(driver => driver.FindElement(_byLocator).Enabled);
        }

        /// <summary>
        /// Wait for element displayed
        /// </summary>
        public void WaitForElementDisplayed()
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(),
               TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            wait.Until(driver => driver.FindElement(_byLocator).Displayed);
        }

        /// <summary>
        /// Wait for element disappeared
        /// </summary>
        public void WaitForElementDisappeared()
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(), TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            wait.Until(driver => !driver.FindElement(_byLocator).Displayed);
        }

        /// <summary>
        /// Wait for element totally remmoved from DOM
        /// </summary>
        public void WaitForElementNotExist()
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(), TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            wait.Until(ExpectedConditions.StalenessOf(GetElement()));
        }

        /// <summary>
        /// Wait for css attribute value of element
        /// </summary>
        /// <param name="cssAttribute"></param>
        /// <param name="cssAttributeValue"></param>
        public void WaitForElementCSSAttribute(string cssAttribute, string cssAttributeValue)
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(), TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            wait.Until(driver => GetElement().GetCssValue(cssAttribute).Contains(cssAttributeValue));
        }

        /// <summary>
        /// Wait for attribute value of element 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeValue"></param>
        public void WaitForElementAttribute(string attribute, string attributeValue)
        {
            var wait = new WebDriverWait(DriverUtils.GetDriver(), TimeSpan.FromSeconds(DriverUtils.GetElementTimeOut()));
            wait.Until(driver => GetElement().GetAttribute(attribute).Contains(attributeValue));
        }

        /// <summary>
        /// Check if element is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsElementDisplayed()
        {
            try
            {
                return GetElement().Displayed;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
