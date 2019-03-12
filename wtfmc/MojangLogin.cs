/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    public sealed class MojangLogin : ILoginClient
    {
        public MojangLogin()
        {
            hclient = new HttpClient
            {
                BaseAddress = new Uri("https://authserver.mojang.com")
            };
        }

        private string rawdata;
        public string AccessToken { get; private set; }
        public string ClientToken { get; private set; }
        private readonly HttpClient hclient;

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
            }
        }

        private async Task<HttpResponseMessage> PostString(string req)
        {
            HttpRequestMessage msg = new HttpRequestMessage
            {
                Method = new HttpMethod("POST"),
            };
            msg.Headers.Add("Content-Type", "application/json; charset=utf-8");
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
