using Logica.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class PasswordHasherTests
    {
        [TestMethod()]
        public void HashPasswordTest()
        {
            Assert.Throws<ArgumentException>(() => PasswordHasher.HashPassword(null));
            Assert.Throws<ArgumentException>(() => PasswordHasher.HashPassword(""));

            Assert.IsNotNull(PasswordHasher.HashPassword("7j4Ew+=hl\\m7H=k|6S^SuG0fbB@#W$3U"));
        }

        [TestMethod()]
        public void VerifyPasswordTest()
        {
            Assert.IsTrue(PasswordHasher.VerifyPassword("7j4Ew+=hl\\m7H=k|6S^SuG0fbB@#W$3U", PasswordHasher.HashPassword("7j4Ew+=hl\\m7H=k|6S^SuG0fbB@#W$3U")));
            Assert.IsFalse(PasswordHasher.VerifyPassword("7j4Ew+=hl\\m7H=k|6S^SuG0fbB@#W$3U", PasswordHasher.HashPassword("7j4Ew+=hl\\m7H=k|6S^SuG0fB@#W$3U")));
        }
    }
}