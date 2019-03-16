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
        /// Raw login data.
        /// </summary>
        string Data { set; get; }

        /// <summary>
        /// Check if the Login server is available.
        /// 检查服务器是否可以访问。
        /// </summary>
        /// <returns>true if server is up, false otherwise.</returns>
        Task<bool> CheckAvailable();

        /// <summary>
        /// Authenticate with server with email & password.
        /// 使用用户名和密码登录。
        /// </summary>
        /// <param name="email">Email, or sometimes Legacy username.</param>
        /// <param name="passwd">Password</param>
        void Authenticate(string email, string passwd);

        /// <summary>
        /// Refresh a login.
        /// 刷新登录（emmm）。
        /// </summary>
        /// <param name="access">A LoginData.</param>
        void Refresh();

        /// <summary>
        /// Logout.
        /// 注销。
        /// </summary>
        /// <param name="access">A LoginData.</param>
        void LogOut();
    }

    /// <summary>
    /// Exception class for any error encoutered while logging in.
    /// 代表登录过程中发生的错误。
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
    /// 服务端返回错误信息时的抛出的异常。
    /// </summary>
    public class BadAuthException : AuthClientException
    {
        public BadAuthException(string message) : base(message)
        {
        }
    }
}
