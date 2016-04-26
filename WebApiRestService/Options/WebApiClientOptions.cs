using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using WebApiRestService.Authentication;

namespace WebApiRestService
{
    /// <summary>
    /// Represents the options to initiate a <see cref="T:WebApiRestService.WebApiClient`1"/> object
    /// </summary>
    public class WebApiClientOptions
    {
        private ContentType contentType = WebApiRestService.ContentType.Json;
        private string baseAddress = "http://localhost/";
        private uint timeout = 30000;
        private MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
        private IAuthentication authentication = new NoAuthentication();

        /// <summary>
        /// An object representing the desired type of authentication
        /// </summary>
        public IAuthentication Authentication
        {
            get
            {
                return authentication;
            }
            set
            {
                authentication = value;
            }
        }

        /// <summary>
        /// Gets or sets the controller name that will be called from the client
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets the content type used in request and response calls
        /// </summary>
        /// <remarks>
        /// If not defined, uses the default value <see cref="WebApiRestService.ContentType.Json"/>
        /// </remarks>
        /// <seealso cref="WebApiRestService.ContentType"/>
        public ContentType ContentType
        {
            get
            {
                return this.contentType;
            }
            set
            {
                if (value == WebApiRestService.ContentType.Json)
                {
                    this.formatter = new JsonMediaTypeFormatter();
                }
                else
                {
                    this.formatter = new XmlMediaTypeFormatter();
                }

                this.contentType = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Net.Http.Formatting.MediaTypeFormatter"/> associated to this instance
        /// </summary>
        public MediaTypeFormatter Formatter
        {
            get
            {
                return this.formatter;
            }
        }

        /// <summary>
        /// Gets or sets the base address that will be used in request calls
        /// </summary>
        /// <remarks>
        /// If not defined, uses the default value http://localhost/
        /// </remarks>
        public string BaseAddress
        {
            get
            {
                return this.baseAddress;
            }
            set
            {
                baseAddress = value.EndsWith("/") ? value : value + "/";
            }
        }

        /// <summary>
        /// Gets or sets the default timeout limit for all client calls
        /// </summary>
        /// <remarks>
        /// If not defined, uses the default value 30000
        /// </remarks>
        public uint Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                timeout = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebApiClientOptions()
        {
        }

        /// <summary>
        /// Initializes the object that represents the options for the <see cref="T:WebApiRestService.WebApiClient`1"/> service
        /// </summary>
        /// <param name="baseAddress">The base address that will be used in request calls</param>
        /// <param name="controller">The controller name that will be called from the client</param>
        public WebApiClientOptions(string baseAddress, string controller)
        {
            if (string.IsNullOrEmpty(baseAddress)) throw new ArgumentNullException("baseAddress");
            if (string.IsNullOrEmpty(controller)) throw new ArgumentNullException("controller");

            this.BaseAddress = baseAddress;
            this.Controller = controller;
        }
    }
}
