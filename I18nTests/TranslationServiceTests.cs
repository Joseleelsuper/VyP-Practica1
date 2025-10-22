using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class TranslationServiceTests
    {
        [TestMethod()]
        public void SetLanguageTest()
        {
            Assert.IsTrue(TranslationService.SetLanguage("ES_es"));
            Assert.AreEqual("ES_es", TranslationService.CurrentLanguage);

            Assert.IsTrue(TranslationService.SetLanguage("EN_en"));
            Assert.AreEqual("EN_en", TranslationService.CurrentLanguage);

            Assert.IsFalse(TranslationService.SetLanguage("EN_en")); // Setting the same language should return false
            Assert.IsFalse(TranslationService.SetLanguage("TK_tk"));
        }
    }
}