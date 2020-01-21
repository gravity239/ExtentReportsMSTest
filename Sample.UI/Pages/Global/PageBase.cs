using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.Windows.Forms;
using Sample.Common.Helper;
using AventStack.ExtentReports;
using static Sample.Common.Helper.ExtentReportsHelper;

namespace Sample.UI.Pages.Global
{
    public abstract class PageBase
    {
        #region Entities

        [ThreadStatic]
        internal static IWebDriver WebDriver;

        internal const int longTimeout = 30;
        internal const int mediumTimeout = 15;
        internal const int shortTimeout = 5;
        internal const int sapLongTimeout = 240;
        internal const int sapShortTimeout = 60;
        #endregion

        #region Actions
        protected PageBase(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }
        protected PageBase()
        {
        }

        /// <summary>
        /// Using for special textbox which includes two input elements
        /// </summary>
        /// <param name="Box"></param>
        /// <param name="textbox"></param>
        /// <param name="value"></param>
        internal void FillTextbox(IWebElement Box, By textbox, string value)
        {
            ScrollIntoView(Box);
            Box.Click();
            for (int i = 0; i < 10; i++)
                if (StableFindElement(textbox, shortTimeout) != null)
                    break;
                else
                    Box.Click();
            
            StableFindElement(textbox).InputText(value);
        }

        internal static IWebElement StableFindElement(By by, long timeout = longTimeout)
        {
            IWebElement Element = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                try
                {
                    var wait = Browser.Wait(shortTimeout);
                    //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
                    wait.Until(driver => driver.FindElement(by).Displayed);
                    Element = WebDriver.FindElement(by);
                    break;
                }

                catch (Exception)
                {
                    // Skip exception
                }
            } while (stopwatch.ElapsedMilliseconds <= timeout * 1000);

            stopwatch.Stop();
            return Element;
        }

        internal static ReadOnlyCollection<IWebElement> StableFindElements(By by, long timeout = longTimeout)
        {
            ReadOnlyCollection<IWebElement> Elements = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                try
                {
                    var wait = Browser.Wait(shortTimeout);
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
                    Elements = WebDriver.FindElements(by);
                    break;
                }

                catch (Exception)
                {
                    // Skip exception
                }
            } while (stopwatch.ElapsedMilliseconds <= timeout * 1000);

