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
    /// Defines the interface for authentication
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// Provides the credentials used to request the WebApi
        /// </summary>
        ICredentials Credentials { get; }

        /// <summary>
        /// When implemented, authenticates the client to use server resources
        /// </summary>
        AuthenticationHeaderValue Authenticate();
    }
}
