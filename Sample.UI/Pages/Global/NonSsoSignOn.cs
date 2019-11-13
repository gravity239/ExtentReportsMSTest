using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Common;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Windows.Forms;

namespace Sample.UI.Pages.Global
{
    public class NonSsoSignOn : NotLoggedInLanding
    {
        #region Entities
        private static By _userIdTextbox => By.Id("txtUserId");
        private static By _companyIdTextbox => By.Id("txtCompanyId");
        private static By _passwordTextbox => By.Id("txtPassword");
        private static By _loginBtn => By.XPath("//a[@id='lnkLogon']/span[.='Login']");

        public IWebElement PasswordTextbox { get { return StableFindElement(_passwordTextbox); } }
        public IWebElement UserIdTextbox { get { return StableFindElement(_userIdTextbox); } }
        public IWebElement CompanyIdTextbox { get { return StableFindElement(_companyIdTextbox); } }
        public IWebElement LoginBtn { get { return StableFindElement(_loginBtn); } }

        #endregion


        #region Actions

        public NonSsoSignOn(IWebDriver webDriver) : base(webDriver)
        {
        }

        //public LoggedInLanding Logon(TestAccount account)
        //{
        //    string logonWindow;

        //    //Click OtherUserLogin Button and Switch to OtherUserLogin Window
        //    if (OtherUserLoginBtn == null)
        //        Browser.MinimizeWindow();
        //    Browser.MaximizeWindow();
        //    //SwitchToNewPopUpWindow(OtherUserLoginBtn, out logonWindow, true);
        //    OtherUserLoginBtn.Click();
        //    //WaitForElementDisplay(By.Id("walkme-player"));

        //    //Fill account fields
        //    UserIdTextbox.InputText(account.Username, true);
        //    WaitForElementEnable(_companyIdTextbox);
        //    CompanyIdTextbox.InputText(account.Company, true);
        //    WaitForElementEnable(_passwordTextbox);
        //    PasswordTextbox.InputText(account.Password, true);

        //    //Click LogIn button
        //    SwitchToNewPopUpWindow(LoginBtn, out logonWindow, true);

        //    var projectsListPage = new ProjectsList(WebDriver);
        //    WaitUntil(driver => projectsListPage.ProjListTitle != null);

        //    return projectsListPage;
        //}

    }
    #endregion
}
