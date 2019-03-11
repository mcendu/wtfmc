/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */
using System;
using System.Net.Http;

namespace wtfmc
{
    /// <summary>
    /// The data usable for login.
    /// </summary>
    public class LoginData
    {
        string accessToken;
    }
    /// <summary>
    /// A login client.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Check if the Login server is available.
        /// </summary>
        /// <returns>true if server is up, false otherwise.</returns>
        bool CheckAvailable();

        /// <summary>
        /// Authenticate with server with email & password.
        /// </summary>
        /// <param name="email">Email, or sometimes Legacy username.</param>
        /// <param name="passwd">Password</param>
        /// <returns>LoginData if authentication is successful, null otherwise.</returns>
        LoginData Authenticate(string email, string passwd);

        /// <summary>
        /// Refresh a login.
        /// </summary>
        /// <param name="access">A LoginData.</param>
        /// <returns>LoginData if authentication is refreshed, null otherwise.</returns>
        LoginData Refresh(LoginData access);

        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="access">A LoginData.</param>
        /// <returns></returns>
        void LogOut(LoginData access);
    }
}
