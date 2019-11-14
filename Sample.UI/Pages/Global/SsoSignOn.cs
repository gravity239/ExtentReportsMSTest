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
    public class SsoSignOn : NonSsoSignOn
    {
        #region Entities

        private static By _username => By.Id("userNameInput");
        private static By _password => By.Id("passwordInput");
        private static By _email => By.Id("i0116");
        private static By _nextButton => By.Id("idSIButton9");
        private static By _signInButton => By.Id("submitButton");
        private static By _denyStaySignedIn => By.Id("idBtn_Back");
        private static By _registerButton => By.Id("lnkRegister");

        public IWebElement Username { get { return StableFindElement(_username); } }
        public IWebElement Password { get { return StableFindElement(_password); } }
        public IWebElement Email { get { return StableFindElement(_email); } }
        public IWebElement NextButton { get { return StableFindElement(_nextButton); } }
        public IWebElement SignInButton { get { return StableFindElement(_signInButton); } }
        public IWebElement DenyStaySignedIn { get { return StableFindElement(_denyStaySignedIn); } }
        public IWebElement RegisterButton { get { return StableFindElement(_registerButton); } }

        #endregion


        #region Actions

        public SsoSignOn(IWebDriver webDriver) : base(webDriver)
        {
            Title = "TeamBinder";
        }
    }
    #endregion
}
