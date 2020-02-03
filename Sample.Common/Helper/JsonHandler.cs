using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.Helper
{
    public class JsonHandler
    {
        private JsonSerializerSettings jsonSerializerSettings;

        private static string jsonFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\TestData\\";
        internal static void WriteToJson(object obj, string testCaseName)
        {
            //Create Serializer and set its properties
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            //Write the object to a json file 
            //File name is based on the type of object, the type of test it will be used in, and a desscriptor
            //Example file name: CoolUserObject.ValidUserCanLogOnAndOff.Inputs.json or CoolUserObject.ValidUserCanLogOnAndOff.Expected.json
            using (StreamWriter sw = new StreamWriter(jsonFilePath + obj.GetType().Name + "." + testCaseName + @".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public JsonHandler(JsonSerializerSettings serializerSettings)
        {
            jsonSerializerSettings = serializerSettings;
        }

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings);
        }

        public static JsonHandler Default
        {
            get
            {
                return new JsonHandler(new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateParseHandling = DateParseHandling.None,
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore
                });
            }
        }

    }
}
