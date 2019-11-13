using Sample.Common;
using Sample.Common.Helper;
using Sample.UI.Pages.Global;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sample.Common.Helper.ExtentReportsHelper;

namespace Sample.UI.Pages
{
    public class MainPage : PageBase
    {
        private IWebDriver _driverMainPage;

        #region Locators

        static readonly By _lnkAccount = By.XPath("//a[@href='#Welcome']");

        #endregion

        #region Elements

        public IWebElement LnkAccount
        {
            get { return StableFindElement(_lnkAccount); }
        }

       

        #endregion

        #region Methods

        public MainPage(IWebDriver driver)
            : base(driver)
        {
            this._driverMainPage = driver;
        }

        [Logging]
        public KeyValuePair<string, bool> ValidateDashboardMainPageDisplayed()
        {
            var validation = new KeyValuePair<string, bool>();
            try
            {
                bool foundDashboardMainpage = CheckLinkAccountDisplayed();
                if (foundDashboardMainpage)
                    validation = SetPassValidation(GetLastNode(), ValidationMessage.ValidateDashboardMainPageDisplayed);
                else
                    validation = SetFailValidation(GetLastNode(), ValidationMessage.ValidateDashboardMainPageDisplayed);
            }
            catch (Exception e)
            {
                validation = SetErrorValidation(GetLastNode(), ValidationMessage.ValidateDashboardMainPageDisplayed, e);
            }

            return validation;
        }

        [Logging]
        public bool CheckLinkAccountDisplayed()
        {
            GetLastNode().Info("check");
            return LnkAccount.Displayed;
        }

        [Logging]
        public KeyValuePair<string, bool> ValidateTextInAlertPopup(string text)
        {
            var validation = new KeyValuePair<string, bool>();
            try
            {
                WaitUntil(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                string errorText = WebDriver.SwitchTo().Alert().Text;

                if (errorText == text)
                    validation = SetPassValidation(GetLastNode(), ValidationMessage.ValidateTextInAlertPopup);
                else
                    validation = SetFailValidation(GetLastNode(), ValidationMessage.ValidateTextInAlertPopup);
            }
            catch (UnhandledAlertException e)
            {
                validation = SetErrorValidation(GetLastNode(), ValidationMessage.ValidateTextInAlertPopup, e);
            }
            return validation;
        }

        #endregion

        private static class ValidationMessage
        {
            public static string ValidateDashboardMainPageDisplayed = "Validate That TA Dashboard Main Page Displayed";
            public static string ValidateTextInAlertPopup = "Validate Dashboard error message appears";
        }
    }
}
