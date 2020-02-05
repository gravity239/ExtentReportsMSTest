using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Sample.Common.CustomAttributes
{
   
    public class DataSource
    {
        [AttributeUsage(AttributeTargets.All)]
        public class BaseDataSourceAttribute: Attribute, ITestDataSource
        {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo)
            {
                string dataPath = "";

                //Get category collection file name from Test Property
                var properties = methodInfo.GetCustomAttributes(typeof(TestCategoryAttribute));
                foreach(TestPropertyAttribute item in properties)
                {
                    if(item.Name == "TestData")
                    {
                        dataPath = item.Value;
                        break;
                    }
                }

                //Read test collection
                List<object> collectionObject = new List<object>(); //Get data source form data path and map data to object
                foreach (var item in collectionObject)
                {
                    yield return new object[] { item };
                }
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data)
            {
                throw new NotImplementedException();
            }
        }
    }
}
