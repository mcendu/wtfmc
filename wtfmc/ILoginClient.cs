/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */

namespace wtfmc
{
    /// <summary>
    /// The data useful for login et al.
    /// </summary>
    public interface ILoginData
    {
        /// <summary>
        /// Raw login data.
        /// </summary>
        string Data { set; get; }
    }
    /// <summary>
    /// A login client.
    /// </summary>
    public interface ILoginClient
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
        ILoginData Authenticate(string email, string passwd);

        /// <summary>
        /// Refresh a login.
        /// </summary>
        /// <param name="access">A LoginData.</param>
        /// <returns>LoginData if authentication is refreshed, null otherwise.</returns>
        ILoginData Refresh(ILoginData access);

        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="access">A LoginData.</param>
        /// <returns></returns>
        void LogOut(ILoginData access);
    }
}
