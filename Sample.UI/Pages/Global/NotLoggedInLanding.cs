using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Sample.UI.Pages.Global
{
    public class NotLoggedInLanding : PageBase
    {
        #region Entities 

        private static By _otherUserLoginBtn => By.XPath("//a[span='OTHER USER LOGIN']");
        private static By _kiewitUserLoginBtn => By.XPath("//a[./span[text()='KIEWIT USER LOGIN']]");

        public IWebElement OtherUserLoginBtn { get { return StableFindElement(_otherUserLoginBtn); } }
        public IWebElement KiewitUserLoginBtn { get { return StableFindElement(_kiewitUserLoginBtn); } }


        #endregion

        #region Actions

        public NotLoggedInLanding(IWebDriver webDriver) : base(webDriver)
        {
        }        
        #endregion
    }
}
