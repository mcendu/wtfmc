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
            string fmt = "${hello_W} world";
            System.Collections.Hashtable subhash = new System.Collections.Hashtable
            {
                { "hello_W", "Hello" }
            };
            Formatter fmr = new Formatter();
            Assert.AreEqual("Hello world", fmr.Format(fmt, subhash));
        }
    }
}
