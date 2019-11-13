using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;


namespace Sample.Common.Helper
{
    public class ExtentReportsHelper
    {
        private static readonly Lazy<ExtentReportsHelper> lazy = new Lazy<ExtentReportsHelper>(() => new ExtentReportsHelper());

        public static ExtentReportsHelper Instance { get { return lazy.Value; } }
        private ExtentReportsHelper()
        {
        }

        private ConcurrentDictionary<string, List<ExtentTest>> _nodeConDict;

        public static ConcurrentDictionary<string, List<ExtentTest>> NodeConDict
        {
            get { return Instance._nodeConDict; }
        }
       
        private static object thisLock = new object();

        [ThreadStatic]
        public static ExtentReports extent;

        [ThreadStatic]
        private static ExtentTest test;

        [ThreadStatic]
        public static string Key;

        /// <summary>
        /// Define report location, report name and title
        /// </summary>
        /// <param name="reportPath"></param>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public static ExtentReports CreateReport(string reportPath, string reportName)
        {
            lock (thisLock)
            {
                if (extent == null)
                {
                    extent = new ExtentReports();
                    System.IO.File.Create(reportPath).Dispose();
                }
                else
                {
                    var reporterList = extent.StartedReporterList;

                    if (reporterList == null || reporterList.Count == 0 || Utils.GetPropertyValue(reporterList.Last(), "Config.DocumentTitle").ToString() != reportName)
                    {
                        extent = new ExtentReports();
                    }
                }

                var htmlReporter = new ExtentV3HtmlReporter(reportPath);
                htmlReporter.Config.ReportName = reportName;
                htmlReporter.Config.DocumentTitle = reportName;

                if (Instance._nodeConDict == null)
                    Instance._nodeConDict = new ConcurrentDictionary<string, List<ExtentTest>>();

                Key = reportName;
                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
                extent.AttachReporter(htmlReporter);
                return extent;
            }
        }

        /// <summary>
        /// push/write everything to the document.
        /// </summary>
        public static void FlushReport()
        {
            lock (thisLock)
            {
                extent.Flush();
                RemovePreviousTestInfo();
            }
        }

        /// <summary>
        /// Remove all test in for in node dictionary before creating a new test or end the test
        /// </summary>
        private static void RemovePreviousTestInfo()
        {
            var localNodeCondict = NodeConDict[Key];
            if (localNodeCondict.Any())
            {
                Instance._nodeConDict[Key] = new List<ExtentTest>();
            }
        }

        /// <summary>
        /// Create new test in Tests area
        /// </summary>
        /// <param name="testDetail"></param>
        /// <param name="testDescription"></param>
        /// <returns></returns>
        public static ExtentTest LogTest(string testDetail, string testDescription = null)
        {
            lock (thisLock)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (Instance._nodeConDict.Keys.Contains(Key))
                    RemovePreviousTestInfo();

                test = extent.CreateTest(testDetail, testDescription);

                var testDict = new ConcurrentDictionary<ExtentTest, string>();
                testDict.TryAdd(test, testDetail);
                return test;
            }

        }

        /// <summary>
        /// Create test node based on method name
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static ExtentTest CreateStepNode([CallerMemberName]string memberName = "")
        {
            lock (thisLock)
            {
                ExtentTest node = null;

                if (!Instance._nodeConDict.Keys.Contains(Key))
                {
                    Instance._nodeConDict.TryAdd(Key, new List<ExtentTest>());
                    node = test.CreateNode(memberName);
                    Instance._nodeConDict[Key].Add(node);
                }

                //ExtentReport just allows creates 3 child nodes
                else if (GetLastNode().Model.Level < 3)
                {
                    node = GetLastNode().CreateNode(memberName);
                    Instance._nodeConDict[Key].Add(node);
                }
                else
                    node = GetLastNode();

                return node;
            }
        }

        /// <summary>
        /// Create child step node. This method is used for multiple parallel thread
        /// </summary>
        /// <param name="parentStepNode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ExtentTest CreateChildStepNode(ExtentTest parentStepNode, string name, string originalKey)
        {
            lock (thisLock)
            {
                if (parentStepNode == null)
                    throw new ArgumentNullException(nameof(parentStepNode));

                Key = originalKey + name;
                Instance._nodeConDict[Key] = new List<ExtentTest> { parentStepNode };
                var childStepNode = parentStepNode.CreateNode(name);
                Instance._nodeConDict[Key].Add(childStepNode);

                return childStepNode;
            }
        }

        /// <summary>
        /// Remove all child step node created by key
        /// </summary>
        /// <param name="name"></param>
        /// <param name="originalKey"></param>
        public static void EndChildStepNode(string name, string originalKey)
        {
            lock (thisLock)
            {
                List<ExtentTest> nodes;
                Instance._nodeConDict.TryRemove(originalKey + name, out nodes);
                Key = originalKey;    
            }
        }

        /// <summary>
        /// Remove test node from the node dictionary
        /// </summary>
        /// <param name="endNode"></param>
        /// <param name="memberName"></param>
        public static void EndStepNode(ExtentTest endNode, [CallerMemberName]string memberName = "")
        {
            lock (thisLock)
            {
                if (endNode.Model.Name == memberName)
                {
                    Instance._nodeConDict[Key].Remove(endNode);
                }
            }
        }

        /// <summary>
        /// Get last test node
        /// </summary>
        /// <returns></returns>
        public static ExtentTest GetLastNode()
        {
            lock (thisLock)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (Instance._nodeConDict[Key].Count > 0)
                    return Instance._nodeConDict[Key].Last();
                else
                    return test;
            }
        }

        /// <summary>
        /// Convert screenshot to Base64 format and attach to ExtentReport 
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static MediaEntityModelProvider AttachScreenshot(string imagePath)
        {
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(Utils.ImageToBase64(imagePath)).Build();
        }
    }
}
