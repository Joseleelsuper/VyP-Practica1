using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace wwwTest.Login
{
    [TestClass]
    public class LoginUserMaria
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
        public void TheLoginUserMariaTest()
        {
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/session/Login.aspx");
            driver.FindElement(By.LinkText("María")).Click();
            driver.FindElement(By.Id("btnLogin")).Click();
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/application/Dashboard.aspx");
            driver.FindElement(By.Id("btnLogout")).Click();
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/session/Login.aspx");
        }
    }
}
