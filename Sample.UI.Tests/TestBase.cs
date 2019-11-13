using System;
using System.Diagnostics;
using System.IO;
using Sample.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using System.Linq;
using Sample.Common.Helper;
using System.Collections.Generic;
using System.Threading;
using System.Net;

namespace Sample.UI.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        protected string browser = string.Empty;
        protected string environment = string.Empty;
        private string excelUserSourcePath = string.Empty;
        private string localTempExcelUserTargetPath = string.Empty;
        protected string captureLocation = "c:\\temp\\testresults\\";

        [ThreadStatic]
        public static List<KeyValuePair<string, bool>> validations;

        [ThreadStatic]
        public static string reportPath = string.Empty;

        [ThreadStatic]
        public static ExtentTest test;

        public ExtentReports extent;
        public TestContext TestContext { get; set; }


        [TestInitialize]
        public void TestInitialize()
        {
            if (TestContext.Properties.Contains("browser"))
            {
                browser = TestContext.Properties["browser"].ToString();
            }

            if (TestContext.Properties.Contains("environment"))
            {
                environment = TestContext.Properties["environment"].ToString();
            }

            if (TestContext.Properties.Contains("headless"))
            {
                Browser.Headless = bool.Parse(TestContext.Properties["headless"].ToString());
            }

            if (!System.IO.Directory.Exists(captureLocation))

            {

                Directory.CreateDirectory(captureLocation);
            }

            validations = new List<KeyValuePair<string, bool>>();
            string report = Utils.GetRandomValue(TestContext.TestName);
            reportPath = captureLocation + report + ".html";
            extent = ExtentReportsHelper.CreateReport(reportPath, TestContext.TestName);
            extent.AddSystemInfo("Environment", TestContext.Properties["environment"].ToString());
            extent.AddSystemInfo("Browser", TestContext.Properties["browser"].ToString());
            test = ExtentReportsHelper.LogTest("Pre-condition");
        }

        protected void ReportResult(Status status, string reportFilePath)
        {

            if (status == Status.Pass)
            {
                // do nothing
            }

            else
            {
                string timeStamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string filePath = captureLocation + "ErrorCapture" + timeStamp + ".png";
                try
                {
                    if (Browser.Driver != null)
                    {
                        Screenshot screenshot = ((ITakesScreenshot)Browser.Driver).GetScreenshot();
                        screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
                        ExtentReportsHelper.GetLastNode().Info("Last screenshot", ExtentReportsHelper.AttachScreenshot(filePath));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("TakeScreenshot encountered an error. " + e.Message);
                }
            }
            TestContext.AddResultFile(reportPath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Passed)
            {
                ReportResult(Status.Pass, reportPath);
                Browser.Quit();
                return;
            }
            else if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                ReportResult(Status.Fail, reportPath);
                Browser.Quit();
                return;
            }
        }

        public static void LogException(Exception exception, string testName)
        {
            // If AssertFailedException
            if (exception == null || exception.ToString().Contains("Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"))
            {
                test.Fail(WebUtility.HtmlEncode(testName + " Failed - " + exception.Message));
            }
            else // If other exception
            {
                string errorMessage = testName + " Got Exception During Execution - " + exception.ToString();

                if (ExtentReportsHelper.GetLastNode() != null)
                {
                    ExtentReportsHelper.GetLastNode().Error(WebUtility.HtmlEncode(errorMessage));
                }
            }
        }
    }

    public class CustomTestMethodAttribute : DataTestMethodAttribute
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            bool success = true;

            TestResult[] testResults = base.Execute(testMethod);

            try
            {

                var test = ExtentReportsHelper.extent.CreateTest("Summary");
                foreach (TestResult item in testResults.Where(x => x.Outcome == UnitTestOutcome.Failed))
                {
                    try
                    {
                        var exception = item.TestFailureException.InnerException;
                        TestBase.LogException(exception, testMethod.TestMethodName);

                    }
                    catch (Exception)
                    {
                        // do nothing
                    }

                    success = false;
                    break;
                }

                for (int i = 0; i < TestBase.validations.Count; i++)
                {
                    test.Info(string.Join(Environment.NewLine, TestBase.validations[i]));
                }
                if (success)
                {
                    test.Pass(testMethod.TestMethodName + " Passed");
                }
                else
                    test.Fail(testMethod.TestMethodName + " Failed");

                ExtentReportsHelper.extent.AnalysisStrategy = AnalysisStrategy.Test;

                try
                {
                    ExtentReportsHelper.FlushReport();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace + " " + e.Message);
                    Console.WriteLine("Error to generate extent report at " + TestBase.reportPath);
                }
            }
            catch (Exception)
            {
                // do nothing
            }
            return testResults;
        }
    }
}
