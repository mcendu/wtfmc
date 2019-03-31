using System;
using wtfmc.MojangAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace wtfmc_unit
{
    [TestClass]
    public class FormatTest
    {
        [TestMethod]
        public void BasicFormatTest()
        {
            string fmt = "${hello} world";
            System.Collections.Hashtable subhash = new System.Collections.Hashtable
            {
                { "hello", "Hello" }
            };
            Formatter fmr = new Formatter();
            Assert.AreEqual("Hello world", fmr.Format(fmt, subhash, fmr));
        }
    }
}
