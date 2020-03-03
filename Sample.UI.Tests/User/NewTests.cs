using System;
using System.Text;
using System.Collections.Generic;
using static Sample.Common.Helper.ExtentReportsHelper;
using Sample.UI.Pages.Google;
using FluentAssertions;
using Sample.UI.Utilities;
using NUnit.Framework;
using System.Threading;

namespace Sample.UI.Tests.User
{
    /// <summary>
    /// Summary description for NewTests
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class NewTests : NunitTest
    {
      

        [Test]
        [Category("Nunit")]
        public void TC006()
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

        [Test]
        [Category("Nunit")]
        public void TC007()
        {
            //Given
            //1. Navigate to Google Home Page.
            test.Info("Navigate to Google page.");
            NavigateToUrl("http://google.com");

            //When
            //2. Enter search text.
            Thread.Sleep(10000);
            test = LogTest("TC007 - Verify that search value is correct.");
            Home google = PageFactory.Get<Home>();
            google.Search("abc");

            //Then
            //VP: Verify that search text displays correctly 
            validations.Add(google.ValidateSearchValue("abc"));
            Console.WriteLine(string.Join(System.Environment.NewLine, validations.ToArray()));
            validations.Should().OnlyContain(validations => validations.Value).Equals(bool.TrueString);
        }
    }
}
