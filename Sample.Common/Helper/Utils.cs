using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using AventStack.ExtentReports.MarkupUtils;
using Newtonsoft.Json;

namespace Sample.Common.Helper
{
    public static class Utils
    {
        public static string GetRandomValue(string value)
        {
            value = string.Format("{0}_{1}", value.Replace(' ', '_'), DateTime.Now.ToString("yyyyMMddhhmmss"));
            return value;
        }

        public static int GetRandomNumber(int min, int max)
        {
            Thread.Sleep(100);
            Random rnum = new Random();
            return rnum.Next(min, max);
        }
      
        public static string ReportFailureOfValidationPoints(string verifiedPoint, string expectedValue, string actualValue)
        {
            string outMessage = verifiedPoint + " - Expected Value: " + expectedValue + " ,Actual Value: " + actualValue;
            return outMessage;
        }

        public static string ReportExceptionInValidation(string verifiedPoint, Exception e)
        {
            string outMessage = verifiedPoint + " Failed With Exception - " + e.Message + " " + e.StackTrace;
            return outMessage;
        }

        public static string GetProjectPath(bool includeBinFolder = false)
        {
            //string path_old = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            string path = Directory.GetCurrentDirectory() + "\\";
            string actualPath = path;
            if (!includeBinFolder)
            {
                actualPath = path.Substring(0, path.LastIndexOf("bin"));
            }
            string projectPath = new Uri(actualPath).LocalPath; // project path of your solution
            return projectPath;
        }

        public static string ImageToBase64(string imagePath)
        {
            string base64String;
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public static string ToDescription<T>(this T val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : val.ToString();
        }

        public static string ConvertObjectToJson(this object dataObject)
        {
            return JsonConvert.SerializeObject(dataObject);
        }

        public static IMarkup MarkupJsonString(this string data)
        {
            return MarkupHelper.CreateCodeBlock(data, CodeLanguage.Json);
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
                obj = prop.GetValue(obj, null);

            return obj;
        }

        public static string ConvertListToString<T>(this List<T> list)
        {
            return string.Join(Environment.NewLine, list.ToArray());
        }

        public static string GetPropertyName(this PropertyInfo propertyInfo)
        {
            try
            {
                return propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single().DisplayName;
            }
            catch
            {
                return propertyInfo.Name;
            }
        }
    }
}
