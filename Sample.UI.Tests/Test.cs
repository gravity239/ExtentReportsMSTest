using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using System.Linq;
using Sample.Common.Helper;
using System.Collections.Generic;
using System.Net;
using SeleniumCore.Utilities;
using Sample.UI.Utilities;
using SeleniumCore.DriverManagement;

namespace Sample.UI.Tests
{
    [TestClass]
    public abstract class Test
    {
        public TestContext TestContext { get; set; }
        protected string browser = string.Empty;
        protected string environment = string.Empty;
        protected string mode = string.Empty;
        private string excelUserSourcePath = string.Empty;
        private string localTempExcelUserTargetPath = string.Empty;
        protected string captureLocation = "";

        [ThreadStatic]
        public static string downloadLocation;
        

        [ThreadStatic]
        public static List<KeyValuePair<string, bool>> validations;

        [ThreadStatic]
        public static string reportPath = string.Empty;

        [ThreadStatic]
        public static ExtentTest test;

        public ExtentReports extent;
      


        [TestInitialize]
        public void TestInitialize()
        {

            if (TestContext.Properties.Contains("Environment"))
            {
                environment = TestContext.Properties["Environment"].ToString();
            }

            downloadLocation = TestContext.TestDir;
            captureLocation = TestContext.TestDir;
            SetUIEnvVariables(TestContext);
            EnvironmentDataAccess.GetTestEnvironment(TestContext.TestName, environment);

            validations = new List<KeyValuePair<string, bool>>();
            string report = Utils.GetRandomValue(TestContext.TestName);
            reportPath = captureLocation + report + ".html";
            extent = ExtentReportsHelper.CreateReport(reportPath, TestContext.TestName);
            //extent.AddSystemInfo("Environment", TestContext.Properties["environment"].ToString());
            //extent.AddSystemInfo("Browser", TestContext.Properties["browser"].ToString());
            test = ExtentReportsHelper.LogTest("Pre-condition");

            //Create new web driver
            DriverUtils.CreateDriver(new DriverProperties(Config.ConfigFilePath,
                Config.Driver));

            DriverUtils.Maximize();
        }

        protected void NavigateToUrl(string url)
        {
            DriverUtils.GoToUrl(url);
        }


        protected void SetUIEnvVariables(TestContext testContext)
        {
            Config.ConfigFilePath = FileUtils.GetParentPath() + testContext.Properties["ConfigPath"];
            Config.Driver = (string)testContext.Properties["Driver"];
            Config.Mode = (string)testContext.Properties["Mode"];
            Config.LogPath = captureLocation;
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
                string filePath = Config.LogPath + @"\ErrorCapture" + timeStamp + ".png";
                try
                {
                    if (DriverUtils.GetDriver() != null)
                    {
                        Screenshot screenshot = ((ITakesScreenshot)DriverUtils.GetDriver()).GetScreenshot();
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
                DriverUtils.CloseCurrent();
                return;
            }
            else if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                ReportResult(Status.Fail, reportPath);
                DriverUtils.CloseCurrent();
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

    public class SampleTestMethodAttribute : DataTestMethodAttribute
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
                        Test.LogException(exception, testMethod.TestMethodName);

                    }
                    catch (Exception)
                    {
                        // do nothing
                    }

                    success = false;
                    break;
                }

                for (int i = 0; i < Test.validations.Count; i++)
                {
                    test.Info(string.Join(Environment.NewLine, Test.validations[i]));
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
                    Console.WriteLine("Error to generate extent report at " + Test.reportPath);
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
