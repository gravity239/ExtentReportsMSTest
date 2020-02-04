using FluentAssertions;
using Sample.Common.Helper;
using Sample.UI.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Sample.Common.Helper.ExtentReportsHelper;
using Sample.UI.Utilities;
using Sample.UI.Pages.Google;

namespace Sample.UI.Tests.User
{
    [TestClass]
    public class NewGoogleTests : Test
    {
       
        [TestCategory("HappyPath")]
        [SampleTestMethod]
        public void TC003()
        {
            //Given
            //1. Navigate to Google Home Page.
            test.Info("Navigate to Google page.");
            NavigateToUrl("http://google.com");

            //When
            //2. Enter search text.
            test = LogTest("TC003 - Verify that search value is correct.");
            Home google = PageFactory.Get<Home>();
            google.Search("abc");

            //Then
            //VP: Verify that search text displays correctly 
            validations.Add(google.ValidateSearchValue("abc"));
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }


        [SampleTestMethod]
        public void TC004()
        {
            //Given
            //1. Navigate to Google Home Page.
            test.Info("Navigate to Google page.");
            NavigateToUrl("http://google.com");

            //When
            //2. Enter search text.
            test = LogTest("TC004 - Verify that search value is correct.");
            Home google = PageFactory.Get<Home>();
            google.Search("abc");

            //Then
            //VP: Verify that search text displays correctly 
            validations.Add(google.ValidateSearchValue("abcd"));
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }
    }
}

