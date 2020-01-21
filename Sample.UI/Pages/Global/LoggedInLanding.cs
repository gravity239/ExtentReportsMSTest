using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Sample.Common.Helper;


namespace Sample.UI.Pages.Global
{
    public class LoggedInLanding : PageBase
    {

        #region Entities
        #endregion


        #region Actions

        public LoggedInLanding(IWebDriver webDriver) : base(webDriver)
        {
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
       
        //public LoggedInLanding LogValidation(ref List<KeyValuePair<string, bool>> validations, List<KeyValuePair<string, bool>> addedValidations)
        //{
        //   validations.AddRange(addedValidations);
        //    return this;
        //}

        //public T LogValidation<T>(ref List<KeyValuePair<string, bool>> validations, List<KeyValuePair<string, bool>> addedValidations)
        //{
        //    LogValidation(ref validations, addedValidations);
        //    return (T)Activator.CreateInstance(typeof(T), WebDriver);
        //}

        //public LoggedInLanding LogValidation(ref List<KeyValuePair<string, bool>> validations, KeyValuePair<string, bool> addedValidation)
        //{
        //    validations.Add(addedValidation);
        //    return this;
        //}

        //public T LogValidation<T>(ref List<KeyValuePair<string, bool>> validations, KeyValuePair<string, bool> addedValidation)
        //{
        //    validations.Add(addedValidation);
        //    return (T)Activator.CreateInstance(typeof(T), WebDriver);
        //}

        #endregion
    }
}
