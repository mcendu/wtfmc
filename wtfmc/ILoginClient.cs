/*
 * By McEndu and skyhgzsh
 * Copyright (C) 2019 MIT License
 */
using System;
using System.Threading.Tasks;

namespace wtfmc
{
    public interface ILoginClient
    {
        /// <summary>
        /// The type of login.
        /// </summary>
        string LoginType { get; }

        /// <summary>
        /// The access token for the client.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// The UUID of the user.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Raw login data.
        /// </summary>
        string Data { set; get; }

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

    /// <summary>
    /// Exception class for any error encoutered while logging in.
    /// </summary>
    public class AuthClientException : System.Exception
    {
        public AuthClientException(string message) : base(message)
        {
        }

        public AuthClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when authentication server returns an error message.
    /// </summary>
    public class BadAuthException : AuthClientException
    {
        public BadAuthException(string message) : base(message)
        {
        }
    }
}
