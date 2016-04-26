using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApiRestService
{
    public partial class WebApiClient<T>
    {
        /// <summary>
        /// Adds a cookie to the request to be sent to the WebApi
        /// </summary>
        /// <param name="name">Cookie name</param>
        /// <param name="value">Cookie value</param>
        /// <param name="path">Cookie path</param>
        /// <param name="domain">Cookie domain</param>
        /// <exception cref="System.ArgumentNullException" />
        public void AddCookie(string name, string value, string path, string domain)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (string.IsNullOrEmpty(domain)) throw new ArgumentNullException("domain");
            
            AddCookie(new Cookie(name, value, path, domain));
        }

        /// <summary>
        /// Adds a collection of cookies to the request to be sent to the WebApi
        /// </summary>
        /// <param name="cookies">The cookie collection</param>
        /// <exception cref="System.ArgumentNullException" />
        public void AddCookie(CookieCollection cookies)
        {
            if (cookies == null) throw new ArgumentNullException("cookies");
            
            foreach (Cookie cookie in cookies)
            {
                AddCookie(cookie);
            }
        }

        /// <summary>
        /// Adds a cookie to the request to be sent to the WebApi
        /// </summary>
        /// <param name="cookie">The Cookie object</param>
        /// <exception cref="System.ArgumentNullException" />
        public void AddCookie(Cookie cookie)
        {
            if (cookie == null) throw new ArgumentNullException("cookie");
            
            if (this.Handler.UseCookies)
            {
                //Handler uses CookieContainer so we can use it as well
                this.Handler.CookieContainer.SetCookies(new Uri(this.Options.BaseAddress), cookie.ToString());
            }
            else
            {
                //Handler isn't using CookieContainer so we can just insert the cookie into the header
                this.Headers.Add("Cookie", cookie.ToString());
            }
        }
    }
}
