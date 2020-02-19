using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Sample.Common.Helper.ExtentReportsHelper;
using Sample.UI.Pages.Google;
using FluentAssertions;
using Sample.UI.Utilities;

namespace Sample.UI.Tests.User
{
    /// <summary>
    /// Summary description for NewTests
    /// </summary>
    [TestClass]
    public class NewTests : Test
    {
        Home google = PageFactory.Get<Home>();

        [TestCategory("HappyPath")]
        [SampleTestMethod]
        public void TC006()
        {
            //Given
            //1. Navigate to Google Home Page.
            test.Info("Navigate to Google page.");
            NavigateToUrl("http://google.com");

            //When
            //2. Enter search text.
            test = LogTest("TC004 - Verify that search value is correct.");
            //var google = PageFactory.Get<Home>();
            google.Search("abc");

            //Then
            //VP: Verify that search text displays correctly 
            validations.Add(google.ValidateSearchValue("abcd"));
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }

        [TestCategory("HappyPath")]
        [SampleTestMethod]
        public void TC007()
        {
            //Given
            //1. Navigate to Google Home Page.
            test.Info("Navigate to Google page.");
            NavigateToUrl("http://google.com");

            //When
            //2. Enter search text.
            test = LogTest("TC004 - Verify that search value is correct.");
            //var google = PageFactory.Get<Home>();
            google.Search("abc");

            //Then
            //VP: Verify that search text displays correctly 
            validations.Add(google.ValidateSearchValue("abcd"));
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }
    }
}