            stopwatch.Stop();
            return Elements;
        }

        internal static IWebElement FindElement(By by, int timeout = shortTimeout)
        {
            try
            {
                return Browser.Wait(timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static void Wait(int seconds = longTimeout)
        {
            System.Threading.Thread.Sleep(seconds * 1000);
        }

        internal static T WaitUntil<T>(Func<IWebDriver, T> condition, int seconds = longTimeout)
        {
            var wait = Browser.Wait(seconds);
            var ignoredExceptions = new List<Type>() { typeof(StaleElementReferenceException),
                                                       typeof(WebDriverTimeoutException),
                                                       typeof(NoSuchElementException) };

            wait.IgnoreExceptionTypes(ignoredExceptions.ToArray());
            return wait.Until(condition);
        }

        internal static void WaitForElement(By elementDescription, long timeout = longTimeout)
        {
            var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(shortTimeout));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                try
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(elementDescription));
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(elementDescription));

                    break;
                }
                catch (Exception)
                {
                    // Skip exception
                }
            } while (stopwatch.ElapsedMilliseconds <= timeout * 1000);

            stopwatch.Stop();
        }

        internal static void WaitFor(By elementDescription)

        {
            var wait = Browser.Wait();

            wait.Until(driver => driver.FindElement(elementDescription));
        }

        internal static void WaitForElementClickable(By elementDescription, int seconds = mediumTimeout)
        {
            IWebElement myDynamicElement = (new WebDriverWait(WebDriver, TimeSpan.FromSeconds(seconds))).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(elementDescription));
        }

        internal static void WaitForElementEnable(By elementDescription, int seconds = mediumTimeout)
        {
            var wait = Browser.Wait(seconds);
            wait.Until(driver => driver.FindElement(elementDescription).Enabled);
        }

        internal static void WaitForElementDisplay(By elementDescription, int seconds = longTimeout)
        {
            var wait = Browser.Wait(seconds);

            wait.Until(driver => driver.FindElement(elementDescription).Displayed);
        }

        internal static void WaitForElementDisappear(By elementDescription, int seconds = mediumTimeout)
        {
            var wait = Browser.Wait(seconds);

            wait.Until(driver => !driver.FindElement(elementDescription).Displayed);
        }

        internal static void WaitForElementNotExist(IWebElement Element, int seconds = mediumTimeout)
        {
            var wait = Browser.Wait(seconds);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(Element));
        }

        internal static void WaitForElementCSSAttribute(IWebElement Element, string cssAttribute, string cssAttributeValue, int seconds = mediumTimeout)
        {
            var wait = Browser.Wait(seconds);

            wait.Until(driver => Element.GetCssValue(cssAttribute).Contains(cssAttributeValue));

        }

        internal static void WaitForElementAttribute(IWebElement Element, string attribute, string attributeValue, int seconds = 15)
        {
            var wait = Browser.Wait(seconds);

            wait.Until(driver => Element.GetAttribute(attribute).Contains(attributeValue));

        }
        internal static bool WaitForElementInvisible(By elementDescription, int seconds = mediumTimeout)
        {
            var wait = Browser.Wait(seconds);
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(elementDescription));
        }
        internal static bool IsElementPresent(By elementDescription)
        {
            try
            {
                WebDriver.FindElement(elementDescription);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Wait for progress appear and disappear
        /// </summary>
        /// <param name="elementDescription"></param>
        /// <param name="timeout"></param>
        internal static void WaitForLoading(By elementDescription, int timeout = mediumTimeout)
        {
            var wait = Browser.Wait(timeout);
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(elementDescription));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(elementDescription));
            }
            catch (Exception)
            {
                //skip the action
            }
        }

        internal static void WaitUntilJSReady(int timeoutSec = mediumTimeout)
        {
            var wait = Browser.Wait(timeoutSec);
            wait.Until((driver) =>
            {
                try
                {
                    string readyState = ((IJavaScriptExecutor)driver).ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException e)
                {
                    return e.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        internal static void WaitForJQueryLoad(int timeout = longTimeout)
        {
            var wait = Browser.Wait(timeout * 2);

            WaitUntilJSReady(timeout);
            //Wait JQuery until it is Ready!
            wait.Until(driver => (bool)((IJavaScriptExecutor)driver).ExecuteScript("return ((typeof(jQuery) != 'undefined') && (jQuery.active === 0));"));
            WaitUntilJSReady(timeout);
        }

        internal static void WaitForJQueryToBeActive(int timeout = longTimeout)
        {

            int numOfWait = 1;
            bool isJqueryUsed = (bool)((IJavaScriptExecutor)WebDriver).ExecuteScript("return (typeof(jQuery) != 'undefined')");
            if (isJqueryUsed)
            {
                while (true)
                {
                    // JavaScript test to verify jQuery is active or not
                    bool ajaxIsComplete = (bool)(((IJavaScriptExecutor)WebDriver).ExecuteScript("return jQuery.active == 0"));
                    if (ajaxIsComplete) break;
                    try
                    {
                        numOfWait++;
                    }
                    catch (Exception) { }
                }
            }
        }

        internal static void WaitForAngularJSLoad(int timeout = mediumTimeout)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            bool isDataLoaded = false;
            int numOfWait = 1;
            while (isDataLoaded == false)
            {
                isDataLoaded = (bool)js.ExecuteScript("return (window.angular != undefined) && (angular.element(document).injector() !== undefined) && (angular.element(document).injector().get('$http').pendingRequests.length === 0)");
                Wait(1);
                numOfWait++;
                if (numOfWait > timeout)
                    break;
            }
        }
        
        internal static void HoverElement(By by, int timeout = longTimeout)
        {
            var element = StableFindElement(by, timeout);

            Actions action = new Actions(WebDriver);
            action.MoveToElement(element);
            action.Perform();
        }
        internal static void ScrollIntoView(IWebElement Element)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)WebDriver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", Element);
        }

        internal static bool RetryingFindClick(IWebElement webElement)
        {
            bool result = false;
            int attempts = 0;
            while (attempts < 2)
            {
                try
                {
                    webElement.Click();
                    result = true;
                    break;
                }
                catch (StaleElementReferenceException)
                {  }
                attempts++;
            }
            return result;
        }

        /// <summary>
        /// Get index of table cell value
        /// </summary>
        /// <param name="TableElement"></param>
        /// <param name="cellValue"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        internal static void GetTableCellValueIndex(IWebElement TableElement, string cellValue, out int rowIndex, out int columnIndex, string colType = "td", bool equalComparison = true)
        {
            rowIndex = columnIndex = -1;
            //ReadOnlyCollection<IWebElement> rowCollection = TableElement.StableFindElements(By.TagName("tr"));
            ReadOnlyCollection<IWebElement> rowCollection = TableElement.StableFindElements(By.XPath("./tr"));
            foreach (var rowItem in rowCollection)
            {
                bool exitLoop = false;
                bool valueComparison = false;
                ReadOnlyCollection<IWebElement> colCollection = rowItem.StableFindElements(By.TagName(colType));
                foreach (var colItem in colCollection)
                {
                    if (equalComparison)
                        valueComparison = colItem.GetAttribute("innerText").Trim() == cellValue;
                    else
                        valueComparison = colItem.GetAttribute("innerText").Trim().Contains(cellValue);
                    if (valueComparison)
                    {
                        rowIndex = rowCollection.IndexOf(rowItem) + 1;
                        columnIndex = colCollection.IndexOf(colItem) + 1;
                        exitLoop = true;
                        break;
                    }
                }
                if (exitLoop == true)
                    break;
            }
        }

        /// <summary>
        /// Get value of table cell by index
        /// </summary>
        /// <param name="TableElement"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="colType"></param>
        /// <returns></returns>
        internal static string GetTableCellValueByIndex(IWebElement TableElement, int rowIndex, int colIndex, string colType = "td")
        {
            return TableElement.FindElement(By.XPath(".//tr[" + rowIndex + "]/" + colType + "[" + colIndex + "]")).Text;
        }

        /// <summary>
        /// Identify TableCell by cell value
        /// </summary>
        /// <param name="TableElement"></param>
        /// <param name="cellValue"></param>
        /// <returns></returns>
        internal static IWebElement TableCell(IWebElement TableElement, string cellValue, string colType = "td", long timeout = longTimeout)
        {
            int i_RowNum = -1;
            int i_ColNum = -1;

            GetTableCellValueIndex(TableElement, cellValue, out i_RowNum, out i_ColNum);
            if (i_RowNum == -1 || i_ColNum == -1)
                return null;
            else
                return TableElement.StableFindElement(By.XPath(".//tr[" + i_RowNum + "]/" + colType + "[" + i_ColNum + "]"), timeout);
        }

        /// <summary>
        /// Identify TableCell by index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        internal static IWebElement TableCell(IWebElement TableElement, int rowIndex, int colIndex, string colType = "td", long timeout = longTimeout)
        {
            try
            {
                return TableElement.StableFindElement(By.XPath(".//tr[" + rowIndex + "]/" + colType + "[" + colIndex + "]"), timeout);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get number of rows of a table 
        /// </summary>
        /// <param name="TableElement"></param>
        /// <returns></returns>
        internal static int GetTableRowNumber(IWebElement TableElement)
        {
            try
            {
                return TableElement.StableFindElements(By.TagName("tr")).Count;
            }

            catch
            {
                return 0;
            }
        }

        internal static int GetTableColumnNumber(IWebElement TableElement, string colType = "td")
        {
            return TableElement.StableFindElements(By.TagName(colType)).Count;

        }

        /// <summary>
        /// Select an item from a customized combobox by text value
        /// </summary>
        /// <param name="Combobox">Combobox element</param>
        /// <param name="data">Dataset locator, usually is "//ul/li"</param>
        /// <param name="value">text value of item</param>
        internal static void SelectComboboxByText(IWebElement Combobox, By data, string value, int timeout = mediumTimeout, bool isEqual = true)
        {

            Combobox.ClickOnElement();
            WaitForElement(data, timeout);
            List<IWebElement> items = GetElementsWithSize(data, 1);

            if (isEqual)
            {
                foreach (IWebElement item in items)
                {
                    if (item.Text.Trim().Equals(value) || item.GetAttribute("innerHTML").Trim().Equals(value))
                    {
                        item.ClickOnElement();
                        break;
                    }
                }
            }
            else
                foreach (IWebElement item in items)
                {
                    if (item.Text.Trim().Contains(value) || item.GetAttribute("innerHTML").Trim().Contains(value))
                    {
                        item.ClickOnElement();
                        break;
                    }
                }
        }

        internal static void SelectFilterByText(IWebElement Combobox, By data, string value, int timeout = mediumTimeout, bool isEqual = true)
        {

            Combobox.ActionsClick();

            WaitForElement(data, timeout);
            List<IWebElement> items = GetElementsWithSize(data, 1);

            if (isEqual)
            {
                foreach (IWebElement item in items)
                {
                    if (item.Text.Trim().Equals(value) || item.GetAttribute("innerHTML").Trim().Equals(value))
                    {
                        item.ClickOnElement();
                        break;
                    }
                }
            }
            else
                foreach (IWebElement item in items)
                {
                    if (item.Text.Trim().Contains(value) || item.GetAttribute("innerHTML").Trim().Contains(value))
                    {
                        item.ClickOnElement();
                        break;
                    }
                }
        }

        /// <summary>
        /// Select random an item from a customized combobox by text value
        /// </summary>
        /// <param name="Combobox">Combobox element</param>
        /// <param name="data">Dataset locator, usually is "//ul/li"</param>
        /// <param name="value">text value of item</param>
        public string SelectRandomComboboxByText(IWebElement Combobox, By data, int timeout = mediumTimeout)
        {
            Combobox.Click();
            WaitForElement(data, timeout);
            List<IWebElement> Items = GetElementsWithSize(data, 1);

            IWebElement ElementRandom = Items[Utils.GetRandomNumber(0, Items.Count())];
            string selectedItem = ElementRandom.GetAttribute("innerHTML").Trim();

            ElementRandom.Click();

            return selectedItem;
        }
        
        /// <summary>
        /// Get elements by locator whenever elements' size equals or larger than the input size
        /// </summary>
        /// <param name="ElementLocator"></param>
        /// <param name="size">Size expected</param>
        /// <param name="timeoutSecond"></param>
        /// <returns>List of Elements that has size as expected</returns>
        internal static List<IWebElement> GetElementsWithSize(By ElementLocator, int size, int timeoutSecond = 30)
        {
            List<IWebElement> eles;
            Stopwatch sw = new Stopwatch();
            long timeout = timeoutSecond * 1000;
            sw.Start();
            do
            {
                eles = StableFindElements(ElementLocator).ToList();
                if (eles.Count >= size)
                    break;
            } while (sw.ElapsedMilliseconds <= timeout);
            sw.Stop();
            return eles;
        }

        internal static void Reload()
        {
            WebDriver.Navigate().Refresh();
        }

        public bool IsPageLoaded(By by)
        {
            try
            {
                WaitForElement(by);
                return true;
            }
            catch
            {
                return false;
            }
        }
                
        internal static void SwitchToNewPopUpWindow(IWebElement ElementToBeClicked, out string parentWindow, bool closePreviousWindow = false, bool doubleClick = false, int timeout = 30)
        {
            parentWindow = WebDriver.CurrentWindowHandle;
            ReadOnlyCollection<string> originalHandles = WebDriver.WindowHandles;

            try
            {
                if (doubleClick == true)
                {
                    ScrollIntoView(ElementToBeClicked);
                    ElementToBeClicked.ClickTwice();
                }
                else
                    ElementToBeClicked.ActionsClick();

                string popUpWindowHandle = Browser.Wait(timeout).Until<string>((d) =>
                {
                    string foundHandle = null;

                    // Subtract out the list of known handles. In the case of a single
                    // popup, the newHandles list will only have one value.
                    List<string> newHandles = WebDriver.WindowHandles.Except(originalHandles).ToList();
                    if (newHandles.Count > 0)
                    {
                        foundHandle = newHandles[0];
                    }

                    return foundHandle;
                });
                if (closePreviousWindow == true)
                {
                    Browser.Close();
                }
                WebDriver.SwitchTo().Window(popUpWindowHandle);
                WebDriver.Manage().Window.Maximize();
            }
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        internal static void SwitchToPopUpWindowByTitle(IWebElement ElementToBeClicked, string windowTitle)
        {
            string winHandleBefore = WebDriver.CurrentWindowHandle;
            int previousWinCount = WebDriver.WindowHandles.Count;
            
            // Perform the action to open a new Window
            ElementToBeClicked.ClickOnElement();

            WaitUntil(driver => driver.WindowHandles.Count != previousWinCount);

            var normalPageLoadTime = WebDriver.Manage().Timeouts().PageLoad;
            WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(shortTimeout);
            foreach (string handle in WebDriver.WindowHandles)
            {
                if (handle != winHandleBefore)
                {
                    try
                    {
                        WebDriver.SwitchTo().Window(handle);
                        Console.WriteLine("CurrentWindow:" + WebDriver.Title);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(handle);
                        Console.WriteLine(e);
                    }
                    if (WebDriver.Title.Contains(windowTitle))
                        break;
                }

            }
            WebDriver.Manage().Timeouts().PageLoad = normalPageLoadTime;
            //string currHandle = WebDriver.CurrentWindowHandle;
        }

        internal static void OpenURLInNewTab(string url, out string parentWindow, int timeout = 30)
        {
            parentWindow = WebDriver.CurrentWindowHandle;
            ((IJavaScriptExecutor)WebDriver).ExecuteScript("window.open('" + url + "','_blank');");

            string[] listTabs = WebDriver.WindowHandles.ToArray();
            WebDriver.SwitchTo().Window(listTabs[listTabs.Count() - 1]);
            WebDriver.Manage().Window.Maximize();
        }

        internal static void SelectDateOnCalendar(string dateString, IWebElement DatePicker, By calendarLocator)
        {
            //Format of dateString: "MM/dd/yyyy"
            string[] monthList = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            string _dayButtonXPath = ".//td[not(contains(@class,'k-other-month'))]/a[text()='{0}']";
            By _displayMonthYear = By.XPath(".//div[contains(@class,'RadCalendarPopup')]//td[@class='rcTitle']");

            string calMonth = null;
            string calYear = null;
            int jumMonthBy = 0;

            //parse the target date
            string[] parts = dateString.Split('/');
            int expMonth = int.Parse(parts[0]);
            int expDay = int.Parse(parts[1]);
            int expYear = int.Parse(parts[2]);

            //open the calendar
            DatePicker.Click();

            IWebElement CalendarPopup = StableFindElement(calendarLocator);
            //WaitForElementAttribute(CalendarPopup, "style", "block");

            //Retrieve current selected month / year name from date picker popup
            IWebElement DispMonthYear = StableFindElement(_displayMonthYear);
            calMonth = DispMonthYear.Text.Split(' ')[0];
            calYear = DispMonthYear.Text.Split(' ')[1];

            //Calculate the differ month
            jumMonthBy = ((expYear - int.Parse(calYear)) * 12) + (expMonth - (Array.IndexOf(monthList, calMonth) + 1));

            if (jumMonthBy > 0)
            {
                while (jumMonthBy-- > 0) StableFindElement(By.XPath(".//div[contains(@class,'RadCalendarPopup')]//a[contains(@class,'rcNext')]")).Click();
            }

            if (jumMonthBy < 0)
            {
                while (jumMonthBy++ < 0) StableFindElement(By.XPath(".//div[contains(@class,'RadCalendarPopup')]//a[contains(@class,'rcPrev')]")).Click();
            }

            By dayElement = By.XPath(string.Format(_dayButtonXPath, expDay));
            IWebElement DayButton = CalendarPopup.StableFindElement(dayElement);
            DayButton.Click();

        }

        internal static KeyValuePair<string, bool> SetPassValidation(ExtentTest test, string testInfo)
        {
            test.Pass(testInfo);
            return new KeyValuePair<string, bool>(testInfo, true);
        }

        internal static KeyValuePair<string, bool> SetFailValidation(ExtentTest test, string testInfo, string expectedValue = null, string actualValue = null)
        {
            if (expectedValue == null)
            {
                test.Fail(testInfo, AttachScreenshot(GetCaptureScreenshot()));
                return new KeyValuePair<string, bool>(testInfo, false);
            }
            else
            {
                test.Fail(Utils.ReportFailureOfValidationPoints(testInfo, expectedValue, actualValue), AttachScreenshot(GetCaptureScreenshot()));
                return new KeyValuePair<string, bool>(Utils.ReportFailureOfValidationPoints(testInfo, expectedValue, actualValue), false);
            }
        }

        internal static KeyValuePair<string, bool> SetErrorValidation(ExtentTest test, string testInfo, Exception exception)
        {
            test.Error(Utils.ReportExceptionInValidation(testInfo, exception), AttachScreenshot(GetCaptureScreenshot()));
            return new KeyValuePair<string, bool>(Utils.ReportExceptionInValidation(testInfo, exception), false);
        }

        internal static MediaEntityModelProvider AttachScreenshot(string imagePath)
        {
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(Utils.ImageToBase64(imagePath)).Build();
        }

        internal static string GetCaptureScreenshot(IWebElement HightLightElement = null)
        {
            string screenshotFilePath;
            TakeScreenshot(out screenshotFilePath, HightLightElement);
            return screenshotFilePath;
        }

        internal static void UploadFiles(string filePath, string fileNames)
        {
            Wait(shortTimeout / 2);
            SendKeys.SendWait(@filePath);
            SendKeys.SendWait(@"{Enter}");
            Wait(shortTimeout / 2);
            SendKeys.SendWait(@fileNames);
            Wait(shortTimeout / 3);
            SendKeys.SendWait(@"{Enter}");
            Wait(shortTimeout / 3);
        }

        internal static void DownloadFileByIE(string fileName)
        {
            Wait(shortTimeout);
            SendKeys.SendWait("%N");
            Wait(shortTimeout / 3);
            SendKeys.SendWait("{TAB}");
            Wait(shortTimeout / 3);
            SendKeys.SendWait("{Down 2}");
            Wait(shortTimeout / 3);
            SendKeys.SendWait("{Enter}");
            Wait(shortTimeout / 3);
            SendKeys.SendWait(@fileName);
            Wait(shortTimeout / 3);
            SendKeys.SendWait(@"{Enter}");
            Wait(shortTimeout / 3);
        }

        internal static void TakeScreenshot(out string filePath, IWebElement HightLightElement = null, string captureLocation = "c:\\temp\\testresults\\")
        {

            string timeStamp = DateTime.Now.ToString("ddMMyyyyHHmmss");

            filePath = captureLocation + "ErrorCapture" + timeStamp + ".png";

            if (HightLightElement != null)
                ((IJavaScriptExecutor)Browser.Driver).ExecuteScript("arguments[0].style.border='3px solid red'", HightLightElement);
            try

            {

                //TODO : Add more info to the filename and suitable location to save to 

                Screenshot screenshot = ((ITakesScreenshot)Browser.Driver).GetScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

            }

            catch (UnhandledAlertException)
            {
                IAlert alert = WebDriver.SwitchTo().Alert();
                alert.Dismiss();
                Screenshot screenshot = ((ITakesScreenshot)Browser.Driver).GetScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
            }

            catch (Exception e)

            {

                Console.WriteLine("TakeScreenshot encountered an error. " + e.Message);

            }



            //Console.WriteLine(callingClassName + "." + callingMethodName + " generated an error. A ScreenShot of the browser has been saved. " + filePath);

        }
        public void LogValidation(ref List<KeyValuePair<string, bool>> validations, List<KeyValuePair<string, bool>> addedValidations)
        {
            validations.AddRange(addedValidations);
        }

        public T LogValidation<T>(ref List<KeyValuePair<string, bool>> validations, List<KeyValuePair<string, bool>> addedValidations)
        {
            LogValidation(ref validations, addedValidations);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }

        public void LogValidation(ref List<KeyValuePair<string, bool>> validations, KeyValuePair<string, bool> addedValidation)
        {
            validations.Add(addedValidation);
        }

        public T LogValidation<T>(ref List<KeyValuePair<string, bool>> validations, KeyValuePair<string, bool> addedValidation)
        {
            validations.Add(addedValidation);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }
    }

    #endregion
}
