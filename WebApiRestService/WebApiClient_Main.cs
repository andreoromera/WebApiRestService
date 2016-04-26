using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiRestService
{
	/// <summary>
	/// Represents a generic rest client able to make calls to a Web Api 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public partial class WebApiClient<T> : IDisposable where T : class
	{
		/// <summary>
		/// The <see cref="System.Net.Http.HttpClient"/> used in this instance
		/// </summary>
		protected HttpClient Client;

        /// <summary>
        /// Gets the HttpClient headers
        /// </summary>
        public HttpRequestHeaders Headers { get; private set; }

        /// <summary>
        /// Gets the <see cref="T:WebApiRestService.WebApiClientOptions"/> associated with this client
        /// </summary>
        /// <remarks>
        /// Just for reference purpose because any change made to this object won't have any effect because it is used
        /// only to create the client instance with default values
        /// </remarks>
        public WebApiClientOptions Options { get; private set; }
        
        /// <summary>
        /// Gets the <see cref="System.Net.Http.HttpClientHandler"/> associated with this client
        /// </summary>
        /// <remarks>
        /// Just for reference purpose because any change made to this handler won't have any effect once it is used 
        /// in the <see cref="System.Net.Http.HttpClient"/> constructor and cannot be changed
        /// </remarks>
        public HttpClientHandler Handler { get; private set; }

        /// <summary>
		/// Initializes a new instance of <see cref="T:WebApiRestService.WebApiClient`1"/> using default options
		/// </summary>
		public WebApiClient() : this(new WebApiClientOptions())
		{

		}

		/// <summary>
        /// Initializes a new instance of <see cref="T:WebApiRestService.WebApiClient`1"/> using custom options
		/// </summary>
		/// <param name="options">Custom options to be used</param>
		/// <exception cref="System.ArgumentNullException" />
        public WebApiClient(WebApiClientOptions options) : this(options, new HttpClientHandler())
        { 
        }
		
        /// <summary>
        /// Initializes a new instance of <see cref="T:WebApiRestService.WebApiClient`1"/> using custom options
		/// </summary>
		/// <param name="options">Custom options to be used</param>
        /// <param name="handler">The desired handler</param>
		/// <exception cref="System.ArgumentNullException" />
		public WebApiClient(WebApiClientOptions options, HttpClientHandler handler)
		{
			if (options == null) throw new ArgumentNullException("options", "Options parameter is required");
            if (handler == null) throw new ArgumentNullException("options", "Handler parameter is required");
            
			this.Options = options;
            this.Handler = handler;

            //Set the credentials to access resources, if provided
            handler.Credentials = options.Authentication.Credentials;

			//Creates the httpClient and sets default properties
            this.Client = new HttpClient(handler);
            this.Client.BaseAddress = new Uri(this.Options.BaseAddress);

            //Sets the default request header properties
            this.Headers = this.Client.DefaultRequestHeaders;
            this.Headers.Accept.Clear();
            this.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.Options.ContentType.ToMediaFormat()));
		}

		/// <summary>
		/// Generates a Uri string based on the parameters provided
		/// </summary>
		/// <remarks>
		/// The generated Uri can be formatted in one of the following types:
		/// <list type="bullet">
		/// <item><description>{controller}</description></item>
		/// <item><description>{controller}/{param}</description></item>
		/// <item><description>{controller}?{param.key1}={param.value1}&amp;{param.key2}={param.value2}</description></item>
		/// <item><description>{controller}/{action}</description></item>
		/// <item><description>{controller}/{action}/{param}</description></item>
		/// <item><description>{controller}/{action}?{param.key1}={param.value1}&amp;{param.key2}={param.value2}</description></item>
		/// </list>
		/// </remarks>
		/// <param name="controller">The controller that will be called. Required.</param>
		/// <param name="action">The action that will be called. Optional.</param>
		/// <param name="param">Can be a simple type like int, string, etc or an anonymous type defining many properties. Optional.</param>
		/// <returns>The complete Uri that will be used to call the Web Api</returns>
		/// <exception cref="System.ArgumentNullException" />
		protected string GenerateUri(string controller, string action, object param)
		{
			string uri = string.Empty;

			//Controller must exist
			if (!string.IsNullOrEmpty(controller))
				uri = controller;
			else
				throw new ArgumentNullException("controller", "Controller parameter is required");

			if (!string.IsNullOrEmpty(action))
			{
				//If action is provided, append it
				uri = string.Format("{0}/{1}", uri, action);
			}

			if (param != null)
			{
				if (param.GetType().IsAnonymousType())
				{
					//Param is an anonymous type, so convert it to a dictionary
					uri += param.ParseAnonymousObject().ToQueryString();
				}
				else
				{
					//Is not an anonymous type. Just append it
					uri = string.Format("{0}/{1}", uri, param);
				}
			}
			
			return uri;
		}

        /// <summary>
        /// Gets the Authorization header
        /// </summary>
        private void Authenticate()
        {
            Client.DefaultRequestHeaders.Authorization = this.Options.Authentication.Authenticate();
        }

        /// <summary>
        /// Encapsulates the error response into a WebApiClientException
        /// </summary>
        /// <param name="response">The response message containing the error</param>
        private WebApiClientException GetException(HttpResponseMessage response)
        {
            WebApiClientExceptionDetails details = new WebApiClientExceptionDetails();

            //Gets the content string where exception details are defined
            string content = response.Content.ReadAsStringAsync().Result;

            try
            {
                Dictionary<string, IList<string>> modelState = null;

                //Parses the content string into a json object
                var json = JObject.Parse(content);
                
                //If ModelState is informed, get it
                IDictionary<string, JToken> jModelState = (JObject)json["ModelState"];

                //Transforms the ModelState into a Dictionary
                if(jModelState != null)
                    modelState = ((JObject)json["ModelState"]).ToObject<Dictionary<string, IList<string>>>();

                string message = (string)json["Message"];
                string exceptionMessage = (string)json["ExceptionMessage"];

                //Creates the details object setting its values
                details.Message = exceptionMessage ?? message;
                details.ExceptionType = (string)json["ExceptionType"];
                details.StackTrace = (string)json["StackTrace"];
                details.ModelState = modelState;
            }
            catch
            {
                if (!string.IsNullOrEmpty(content))
                {
                    //Content isn't in json format but still contains useful information
                    details.Message = content;
                }
            }

            //Sets the reason phrase
            details.Reason = response.ReasonPhrase;
            
            //Return the exception
            return new WebApiClientException(response.StatusCode, details);
        }

        /// <summary>
		/// Disposes resources
		/// </summary>
		public void Dispose()
		{
			this.Client.Dispose();
		}
	}
}