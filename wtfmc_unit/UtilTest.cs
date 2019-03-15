using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using wtfmc;

namespace wtfmc_unit
{
    [TestClass]
    public class UtilTest
    {
        /// <summary>
        /// The bintohex method is intended to be compliant
        /// with the way cryptographic hashes are outputted.
        /// </summary>
        [TestMethod]
        public void HashComplianceTest()
        {
            const string intended = "a94a8fe5ccb19ba61c4c0873d391e987982fbbd3";
            byte[] hashinput = { 0x74, 0x65, 0x73, 0x74 };
            string actual = Util.bintohex(SHA1.Create().ComputeHash(hashinput));
            Assert.AreEqual(intended, actual);
        }
    }
}
