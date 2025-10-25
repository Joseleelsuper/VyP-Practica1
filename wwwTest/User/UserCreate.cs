using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace wwwTest.User
{
    [TestClass]
    public class UserCreate
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private bool acceptNextAlert = true;
        
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            driver = new FirefoxDriver();
            baseURL = "https://www.google.com/";
        }
        
        [ClassCleanup]
        public static void CleanupClass()
        {
            try
            {
                //driver.Quit();// quit does not close the window
                driver.Close();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
        
        [TestInitialize]
        public void InitializeTest()
        {
            verificationErrors = new StringBuilder();
        }
        
        [TestCleanup]
        public void CleanupTest()
        {
            Assert.AreEqual("", verificationErrors.ToString());
        }
        
        [TestMethod]
        public void TheUserCreateTest()
        {
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
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
        
        private string CloseAlertAndGetItsText() {
            try {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert) {
                    alert.Accept();
                } else {
                    alert.Dismiss();
                }
                return alertText;
            } finally {
                acceptNextAlert = true;
            }
        }
    }
}
