using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using static Sample.Common.Helper.ExtentReportsHelper;
using Sample.Common.Helper;
using Sample.UI.Pages.Global;
using SeleniumCore.ElementWrapper;
using SeleniumCore.Helpers;
using Newtonsoft.Json.Linq;

namespace Sample.UI.Pages.Google
{
    public class Home : Page
    {
        protected string _searchTextBox;
        public Home()
        {
            //selector = Locator.Instance.Load(locatorPath);
            //_searchTextBox = Locator.Instance.Get(selector, "searchTextBox");
        }

        public Element SearchTextbox => new Element("name=q");
        //public Element SearchTextbox => new Element(_searchTextBox);
        //public Element SearchForm => new Element(Locator.Instance.Get(selector, "searchForm"));


        [Logging]
        public Home Search(string text)
        {
            GetLastNode().Info("Search for: " + text);
            SearchTextbox.SendKeys(text);

            //Child node handle illustration for multiple threads
            var currentLastNode = GetLastNode();
            List<string> list = new List<string> { "1", "2", "3", "4", "5", "6" };
            string originalKey = Key;
            Parallel.ForEach(list, new ParallelOptions { MaxDegreeOfParallelism = 4 }, i =>
            {
                var childNode = CreateChildStepNode(currentLastNode, i, originalKey);
                childNode.Info(i);
                PrintString(i);
                EndChildStepNode(i, originalKey);
            });

            return this;
        }

        [Logging]
        public void PrintString(string printItem)
        {
            GetLastNode().Info(printItem);
        }

        [Logging]
        public string GetSearchedValue()
        {
            GetLastNode().Info("Get searched value");
            return SearchTextbox.GetValue();
        }

        [Logging]
        public KeyValuePair<string, bool> ValidateSearchValue(string expectedValue)
        {
            try
            {
                string actualValue = GetSearchedValue();
                if (actualValue == expectedValue)
                    return SetPassValidation(GetLastNode(), "Validate that searched value is correct");
                else
                    return SetFailValidation(GetLastNode(), "Validate that searched value is correct", expectedValue, actualValue);
            }
            catch (Exception e)
            {
                return SetErrorValidation(GetLastNode(), "Validate that searched value is correct", e);
            }
        }
    }
}
