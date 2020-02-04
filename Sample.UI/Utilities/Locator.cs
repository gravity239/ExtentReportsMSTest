using Newtonsoft.Json.Linq;
using Sample.UI.Utilities;
using SeleniumCore.DriverManagement;
using SeleniumCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.Helpers
{
    public class Locator
    {

        private static readonly Lazy<Locator> lazy = new Lazy<Locator>(() => new Locator());
        private Locator() { }

        /// <summary>
        ///     Get LoaderSelector instance
        /// </summary>
        public static Locator Instance => lazy.Value;


        /// <summary>
        ///     Load json selector file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public JObject Load(string path)
        {
            return JsonParser.CreateJsonObjectFromFile(path);
        }

        /// <summary>
        ///     Get selector of web element from json file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public string Get(JObject selectors, string locatorName, string pageName = null)
        {
            string key;
            if (Config.Driver.ToLower().Contains(DriverType.IE.ToString().ToLower()))
                key = "default";
            else
                key = Config.Driver.ToLower().Split('-')[1];
            if (!string.IsNullOrEmpty(pageName))
            {
                return (string)selectors[pageName][key][locatorName];
            }

            var stackTrace = new StackTrace();
            var method = stackTrace.GetFrame(1).GetMethod();
            pageName = method.ReflectedType.Name;
            return (string)selectors[pageName][key][locatorName];
        }
    }
}
