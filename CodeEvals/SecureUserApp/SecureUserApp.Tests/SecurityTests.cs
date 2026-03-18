using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureUserApp;
using System;

namespace SecureUserApp.Tests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void Register_User_Should_Hash_Password()
        {
            var user = new User("diksha", "1234");
            Assert.AreNotEqual("1234", user.HashedPassword);
        }
        [TestMethod]
        public void Authenticate_With_Correct_Password_Should_Return_True()
        {
            var user = new User("diksha", "1234");
            var result = user.Authenticate("1234");
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void Authenticate_With_Wrong_Password_Should_Return_False()
        {
            var user = new User("diksha", "1234");
            var result = user.Authenticate("wrong");
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void Encrypt_Decrypt_Should_Return_Original_Text()
        {
            var service = new EncryptionService();
            string original = "Hello";
            string encrypted = service.Encrypt(original);
            string decrypted = service.Decrypt(encrypted);
            Assert.AreEqual(original, decrypted);
        }
        [TestMethod]
        public void Decrypt_Invalid_Data_Should_Throw_Exception()
        {
            var service = new EncryptionService();
            try
            {
                service.Decrypt("invalid_cipher");
                Assert.Fail("Expected FormatException was not thrown.");
            }
            catch (FormatException)
            {
                return;
            }
            Assert.Fail("Expected FormatException was not thrown.");
        }
    }
}