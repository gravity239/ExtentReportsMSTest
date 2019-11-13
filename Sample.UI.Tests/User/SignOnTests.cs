using FluentAssertions;
using Sample.Common.Helper;
using Sample.UI.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Sample.Common.Helper.ExtentReportsHelper;

namespace Sample.UI.Tests.User
{
    [TestClass]
    public class SignOnTests : TestBase
    {

        [CustomTestMethod]
        public void TC001()
        {
            //Given
            //1. Navigate to Dashboard login page.
            test.Info("Navigate to Dashboard login page.");
            var driver = Browser.Open(Constant.HomePage, "chrome");
            //When
            //2. Enter valid username and password.
            //3. Click on "Login" button

            test = LogTest("DA_LOGIN_TC001 - Verify that user can login specific repository successfully via Dashboard login page with correct credentials.");
            Login loginPage = new Login(driver);
            MainPage mainPage = loginPage.SignOn("administrator", "");

            //Then
            //VP: Verify that Dashboard Mainpage appears
            validations.Add(mainPage.ValidateDashboardMainPageDisplayed());
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }

        [CustomTestMethod]
        public void TC002()
        {
            //Given
            //1. Navigate to Dashboard login page.
            test.Info("Navigate to Dashboard login page.");
            var driver = Browser.Open(Constant.HomePage, "chrome");
            //When
            //2. Enter valid username and password.
            //3. Click on "Login" button

            test = LogTest("DA_LOGIN_TC002 - Verify that user can login specific repository successfully via Dashboard login page with correct credentials.");
            Login loginPage = new Login(driver);
            MainPage mainPage = loginPage.SignOn("administrator", "123");

            //Then
            //VP: Verify that Dashboard Mainpage appears
            validations.Add(mainPage.ValidateDashboardMainPageDisplayed());
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }



        //[CustomTestMethod]
        //public void TC002()
        //{
        //    //Given
        //    //1. Navigate to Dashboard login page.
        //    test.Info("Navigate to Dashboard login page.");
        //    var driver = Browser.Open(Constant.HomePage, "chrome");

        //    //When
        //    //2. Enter valid username and password.
        //    //3. Click on "Login" button

        //    test = LogTest("DA_LOGIN_TC002 - Verify Dashboard Error message \"Username or password is invalid\" appears");
        //    Login loginPage = new Login(driver);
        //    MainPage mainPage = loginPage.SignOn("test", "123");

        //    //Then
        //    //VP: Verify that Dashboard Mainpage appears
        //    validations.Add(mainPage.ValidateTextInAlertPopup("Username or password is invalid"));
        //    Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
        //}
    }
}

