using BLL;
using BLL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        IEnumerable<User> users;

        [TestInitialize]
        public void Initialize()
        {
            users = new List<User>
            {
                new User{ Email = "testEmail", Password = "testPwd" },
                new User{ Email = "testEmail2", Password = "testPwd2" },
                new User{ Email = "testEmail2", Password = "testPwd3" }
            };
        }

        [TestMethod]
        public void WithoutPasswordsTest()
        {
            var res = ExtensionMethods.WithoutPassword(users.First());

            Assert.IsNotNull(res.Email);
            Assert.IsNull(res.Password);
        }

        [TestMethod]
        public void failWithoutPasswordsTest()
        {
            var res = ExtensionMethods.WithoutPassword(null);

            Assert.IsNull(res);
        }

        [TestMethod]
        public void WithoutPasswordTest()
        {
            IEnumerable<User> expected = new List<User>
            {
                new User{ Email = "testEmail", Password = null },
                new User{ Email = "testEmail2", Password = null },
                new User{ Email = "testEmail2", Password = null }
            };

            var actual = ExtensionMethods.WithoutPasswords(users);

            var res = actual.All(i => i.Password == null);

            Assert.AreEqual(true, res);
        }

        [TestMethod]
        public void failWithoutPasswordTest()
        {
            var actual = ExtensionMethods.WithoutPasswords(null);

            Assert.IsNull(actual);
        }
    }
}