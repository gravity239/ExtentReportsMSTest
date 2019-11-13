using Sample.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sample.UI.Pages.Global.PageBase;

namespace Sample.UI
{
    public static class IWebElementExtensions
    {
        public static void InputText(this IWebElement Element, string text, bool byJS = false)
        {
            if (byJS)
            {
                try
                {
                    ((IJavaScriptExecutor)WebDriver).ExecuteScript("arguments[0].value = arguments[1];", Element, text);
                }
                catch (TimeoutException e)
                {
                    throw new Exception($"{Element.TagName} - Element not visible within timeout period - Message: {e.Message}");
                }
            }
            else
            {
                Element.SendKeys("");
                Element.Clear();
                Element.SendKeys(text);
            }
        }

        public static void Check(this IWebElement Element)
        {
            bool isChecked = Element.Selected;
            if (isChecked == false)
            {
                Element.Click();
            }
        }

        public static void UnCheck(this IWebElement Element)
        {
            bool isChecked = Element.Selected;
            if (isChecked == true)
            {
                Element.Click();
            }
        }

        public static bool IsDisplayed(this IWebElement Element)
        {
            try
            {
                return Element.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public static IWebElement StableFindElement(this IWebElement Element, By by, long timeout = longTimeout)
        {
            IWebElement Ele = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                try
                {
                    var wait = Browser.Wait(shortTimeout);
                    wait.Until(d => Element.FindElement(by).Displayed);
                    Ele = Element.FindElement(by);
                    HoverElement(Ele);
                    break;
                }

                catch (StaleElementReferenceException)
                {
                    if (FindElement(by) != null)
                        if (FindElement(by) != null)
                            try
                            {
                                if (!Element.FindElement(by).IsDisplayed())
                                    HoverElement(Element.FindElement(by));
                            }
                            catch
                            {
                                return Ele;
                            }
                }

                catch (WebDriverTimeoutException)
                {
                    if (FindElement(by) != null)
                        try
                        {
                            if (!Element.FindElement(by).IsDisplayed())
                                HoverElement(Element.FindElement(by));
                        }
                        catch
                        {
                            return Ele;
                        }     
                }

                catch (Exception)
                {
                    //skip remain exceptions
                }
            } while (stopwatch.ElapsedMilliseconds <= timeout * 1000);

            stopwatch.Stop();
            return Ele;
        }

        public static ReadOnlyCollection<IWebElement> StableFindElements(this IWebElement Element, By by, long timeout = longTimeout)
        {
            ReadOnlyCollection<IWebElement> Eles = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                try
                {
                    var wait = Browser.Wait(shortTimeout);
                    wait.Until(d => Element.FindElements(by).Count > 0);
                    Eles = Element.FindElements(by);
                    break;
                }

                catch
                {
                    //skip remain exceptions
                }
            } while (stopwatch.ElapsedMilliseconds <= timeout * 1000);

            stopwatch.Stop();
            return Eles;
        }

        public static string GetValue(this IWebElement Element)
        {
            return Element.GetAttribute("value");
        }

        public static IWebDriver SwitchToIFrameBySelector(this IWebDriver driver, By bySelector, int seconds = sapLongTimeout)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                try
                {
                    WaitForElement(bySelector);
                    driver = Browser.Wait(longTimeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt(bySelector));
                    break;
                }
                catch (Exception)
                {
                    //Skip the exception
                }

            } while (stopwatch.ElapsedMilliseconds <= seconds * 1000);
            stopwatch.Stop();
            return driver;
        }
        public static void SwitchOutOfIFrame(this IWebDriver driver)
        {
            driver.SwitchTo().DefaultContent();
        }

        public static void WaitAndClick(this IWebElement Element)
        {
            try
            {
                WebDriverWait wait = Browser.Wait(mediumTimeout);
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.className("loader")));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Element)).Click();
            }
            catch (WebDriverException)
            {
                ScrollIntoView(Element);
                Element.Click();
            }
        }

        public static void ActionsClick(this IWebElement Element)
        {
            Actions actions = new Actions(WebDriver);
            actions.MoveToElement(Element).Click(Element).Build().Perform();
        }
        public static void ClickTwice(this IWebElement Element)
        {
            Actions actions = new Actions(WebDriver);
            actions.Click(Element).Click(Element).Perform();
        }

        public static void DoubleClick(this IWebElement Element)
        {
            Actions actions = new Actions(WebDriver);
            actions.DoubleClick(Element).Perform();
        }

        //public static void ClickOnElement(this IWebElement Element)
        //{
        //    var normalPageLoadTime = WebDriver.Manage().Timeouts().PageLoad;
        //    if (Browser.Driver.GetType() == typeof(InternetExplorerDriver))
        //    {
        //        try
        //        {
        //            ScrollIntoView(Element);
        //            WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(longTimeout);
        //            ((IJavaScriptExecutor)WebDriver).ExecuteScript("arguments[0].click();", Element);
        //        }
        //        catch (WebDriverTimeoutException e)
        //        {
        //            Console.WriteLine(e.Message);
        //        }
        //        finally
        //        {
        //            WebDriver.Manage().Timeouts().PageLoad = normalPageLoadTime;
        //        }
        //    }
        //    else
        //        Element.Click();
        //}
        public static void ClickOnElement(this IWebElement Element)
        {
            var normalPageLoadTime = WebDriver.Manage().Timeouts().PageLoad;
            
            try
            {
                if (Browser.Driver.GetType() == typeof(InternetExplorerDriver))
                    ScrollIntoView(Element);
                WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(longTimeout);
                Element.Click();
            }
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                WebDriver.Manage().Timeouts().PageLoad = normalPageLoadTime;
            }
                        
        }
        public static void ClickWithHandleTimeout(this IWebElement Element)
        {
            var normalPageLoadTime = WebDriver.Manage().Timeouts().PageLoad;
            
            try
            {
                WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(shortTimeout);
                Element.Click();
            }
            catch (WebDriverException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                WebDriver.Manage().Timeouts().PageLoad = normalPageLoadTime; 
            }
        }

        public static void HoverElement(this IWebElement Element)
        {
            Actions action = new Actions(WebDriver);
            action.MoveToElement(Element);
            action.Perform();
        }
        public static void HoverAndClickWithJS(this IWebElement Element)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)WebDriver;
            Element.HoverWithJS();
            jse.ExecuteScript("arguments[0].click();", Element);
        }
        public static void HoverWithJS(this IWebElement Element)
        {
            var mouseOverScript = "if(document.createEvent){var evObj = document.createEvent('MouseEvents');"
                + "evObj.initEvent('mouseover', true, false);" 
                +"arguments[0].dispatchEvent(evObj); }"
                + "else if(document.createEventObject) { arguments[0].fireEvent('onmouseover'); }";
            ((IJavaScriptExecutor)WebDriver).ExecuteScript(mouseOverScript, Element);
        }
        public static void enableWithJS(this IWebElement Element)
        {
            ((IJavaScriptExecutor)WebDriver).ExecuteScript("arguments[0].style.display='block';",Element);
        }

        public static void SelectItem(this IWebElement element, string item, string selectby = "Text")
        {
            SelectElement selector = new SelectElement(element);
            if (selectby == "Value")
                selector.SelectByValue(item);
            else if (selectby == "Index")
                selector.SelectByIndex(int.Parse(item) - 1);
            else
                selector.SelectByText(item);
        }
    }
}
