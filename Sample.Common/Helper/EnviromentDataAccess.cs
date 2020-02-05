using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Common.Helper
{
    public class EnvironmentDataAccess
    {
        public static void GetTestEnvironment(string testName, string environment = "dev", string sourceFilePath = null)
        {
            if (sourceFilePath == null)
            {
                sourceFilePath = Utils.GetProjectPath() + @"Resources\DT_CommonData.xml";
            }
            //Thread.Sleep(1000);
            //string localTempTargetPath = string.Format(@"c:\\temp\\testresults\\DT_CommonData_{0}_{1}.xml", testName, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString());
            //Copy(sourceFilePath, localTempTargetPath);
            GetData(sourceFilePath, environment);
        }

        private static void CopyDataFileToTemp(string sourceFilePath, string targetFilePath)
        {
            File.Copy(sourceFilePath, targetFilePath, true);
        }

        private static void Copy(string src, string dest)
        {
            System.IO.File.Copy(src, dest);
        }

        private static void GetData(string pathFile, string environment)
        {
            XMLHandler xml = new XMLHandler().LoadXMLFile(pathFile);
            TestEnvironmentData.Email = xml.GetValueOfNodeByName(environment + "/Email");
            TestEnvironmentData.Password = xml.GetValueOfNodeByName(environment + "/Password");
            TestEnvironmentData.BaseURL = xml.GetValueOfNodeByName(environment + "/BaseURL");
            TestEnvironmentData.UIURL = xml.GetValueOfNodeByName(environment + "/UIURL");
            TestEnvironmentData.TenantId = xml.GetValueOfNodeByName(environment + "/TenantId");
            TestEnvironmentData.Audience = xml.GetValueOfNodeByName(environment + "/Audience");
            TestEnvironmentData.ClientId = xml.GetValueOfNodeByName(environment + "/ClientId");
            TestEnvironmentData.SecretId = xml.GetValueOfNodeByName(environment + "/SecretId");
            TestEnvironmentData.FunctionKey = xml.GetValueOfNodeByName(environment + "/FunctionKey");
            TestEnvironmentData.SubscriptionId = xml.GetValueOfNodeByName(environment + "/SubscriptionId");
            TestEnvironmentData.ResourceGroupName = xml.GetValueOfNodeByName(environment + "/ResourceGroupName");
        }

        public static class TestEnvironmentData
        {
            public static string Email { get; set; }
            public static string Password { get; set; }
            public static string BaseURL { get; set; }
            public static string UIURL { get; set; }
            public static string TenantId { get; set; }
            public static string Audience { get; set; }
            public static string ClientId { get; set; }
            public static string SecretId { get; set; }
            public static string FunctionKey { get; set; }
            public static string SubscriptionId { get; set; }
            public static string ResourceGroupName { get; set; }
        }
    }
}
