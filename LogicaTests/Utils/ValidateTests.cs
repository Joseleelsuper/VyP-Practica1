using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logica.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: DoNotParallelize] // https://learn.microsoft.com/es-es/dotnet/core/testing/mstest-analyzers/mstest0001

namespace Logica.Utils.Tests
{
    [TestClass()]
    public class ValidateTests
    {
        [TestMethod()]
        public void NIFTest()
        {
            Assert.IsTrue(Validate.NIF("23000000T"));
            Assert.IsTrue(Validate.NIF("23000001R"));
            Assert.IsTrue(Validate.NIF("23000002W"));
            Assert.IsTrue(Validate.NIF("23000003A"));
            Assert.IsTrue(Validate.NIF("23000004G"));
            Assert.IsTrue(Validate.NIF("23000005M"));
            Assert.IsTrue(Validate.NIF("23000006Y"));
            Assert.IsTrue(Validate.NIF("23000007F"));
            Assert.IsTrue(Validate.NIF("23000008P"));
            Assert.IsTrue(Validate.NIF("23000009D"));
            Assert.IsTrue(Validate.NIF("23000010X"));
            Assert.IsTrue(Validate.NIF("23000011B"));
            Assert.IsTrue(Validate.NIF("23000012N"));
            Assert.IsTrue(Validate.NIF("23000013J"));
            Assert.IsTrue(Validate.NIF("23000014Z"));
            Assert.IsTrue(Validate.NIF("23000015S"));
            Assert.IsTrue(Validate.NIF("23000016Q"));
            Assert.IsTrue(Validate.NIF("23000017V"));
            Assert.IsTrue(Validate.NIF("23000018H"));
            Assert.IsTrue(Validate.NIF("23000019L"));
            Assert.IsTrue(Validate.NIF("23000020C"));
            Assert.IsTrue(Validate.NIF("23000021K"));
            Assert.IsTrue(Validate.NIF("23000022E"));

            Assert.IsFalse(Validate.NIF("12345678A")); // Incorrect letter

            // Edge Cases
            Assert.IsFalse(Validate.NIF("1234567Z")); // Too short
            Assert.IsFalse(Validate.NIF("123456789Z")); // Too long
            Assert.IsFalse(Validate.NIF("ABCDEFGHZ")); // Non-numeric characters
            Assert.IsFalse(Validate.NIF("")); // Empty string
            Assert.IsFalse(Validate.NIF(null)); // Null input
        }

        [TestMethod()]
        public void IBANTest()
        {
            // Normal Tests
            Assert.IsTrue(Validate.IBAN("ES9121000418450200051332")); // Correct IBAN
            Assert.IsFalse(Validate.IBAN("ES9121000418450200051333")); // Incorrect IBAN
            Assert.IsTrue(Validate.IBAN("ES91 2100 0418 4502 0005 1332")); // Correct IBAN with spaces
            Assert.IsFalse(Validate.IBAN("ES91 2100 0418 4502 0005 1333")); // Incorrect IBAN with spaces

            // Edge Cases
            Assert.IsFalse(Validate.IBAN("ES912100041845020005133")); // Too short
            Assert.IsFalse(Validate.IBAN("ES91210004184502000513322")); // Too long
            Assert.IsFalse(Validate.IBAN("US9121000418450200051332")); // Incorrect country code
            Assert.IsFalse(Validate.IBAN("ES91-2100-0418-4502-0005-1332")); // Invalid characters
            Assert.IsFalse(Validate.IBAN("")); // Empty string
            Assert.IsFalse(Validate.IBAN(null)); // Null input
        }

        [TestMethod()]
        public void EmailTest()
        {
            // Normal Tests
            Assert.IsTrue(Validate.Email("tory002254@gmail.com")); // Correct email
            Assert.IsFalse(Validate.Email("tory002254@gmail")); // Missing TLD

            // Edge Cases
            Assert.IsFalse(Validate.Email("")); // Empty string
            Assert.IsFalse(Validate.Email(null)); // Null input
            Assert.IsFalse(Validate.Email("plainaddress")); // No '@' and domain
            Assert.IsFalse(Validate.Email("@missingusername.com")); // Missing username
            Assert.IsFalse(Validate.Email("tory002254@.com")); // Missing domain name
            Assert.IsFalse(Validate.Email("tory002254.com")); // Missing '@'
        }

        [TestMethod()]
        public void TelfTest()
        {
            // Normal Tests
            Assert.IsTrue(Validate.Telf("644842590")); // Correct phone number
            Assert.IsTrue(Validate.Telf("+34644842590")); // Correct phone number
            Assert.IsTrue(Validate.Telf("0034644842590")); // Correct phone number
            Assert.IsTrue(Validate.Telf("34644842590")); // Correct phone number
            Assert.IsFalse(Validate.Telf("+66644842590")); // Only Spain prefix is validated

            Assert.IsFalse(Validate.Telf("12345")); // Too short
            Assert.IsFalse(Validate.Telf("1234567890")); // Too long
            Assert.IsFalse(Validate.Telf("64484A590")); // Non-numeric characters

            Assert.IsFalse(Validate.Telf("044842590")); // Does not start with 6, 7, 8, or 9
            Assert.IsFalse(Validate.Telf("144842590")); // Does not start with 6, 7, 8, or 9
            Assert.IsFalse(Validate.Telf("244842590")); // Does not start with 6, 7, 8, or 9
            Assert.IsFalse(Validate.Telf("344842590")); // Does not start with 6, 7, 8, or 9

            Assert.IsTrue(Validate.Telf("744842590")); // Starts with 7
            Assert.IsTrue(Validate.Telf("844842590")); // Starts with 8
            Assert.IsTrue(Validate.Telf("944842590")); // Starts with 9
            Assert.IsTrue(Validate.Telf("644842590")); // Starts with 6

            // Edge Cases
            Assert.IsFalse(Validate.Telf("")); // Empty string
            Assert.IsFalse(Validate.Telf(null)); // Null input
        }
    }
}