using System;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace wwwTest.Login
{
    [TestClass]
    public class LoginErrorBadUsername
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
        public void TheLoginErrorBadUsernameTest()
        {
            driver.Navigate().GoToUrl("http://localhost:61371/src/aspx/session/Login.aspx");
            driver.FindElement(By.LinkText("Admin")).Click();
            driver.FindElement(By.Id("txtEmail")).Click();
            driver.FindElement(By.Id("txtEmail")).Clear();
            driver.FindElement(By.Id("txtEmail")).SendKeys("admin@admin.ne");
            driver.FindElement(By.Id("btnLogin")).Click();
        }
    }
}
