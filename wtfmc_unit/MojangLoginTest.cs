using System;
using wtfmc.MojangAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace unit
{
    [TestClass]
    public class MojangLoginTest
    {
        [TestMethod]
        public void ExportDataTest()
        {
            // Taken from actual login data; accessToken has since been invalidated
            string payload = "{\"accessToken\":\"dbc5065818454f2ba7261ccf7146d64a\",\"clientToken\":\"0123456789abcdef0123456789abcdef\",\"selectedProfile\":{\"agent\":\"minecraft\",\"id\":\"f38abefd3b7d4f7a967bb1da32da2f3d\",\"name\":\"McEndu\",\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"createdAt\":1528198061000,\"legacyProfile\":false,\"suspended\":false,\"tokenId\":\"22049502\",\"paid\":true,\"migrated\":false},\"availableProfiles\":[{\"agent\":\"minecraft\",\"id\":\"f38abefd3b7d4f7a967bb1da32da2f3d\",\"name\":\"McEndu\",\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"createdAt\":1528198061000,\"legacyProfile\":false,\"suspended\":false,\"tokenId\":\"22049502\",\"paid\":true,\"migrated\":false}],\"user\":{\"id\":\"37147c9640e78c8f074d7695bccfcf0d\",\"email\":\"nathandu@outlook.com\",\"username\":\"nathandu@outlook.com\",\"registerIp\":\"64.64.108.*\",\"registeredAt\":1528033389000,\"passwordChangedAt\":1528033389000,\"dateOfBirth\":1053216000000,\"suspended\":false,\"blocked\":false,\"secured\":false,\"migrated\":false,\"emailVerified\":true,\"legacyUser\":false,\"properties\":[{\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"profileId\":\"\",\"name\":\"preferredLanguage\",\"value\":\"en-us\",\"languageCode\":\"en-us\"},{\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"profileId\":\"\",\"name\":\"registrationCountry\",\"value\":\"JP\"}],\"verifiedByParent\":false,\"hashed\":false,\"fromMigratedUser\":false}}";
            MojangLogin aclient = new MojangLogin();
            aclient.Data = aclient.Serialize(JObject.Parse(payload));
            Assert.AreEqual("dbc5065818454f2ba7261ccf7146d64a", aclient.AccessToken);
            Assert.AreEqual("0123456789abcdef0123456789abcdef", aclient.ClientToken);
        }

        /*
        [TestMethod]
        public void DemoUserTest()
        {

        }
        */

        [TestMethod]
        public void MergeTest()
        {
            string payload = "{\"accessToken\":\"dbc5065818454f2ba7261ccf7146d64a\",\"clientToken\":\"0123456789abcdef0123456789abcdef\",\"selectedProfile\":{\"agent\":\"minecraft\",\"id\":\"f38abefd3b7d4f7a967bb1da32da2f3d\",\"name\":\"McEndu\",\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"createdAt\":1528198061000,\"legacyProfile\":false,\"suspended\":false,\"tokenId\":\"22049502\",\"paid\":true,\"migrated\":false},\"availableProfiles\":[{\"agent\":\"minecraft\",\"id\":\"f38abefd3b7d4f7a967bb1da32da2f3d\",\"name\":\"McEndu\",\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"createdAt\":1528198061000,\"legacyProfile\":false,\"suspended\":false,\"tokenId\":\"22049502\",\"paid\":true,\"migrated\":false}],\"user\":{\"id\":\"37147c9640e78c8f074d7695bccfcf0d\",\"email\":\"nathandu@outlook.com\",\"username\":\"nathandu@outlook.com\",\"registerIp\":\"64.64.108.*\",\"registeredAt\":1528033389000,\"passwordChangedAt\":1528033389000,\"dateOfBirth\":1053216000000,\"suspended\":false,\"blocked\":false,\"secured\":false,\"migrated\":false,\"emailVerified\":true,\"legacyUser\":false,\"properties\":[{\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"profileId\":\"\",\"name\":\"preferredLanguage\",\"value\":\"en-us\",\"languageCode\":\"en-us\"},{\"userId\":\"37147c9640e78c8f074d7695bccfcf0d\",\"profileId\":\"\",\"name\":\"registrationCountry\",\"value\":\"JP\"}],\"verifiedByParent\":false,\"hashed\":false,\"fromMigratedUser\":false}}";
            MojangLogin aclient = new MojangLogin();
            aclient.Data = aclient.Serialize(JObject.Parse(payload));
            JObject pl2 = new JObject
            {
                "clientToken", "9876547210e333339876547210e33333"
            };
        }
    }
}
