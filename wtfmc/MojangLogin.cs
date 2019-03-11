/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    public sealed class MojangLoginData : ILoginData
    {
        public MojangLoginData()
        {
        }

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
    }
    public sealed class MojangLogin : ILoginClient
    {
        public readonly string clientToken;
        public MojangLogin(string clientToken)
        {
            this.clientToken = clientToken;
        }

        private async Task<HttpResponseMessage> ReqAuth(string req)
        {
            HttpClient hclient = new HttpClient
            {
                BaseAddress = new Uri("authserver.mojang.com")
            };
            return await hclient.PostAsync("/authenticate", new StringContent(req));
        }

        public ILoginData Authenticate(string email, string passwd)
        {
            string req = "{" +
                "\"agent\": {" +
                "\"name\": \"Minecraft\", \"version\": 1" +
                "}," +
                "\"username\": \""+email+"\"," +
                "\"password\": \""+passwd+"\"," +
                "\"clientToken\": \""+clientToken+"\"," +
                "\"requestUser\": true" +
                "}";
            HttpResponseMessage res = ReqAuth(req).Result;
            if ((int)res.StatusCode == 200)
            {
                return new MojangLoginData
                {
                    Data = res.Content.ReadAsStringAsync().Result
                };
            }
            return null;
        }

        public bool CheckAvailable()
        {
            throw new NotImplementedException();
        }

        public void LogOut(ILoginData access)
        {
            throw new NotImplementedException();
        }

        public ILoginData Refresh(ILoginData access)
        {
            throw new NotImplementedException();
        }
    }
}
