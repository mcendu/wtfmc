/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    public sealed class MojangLogin : ILoginClient
    {
        private string rawdata;

        private string AccessToken { get; set; }
        private string ClientToken { get; set; }
        private string ProfileID { get; set; }
        private string ProfileName { get; set; }

        public string Data
        {
            get
            {
                return rawdata;
            }
            set
            {
                rawdata = value;
                JObject data = JObject.Parse(value);
                ClientToken = (string)data["clientToken"];
                AccessToken = (string)data["accessToken"];
                ProfileID = (string)data["selectedProfile"]["id"];
                ProfileName = (string)data["selectedProfile"]["name"];
            }
        }

        private async Task<HttpResponseMessage> PostString(string req)
        {
            HttpClient hclient = new HttpClient
            {
                BaseAddress = new Uri("authserver.mojang.com")
            };
            return await hclient.PostAsync("/authenticate", new StringContent(req));
        }

        public void Authenticate(string email, string passwd)
        {
            try
            {
                string req = "{" +
                    "\"agent\": {" +
                    "\"name\": \"Minecraft\", \"version\": 1" +
                    "}," +
                    "\"username\": \"" + email + "\"," +
                    "\"password\": \"" + passwd + "\"," +
                    "\"clientToken\": \"" + ClientToken + "\"," +
                    "\"requestUser\": true" +
                    "}";
                HttpResponseMessage res = PostString(req).Result;
                if ((int)res.StatusCode != 200)
                {
                    throw new AuthClientException("Authentication failed");
                }
                Data = res.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw new AuthClientException("An error occured while authenticating");
            }
        }

        public async Task<bool> CheckAvailable()
        {
            HttpClient hclient = new HttpClient
            {
                BaseAddress = new Uri("authserver.mojang.com")
            };
            HttpResponseMessage res = await hclient.GetAsync("/");
            if ((int)res.StatusCode != 200)
                return false;
            JObject sstatus = JObject.Parse(res.Content.ReadAsStringAsync().Result);
            if ((string)sstatus["Status"] != "OK")
                return false;
            return true;
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
