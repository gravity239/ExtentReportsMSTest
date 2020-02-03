using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SampleAttribute : Attribute
    {
        private string _sampleArgument;
        public SampleAttribute(string sampleArgument)
        {
            _sampleArgument = sampleArgument;
        }

        public string GetSampleArgument()
        {
            return _sampleArgument;
        }
    }

    public static class SampleAttributeMethod
    {
        public static string GetSample(PropertyInfo propertyInfo)
        {
            try
            {
                return propertyInfo.GetCustomAttributes(typeof(SampleAttribute), true).Cast<SampleAttribute>().First().GetSampleArgument();
            }
            catch
            {
                return "";
            }
        }
    }
}
