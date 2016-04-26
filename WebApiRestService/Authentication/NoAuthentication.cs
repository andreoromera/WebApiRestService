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
    /// Defines no authentication
    /// </summary>
    public class NoAuthentication : IAuthentication
    {
        /// <summary>
        /// Not necessary for this kind of authentication
        /// </summary>
        public AuthenticationHeaderValue Authenticate()
        {
            return null;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public ICredentials Credentials
        {
            get { return null; }
        }
    }
}
