using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApiRestService.Authentication
{
    /// <summary>
    /// Defines authentication using Negotiate, NTLM, Basic or Kerberos
    /// </summary>
    public class WindowsIntegratedAuthentication : IAuthentication
    {
        private ICredentials credential = null;

        /// <summary>
        /// Windows Integrated Authentication uses credentials to authenticate the user
        /// </summary>
        public ICredentials Credentials
        {
            get
            {
                return this.credential;
            }
        }

        /// <summary>
        /// Creates the object using just username and password
        /// </summary>
        /// <remarks>
        /// It works with Negotiate, NTLM and Basic authentication types when domain isn't needed
        /// </remarks>
        /// <param name="username">The windows user name</param>
        /// <param name="password">The user password</param>
        /// <exception cref="System.ArgumentNullException" />
        public WindowsIntegratedAuthentication(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            this.credential = new NetworkCredential(username, password);
        }

        /// <summary>
        /// Creates the object using username, password and domain
        /// </summary>
        /// <remarks>
        /// It is used when the Api is hosted in a server joined to a domain
        /// </remarks>
        /// <param name="username">The windows user name</param>
        /// <param name="password">The user password</param>
        /// <param name="domain">The network domain</param>
        /// <exception cref="System.ArgumentNullException" />
        public WindowsIntegratedAuthentication(string username, string password, string domain)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(domain)) throw new ArgumentNullException("domain");

            this.credential = new NetworkCredential(username, password);
        }

        /// <summary>
        /// Lets the api consumer to provide the credentials
        /// </summary>
        /// <remarks>
        /// Useful when is necessary to provide, for example, a <see cref="System.Net.CredentialCache" />
        /// </remarks>
        /// <param name="credentials">An object that implements <see cref="System.Net.ICredentials" /></param>
        /// <exception cref="System.ArgumentNullException" />
        public WindowsIntegratedAuthentication(ICredentials credentials)
        {
            if (credentials == null) throw new ArgumentNullException("credentials");

            this.credential = credentials;
        }

        /// <summary>
        /// Not necessary for this kind of authentication because windows negotiates the authentication process itself
        /// </summary>
        public AuthenticationHeaderValue Authenticate()
        {
            return null;
        }
    }
}
