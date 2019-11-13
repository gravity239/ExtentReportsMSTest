using AventStack.ExtentReports;
using Sample.Common.Helper;
using Sample.UI.Pages.Global;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Sample.Common.Helper.ExtentReportsHelper;

namespace Sample.UI.Pages
{
    public class Login : PageBase
    {

        #region Locators

        static readonly By _txtUsername = By.XPath("//input[@id='username']");
        static readonly By _txtPassword = By.XPath("//input[@id='password']");
        static readonly By _btnLogin = By.XPath("//div[@class='btn-login']");
        static readonly By _cmbRepo = By.XPath("//select[@id='repository']");

        #endregion

        #region Elements
        public IWebElement TxtUsername
        {
            get { return StableFindElement(_txtUsername); }
        }

        public IWebElement TxtPassword
        {
            get { return StableFindElement(_txtPassword); }
        }

        public IWebElement BtnLogin
        {
            get { return StableFindElement(_btnLogin); }
        }

        public IWebElement CmbRepo
        {
            get { return StableFindElement(_cmbRepo); }
        }

        #endregion

        #region Methods

        public Login(IWebDriver webDriver) : base(webDriver)
        {
        }

        /// <summary>
        /// Login to TA Dashboard page
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <Author>Long and Phat</Author>
        /// <returns></returns>
        [Logging]
        public MainPage SignOn(string username, string password, string repositoryName = null)
        {
            if (repositoryName != null)
            {
                CmbRepo.SelectItem(repositoryName);
            }
            //else
            //    throw new System.ArgumentException("Parameter cannot be null", "original");

            TxtUsername.SendKeys(username);
            TxtPassword.SendKeys(password);
            BtnLogin.Click();

            //Child node handle
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
            return new MainPage(WebDriver);
        }

        [Logging]
        public void PrintString(string printItem)
        {
            GetLastNode().Info(printItem);
        }

        #endregion
    }
}
