using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using static Sample.Common.Helper.ExtentReportsHelper;
using Sample.Common.Helper;
using Sample.UI.Pages.Global;

namespace Sample.UI.Pages
{
    public class GoogleHome: PageBase
    {
        private static By _searchTextbox => By.Name("q");
        private static By _searchForm => By.Id("searchform");

        public IWebElement SearchTextbox { get { return StableFindElement(_searchTextbox); } }
        public IWebElement SearchForm { get { return StableFindElement(_searchForm); } }

        public GoogleHome(IWebDriver webDriver) : base(webDriver)
        {
        }

        [Logging]
        public GoogleHome Search(string text)
        {
            GetLastNode().Info("Search for: " + text);
            SearchTextbox.InputText(text);

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
