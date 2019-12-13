using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.Utilities
{
    /// <summary>
    ///     This class used to parse Json file to objects .
    /// </summary>
    public class JsonParser
    {
        /// <summary>
        ///     Create object from json file.
        /// </summary>
        public static JObject CreateJsonObjectFromFile(string filePath)
        {
            using (var r = new StreamReader(filePath))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                return jobj;
            }
        }

        /// <summary>
        ///     Create list (string) from json values. ex: ob:[ob1:"string",ob2:"string",...].
        /// </summary>
        public static List<string> GetListValueInJson(string value)
        {
            var a = JArray.Parse(value);
            var list = new List<string>();
            foreach (var o in a.Children<JObject>()) list.AddRange(o.Properties().Select(p => (string)p.Value));

            return list;
        }

        /// <summary>
        ///     Get object from json file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public static T Get<T>(string jsonPath = null)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                var type = typeof(T);
                var instance = Activator.CreateInstance(type);
                var toInvoke = type.GetMethod("get_Path");
                jsonPath = toInvoke.Invoke(instance, null).ToString();
            }

            jsonPath = FileUtils.GetBasePath() + jsonPath;
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(jsonPath));
        }
    }
}
