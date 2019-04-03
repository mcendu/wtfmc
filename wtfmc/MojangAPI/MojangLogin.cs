/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    public sealed class MojangLogin : ILoginClient
    {
        public MojangLogin()
        {
        }

        public string UID => (string)Data["userid"];
        public string AccessToken
            => (string)Data["accessToken"];
        public string ClientToken
            => (string)Data["clientToken"];
        public string ID => (string)Data["id"];
        public string Username => (string)Data["displayName"];

        private static readonly HttpClient hclient = new HttpClient
        {
            BaseAddress = new Uri("https://authserver.mojang.com")
        };

        public LoginType LoginType => LoginType.Mojang;

        public JObject Data { get; set; }

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
                throw new AuthClientException(e.ToString(), e);
            }
        }

        private static void GenException(string errordata)
        {
            JObject jobj = JObject.Parse(errordata);
            string err = (string)jobj["error"];
            string errdesc = (string)jobj["errorMessage"];
            throw new BadAuthException($"{err}: {errdesc}");
        }

        public void Authenticate(string email, string passwd)
        {
            JObject serverFmt = JObject.Parse(AuthQuery("authenticate", new string[] { email, passwd }).Result);
            Data = Serialize(serverFmt);
        }

        public static bool CheckAvailable()
        {
            HttpResponseMessage res;
            try
            {
                res = hclient.GetAsync("/").Result;
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
                string serverFmt = AuthQuery("refresh").Result;
                Data["accessToken"] = JObject.Parse(serverFmt)["accessToken"];
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

        /// <remarks>
        /// Do not refer to this when implementing
        /// ILoginClient.Serialize.
        /// </remarks>
        public JObject Serialize(JObject serverFmt)
        {
            JObject data = new JObject
            {
                { "authtype", LoginType.ToString() },
                { "userid", serverFmt["user"]["id"] },
                { "id", serverFmt["selectedProfile"]["id"] },
                { "accessToken", serverFmt["accessToken"] },
                { "username", serverFmt["user"]["username"] },
                { "properties", serverFmt["user"]["properties"] },
                { "displayName", serverFmt["selectedProfile"]["name"] },
                { "clientToken", serverFmt["clientToken"] },
            };
            return data;
        }
    }
}
