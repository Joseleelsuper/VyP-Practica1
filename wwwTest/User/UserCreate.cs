using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace wwwTest.User
{
    [TestClass]
    public class UserCreate
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
        public void TheUserCreateTest()
        {
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/session/Login.aspx");
            driver.FindElement(By.LinkText("Admin")).Click();
            driver.FindElement(By.Id("btnLogin")).Click();
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/application/Dashboard.aspx");
            driver.FindElement(By.Id("txtNewName")).Click();
            driver.FindElement(By.Id("txtNewName")).Clear();
            driver.FindElement(By.Id("txtNewName")).SendKeys("Prueba");
            driver.FindElement(By.Id("txtNewSurname")).Clear();
            driver.FindElement(By.Id("txtNewSurname")).SendKeys("García");
            driver.FindElement(By.Id("txtNewEmail")).Clear();
            driver.FindElement(By.Id("txtNewEmail")).SendKeys("PGarcia@hotmail.com");
            driver.FindElement(By.Id("txtNewPassword")).Clear();
            driver.FindElement(By.Id("txtNewPassword")).SendKeys("PruebaGarcia123_321");
            driver.FindElement(By.Id("txtNewNif")).Clear();
            driver.FindElement(By.Id("txtNewNif")).SendKeys("93414384E");
            driver.FindElement(By.Id("txtNewTelf")).Clear();
            driver.FindElement(By.Id("txtNewTelf")).SendKeys("654655368");
            driver.FindElement(By.Id("btnCreateUser")).Click();
        }
    }
}
