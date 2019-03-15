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
            ClientToken = "00000000000000000000000000000000";
        }

        private string rawdata;
        public string AccessToken { get; private set; }
        public string ClientToken { get; set; }
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

        /// <summary>
        /// Query the authentication server.
        /// </summary>
        /// <param name="method">A string representing a method.</param>
        /// <param name="fields">A username-password pair if authenticating with them,
        /// or null otherwise.</param>
        /// <returns>A string with JSON data.</returns>
        private async Task<string> AuthQuery(string method, string[] fields)
        {
            // Generate payload to be sent
            JObject req = new JObject
            {
                { "clientToken", ClientToken }
            };
            if (method == "authenticate")
            {
                req.Add("agent", new JObject
                {
                    { "name", "Minecraft" },
                    { "version", 1 }
                });
                req.Add("username", fields[0]);
                req.Add("password", fields[1]);
                req.Add("requestUser", true);
            }
            else
            {
                req.Add("accessToken", AccessToken);
            }

            try
            {
                // Generate the HTTP POST message
                HttpRequestMessage msg = new HttpRequestMessage
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri("/" + method)
                };
                msg.Headers.Add("Content-Type", "application/json; charset=utf-8");
                msg.Content = new StringContent(req.ToString());
                // Send the payload
                HttpResponseMessage res = await hclient.SendAsync(msg);
                string resContent = await res.Content.ReadAsStringAsync();
                if (!res.IsSuccessStatusCode)
                {
                    GenException(resContent);
                }
                return resContent;
            }
            catch (Exception e)
            {
                throw new AuthClientException(e.ToString());
            }
        }

        private static void GenException(string errordata)
        {
            JObject jobj = JObject.Parse(errordata);
            string err = jobj["error"].ToString();
            string errdesc = jobj["errorMessage"].ToString();
            throw new BadAuthException($"{err}: {errdesc}");
        }

        public void Authenticate(string email, string passwd)
        {
            Data = AuthQuery("authenticate", new string[] { email, passwd }).Result;
        }

        public async Task<bool> CheckAvailable()
        {
            HttpResponseMessage res;
            try
            {
                res = await hclient.GetAsync("/");
            }
            catch
            {
                return false;
            }
            if ((int)res.StatusCode != 200)
                return false;
            JObject sstatus = JObject.Parse(res.Content.ReadAsStringAsync().Result);
            if ((string)sstatus["Status"] != "OK")
                return false;
            return true;
        }

        public void LogOut()
        {
            AuthQuery("invalidate").Wait();
        }

        public void Refresh()
        {
            try
            {
                AuthQuery("validate").Wait();
            }
            catch (BadAuthException)
            {
                Data = AuthQuery("refresh").Result;
            }
        }

        /// <summary>
        /// Query the authentication server, with the fields parameter omitted.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private async Task<string> AuthQuery(string method)
        {
            return await AuthQuery(method, null);
        }
    }
}
