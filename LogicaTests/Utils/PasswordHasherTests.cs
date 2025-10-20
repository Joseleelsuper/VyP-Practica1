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

            Assert.IsNotNull(PasswordHasher.HashPassword("password123"));
        }

        [TestMethod()]
        public void VerifyPasswordTest()
        {
            Assert.IsTrue(PasswordHasher.VerifyPassword("password123", PasswordHasher.HashPassword("password123")));
        }
    }
}