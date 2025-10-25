using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace wwwTest.Activities
{
    [TestClass]
    public class ActiviteErrorCreate
    {
        private static IWebDriver driver;
        
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            driver = new EdgeDriver();
        }
        
        [ClassCleanup]
        public static void CleanupClass()
        {
            try
            {
                driver.Close();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
        
        [TestMethod]
        public void TheActiviteErrorCreateTest()
        {
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/application/Dashboard.aspx");
            driver.FindElement(By.Id("btnAddActivity")).Click();
        }
    }
}
