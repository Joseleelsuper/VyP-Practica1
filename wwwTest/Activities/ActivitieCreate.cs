using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace wwwTest.Activities
{
    [TestClass]
    public class ActivitieCreate
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
        public void TheActivitieCreateTest()
        {
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/session/Login.aspx");
            driver.FindElement(By.LinkText("Admin")).Click();
            driver.FindElement(By.Id("btnLogin")).Click();
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/application/Dashboard.aspx");
            driver.FindElement(By.Id("txtName")).Click();
            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys("Prueba");
            driver.FindElement(By.Id("btnAddActivity")).Click();
        }
    }
}
