using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logica.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

[assembly: DoNotParallelize] // https://learn.microsoft.com/es-es/dotnet/core/testing/mstest-analyzers/mstest0001

namespace Logica.Utils.Tests
{
    [TestClass()]
    public class ValidateTests
    {
        [DataContract]
        private class IbanItem
        {
            [DataMember(Name = "IBAN")] public string IBAN { get; set; }
            [DataMember(Name = "return")] public int Return { get; set; }
        }
        public static IEnumerable<object[]> LeerIbanJson()
        {
            var filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "DatosTest", "iban.json"));
            using (var fs = File.OpenRead(filePath))
            {
                var ser = new DataContractJsonSerializer(typeof(List<IbanItem>));
                var list = (List<IbanItem>)ser.ReadObject(fs);
                foreach (var item in list)
                {
                    yield return new object[] { item.IBAN, item.Return };
                }
            }
        }

        [TestMethod]
        [DynamicData(nameof(LeerIbanJson))]
        public void IBANTest(string iban, int expected)
        {
            bool ok = Validate.IBAN(iban);
            Assert.AreEqual(expected == 1, ok, $"IBAN '{iban}' esperado {expected}");
        }

        // NIF data-driven test from CSV
        public static IEnumerable<object[]> LeerDniCsv()
        {
            var filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "DatosTest", "dni.csv"));
            return File.ReadAllLines(filePath)
                        .Where(l => l != null)
                        .Select(linea => linea.Split(';'))
                        .Where(campos => campos.Length >= 2)
                        .Select(campos => new object[]
                        {
                            campos[0] == "null" ? null : campos[0],
                            int.Parse(campos[1])
                        });
        }

        [TestMethod]
        [DynamicData(nameof(LeerDniCsv))]
        public void NIFTest(string nif, int expected)
        {
            bool ok = Validate.NIF(nif);
            Assert.AreEqual(expected == 1, ok, $"NIF '{nif}' esperado {expected}");
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

        // Duplicate removed below
        public static IEnumerable<object[]> LeerTelfXml()
        {
            var filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "DatosTest", "telf.xml"));
            var doc = new XmlDocument();
            doc.Load(filePath);
            var nodes = doc.SelectNodes("/telephones/telf");
            foreach (XmlNode el in nodes)
            {
                var isNull = el.Attributes["null"] != null ? el.Attributes["null"].Value : null;
                var valAttr = el.Attributes["value"] != null ? el.Attributes["value"].Value : null;
                var retAttr = el.Attributes["return"] != null ? el.Attributes["return"].Value : null;
                int ret;
                int.TryParse(retAttr, out ret);
                string value = (string.Equals(isNull, "true", StringComparison.OrdinalIgnoreCase)) ? null : valAttr;
                yield return new object[] { value, ret };
            }
        }

        [TestMethod]
        [DynamicData(nameof(LeerTelfXml))]
        public void TelfTest(string telf, int expected)
        {
            bool ok = Validate.Telf(telf);
            Assert.AreEqual(expected == 1, ok, $"Telf '{telf}' esperado {expected}");
        }

        [TestMethod]
        public void PasswordTest()
        {
            // Normal Tests
            Assert.IsTrue(Validate.Password("StrongP@ssw0rd")); // Valid password
            Assert.IsFalse(Validate.Password("weakpass")); // No uppercase, digit, special char
            // Edge Cases
            Assert.IsFalse(Validate.Password("")); // Empty string
            Assert.IsFalse(Validate.Password(null)); // Null input
            Assert.IsFalse(Validate.Password("Short1!")); // Too short
            Assert.IsFalse(Validate.Password("nouppercase1!")); // No uppercase letter
            Assert.IsFalse(Validate.Password("NOLOWERCASE1!")); // No lowercase letter
            Assert.IsFalse(Validate.Password("NoDigit!")); // No digit
            Assert.IsFalse(Validate.Password("NoSpecialChar1")); // No special character
        }
    }
}