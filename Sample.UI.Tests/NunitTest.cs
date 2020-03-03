using System;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using System.Linq;
using Sample.Common.Helper;
using System.Collections.Generic;
using System.Net;
using SeleniumCore.Utilities;
using Sample.UI.Utilities;
using SeleniumCore.DriverManagement;
using NUnit.Framework;

namespace Sample.UI.Tests
{
    [TestFixture]
    public abstract class NunitTest
    {
        //public TestContext TestContext { get; set; }
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

        //[AssemblyInitialize]
        //public static void AssemblyInitialize(TestContext testContext)
        //{
        //    Console.WriteLine("Assembly initialize: " + testContext.TestName);
        //    SetUIEnvVariables(testContext);
        //}

        [SetUp]
        public void TestInitialize()
        {

            if (TestContext.Parameters.Names.Contains("Environment"))
            {
                environment = TestContext.Parameters["Environment"].ToString();
            }

            downloadLocation = TestContext.CurrentContext.TestDirectory;
            captureLocation = TestContext.CurrentContext.TestDirectory;
            //SetUIEnvVariables(TestContext);
            SetUIEnvVariables();
            //EnvironmentDataAccess.GetTestEnvironment(TestContext.CurrentContext.Test.MethodName, environment);

            validations = new List<KeyValuePair<string, bool>>();
            string report = Utils.GetRandomValue(TestContext.CurrentContext.Test.MethodName);
            reportPath = captureLocation + "\\" + report + ".html";
            extent = ExtentReportsHelper.CreateReport(reportPath, TestContext.CurrentContext.Test.MethodName);
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


        protected static void SetUIEnvVariables()
        {
            Config.ConfigFilePath = /*FileUtils.GetParentPath()*/ AppContext.BaseDirectory + TestContext.Parameters["ConfigPath"];
            Config.Driver = TestContext.Parameters["Driver"].ToString();
            Config.MachineType = TestContext.Parameters["Mode"].ToString();
            Config.LogPath = TestContext.CurrentContext.TestDirectory;
        }

        protected void ReportResult(Status status, string reportFilePath)
        {

            if (status == Status.Pass)
            {
                // do nothing
            }

            else
            {
                string timeStamp = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
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
            ExtentReportsHelper.FlushReport();
            TestContext.AddTestAttachment(reportPath);       
        }

        [TearDown]
        public void TestCleanup()
        {
            //DriverUtils.CloseCurrent();
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                ReportResult(Status.Pass, reportPath);
                DriverUtils.CloseCurrent();
                return;
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
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
}
