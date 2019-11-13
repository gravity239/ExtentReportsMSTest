using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Sample.Common.Helper;


namespace Sample.UI.Pages.Global
{
    public class LoggedInLanding : PageBase
    {

        #region Entities

        private static By _userNameLable => By.XPath("//div[@id='divUserName']/span");
        private static By _logoutLink => By.Id("LogoutLabel");
        private static By _logoutYesButton => By.XPath("//div[@class='rwDialogPopup radconfirm']//a[.//span[text()='Yes']]");
        private static By _alertPopup => By.Id("kendoAlertWindow");
        private static By _saveChangeYesButton => By.Id("Yes");
        private static By _saveChangeNoButton => By.Id("No");
        private static By _saveItemPopUp => By.XPath("//div[contains(@id,'RadWindowWrapper_alert')]");
        private static By _iframeAutoRecovery => By.XPath("//iframe[@name='RadWindowAutoRecovery']");
        private static By _recoveryCancelButton => By.Id("divCancel");
        private static By _recoveryNoButton => By.Id("divNo");


        public IWebElement LogoutLink { get { return StableFindElement(_logoutLink); } }
        public IWebElement LogoutYesButton { get { return StableFindElement(_logoutYesButton); } }
        public IWebElement SaveChangeYesButton { get { return StableFindElement(_saveChangeYesButton); } }
        public IWebElement SaveChangeNoButton { get { return StableFindElement(_saveChangeNoButton); } }
        public IWebElement IframeAutoRecovery { get { return StableFindElement(_iframeAutoRecovery); } }
        public IWebElement RecoveryCancelButton { get { return StableFindElement(_recoveryCancelButton); } }
        public IWebElement RecoveryNoButton { get { return StableFindElement(_recoveryNoButton); } }
        #endregion


        #region Actions

        public LoggedInLanding(IWebDriver webDriver) : base(webDriver)
        {
        }

        public NonSsoSignOn Logout()
        {
            LogoutLink.Click();
            WebDriver.SwitchTo().ActiveElement();
            LogoutYesButton.Click();
            return new NonSsoSignOn(WebDriver);
        }               

        public T DownloadFile<T>(string fileName)
        {
            DownloadFileByIE(fileName);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }

        public LoggedInLanding SwitchToWindow(string window, bool closePreviousWindow = false)
        {
            if (closePreviousWindow == true)
            {
                Browser.Close();
            }
            WebDriver.SwitchTo().Window(window);
            return this;
        }
       
        public LoggedInLanding LogValidation(ref List<KeyValuePair<string, bool>> validations, List<KeyValuePair<string, bool>> addedValidations)
        {
           validations.AddRange(addedValidations);
            return this;
        }

        public T LogValidation<T>(ref List<KeyValuePair<string, bool>> validations, List<KeyValuePair<string, bool>> addedValidations)
        {
            LogValidation(ref validations, addedValidations);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }

        public LoggedInLanding LogValidation(ref List<KeyValuePair<string, bool>> validations, KeyValuePair<string, bool> addedValidation)
        {
            validations.Add(addedValidation);
            return this;
        }

        public T LogValidation<T>(ref List<KeyValuePair<string, bool>> validations, KeyValuePair<string, bool> addedValidation)
        {
            validations.Add(addedValidation);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }

        #endregion
    }
}
