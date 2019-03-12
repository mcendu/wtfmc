/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */

using System.Threading.Tasks;

namespace wtfmc
{
    public interface ILoginClient
    {
        /// <summary>
        /// Raw login data.
        /// </summary>
        string Data { set; get; }

        /// <summary>
        /// Check if the Login server is available.
        /// </summary>
        /// <returns>true if server is up, false otherwise.</returns>
        Task<bool> CheckAvailable();

        /// <summary>
        /// Authenticate with server with email & password.
        /// </summary>
        /// <param name="email">Email, or sometimes Legacy username.</param>
        /// <param name="passwd">Password</param>
        void Authenticate(string email, string passwd);

        /// <summary>
        /// Refresh a login.
        /// </summary>
        /// <param name="access">A LoginData.</param>
        void Refresh();

        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="access">A LoginData.</param>
        void LogOut();
    }

    public class AuthClientException : System.Exception
    {
        public AuthClientException(string message) : base(message)
        {
        }
    }
}
