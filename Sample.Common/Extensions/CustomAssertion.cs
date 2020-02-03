using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sample.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.Extensions
{
    public static class CustomAssertion
    {
        public static void ShouldBeEquivalentTo(this string actual, string expected, string because = "")
        {
            try
            {
                JToken actualJson = JToken.Parse(actual);
                JToken expectedJson = JToken.Parse(expected);
                actualJson.Should().BeEquivalentTo(expectedJson, because);
            }
            catch (JsonReaderException)
            {
                // if value is not in JSON format, compare with origin string value
                actual.Should().BeEquivalentTo(expected, because);
            }
        }

        public static void ShouldBeEquivalentTo(this object actual, object expected, string because = "")
        {
            try
            {
                JToken actualJson = JToken.Parse(actual.ToString());
                JToken expectedJson = JToken.Parse(expected.ToString());
                actualJson.Should().BeEquivalentTo(expectedJson, because);
            }
            catch (JsonReaderException)
            {
                // if value is not in JSON format, compare with origin string value
                actual.Should().BeEquivalentTo(expected, because);
            }
        }

        public static void ShouldBeEquivalentTo<T>(this string actual, object expected, string because = "")
        {
            T actualObject = JsonHandler.Default.Deserialize<T>(actual);
            actualObject.Should().BeEquivalentTo(expected, because);
        }

        public static void ShouldHaveItemBeEquivalentTo(this string actual, string expected, string itemName, string because = "")
        {
            JObject actualJson = JsonHandler.Default.Deserialize<JObject>(actual);
            string actualValue = actualJson.SelectToken($"$..{itemName}").ToString();
            actualValue.Should().ShouldBeEquivalentTo(expected, because);
        }

        public static void ShouldHaveArrayItemBeEquivalentTo(this string actual, string[] expected, string itemName, string because = "")
        {
            JObject actualJson = JsonHandler.Default.Deserialize<JObject>(actual);
            string[] actualValue = Array.Empty<string>();
            if (actualJson.SelectToken($"$..{itemName}") != null)
            {
                actualValue = actualJson.SelectToken($"$..{itemName}").ToObject<string[]>();
            }
            actualValue.Should().BeEquivalentTo(expected, because);
        }

        public static void ShouldHaveItemContain(this string actual, string expected, string itemName, string because = "")
        {
            JObject actualJson = JsonHandler.Default.Deserialize<JObject>(actual);
            string actualValue = actualJson.SelectToken($"$..{itemName}").ToString();
            actualValue.Should().Contain(expected, because);
        }

        public static void ShouldHaveItemMatchRegex(this string actual, string itemName, string regularExpression, string because = "")
        {
            JObject actualJson = JsonHandler.Default.Deserialize<JObject>(actual);
            string actualValue = actualJson.SelectToken($"$..{itemName}").ToString();
            actualValue.Should().MatchRegex(regularExpression, because);
        }

        public static void ShouldHaveItemNotBeNullOrEmpty(this string actual, string expected, string itemName, string because = "")
        {
            JObject actualJson = JsonHandler.Default.Deserialize<JObject>(actual);
            string actualValue = actualJson.SelectToken($"$..{itemName}").ToString();
            actualValue.Should().NotBeNullOrEmpty(expected, because);
        }
    }
}
