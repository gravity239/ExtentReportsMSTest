using AventStack.ExtentReports;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Sample.Common.Helper;
using Sample.UI.Utilities;
using SeleniumCore.DriverManagement;
using SeleniumCore.ElementWrapper;
using SeleniumCore.Helpers;
using SeleniumCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.UI.Pages.Global
{
    public class Page
    {
        protected JObject selector;
        protected string locatorPath = FileUtils.GetBasePath() + @"\Selectors\Selector.json";
        public void LogValidation(ref List<KeyValuePair<string, bool>> validations, List<KeyValuePair<string, bool>> addedValidations)
        {
            validations.AddRange(addedValidations);
        }

        public void LogValidation(ref List<KeyValuePair<string, bool>> validations, KeyValuePair<string, bool> addedValidation)
        {
            validations.Add(addedValidation);
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
        internal static void TakeScreenshot(out string filePath, IWebElement HightLightElement = null)
        {

            string timeStamp = DateTime.Now.ToString("ddMMyyyyHHmmss");

            filePath = Config.LogPath + @"\ErrorCapture" + timeStamp + ".png";

            if (HightLightElement != null)
                ((IJavaScriptExecutor)Browser.Driver).ExecuteScript("arguments[0].style.border='3px solid red'", HightLightElement);
            try

            {

                //TODO : Add more info to the filename and suitable location to save to 

                Screenshot screenshot = ((ITakesScreenshot)DriverUtils.GetDriver()).GetScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

            }

            catch (UnhandledAlertException)
            {
                IAlert alert = DriverUtils.GetDriver().SwitchTo().Alert();
                alert.Dismiss();
                Screenshot screenshot = ((ITakesScreenshot)DriverUtils.GetDriver()).GetScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
            }

            catch (Exception e)

            {

                Console.WriteLine("TakeScreenshot encountered an error. " + e.Message);

            }



            //Console.WriteLine(callingClassName + "." + callingMethodName + " generated an error. A ScreenShot of the browser has been saved. " + filePath);

        }

    }
}
