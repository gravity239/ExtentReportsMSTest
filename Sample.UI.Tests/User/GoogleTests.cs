using FluentAssertions;
using Sample.Common.Helper;
using Sample.UI.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Sample.Common.Helper.ExtentReportsHelper;

namespace Sample.UI.Tests.User
{
    [TestClass]
    public class GoogleTests : TestBase
    {

        [CustomTestMethod]
        public void TC001()
        {
            //Given
            //1. Navigate to Google Home Page.
            test.Info("Navigate to Google page.");
            var driver = Browser.Open(Constant.HomePage, "chrome");
            
            //When
            //2. Enter search text.
            test = LogTest("TC001 - Verify that search value is correct.");
            GoogleHome google = new GoogleHome(driver);
            google.Search("abc");

            //Then
            //VP: Verify that search text displays correctly 
            validations.Add(google.ValidateSearchValue("abc"));
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }

        [CustomTestMethod]
        public void TC002()
        {
            //Given
            //1. Navigate to Google Home Page.
            test.Info("Navigate to Google page.");
            var driver = Browser.Open(Constant.HomePage, "chrome");

            //When
            //2. Enter search text.
            test = LogTest("TC002 - Verify that search value is correct.");
            GoogleHome google = new GoogleHome(driver);
            google.Search("abc");

            //Then
            //VP: Verify that search text displays correctly 
            validations.Add(google.ValidateSearchValue("abcd"));
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }
    }
}

