using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiRestService
{
	public partial class WebApiClient<T>
	{
		/// <summary>
		/// Send an object to the Web Api to be created
		/// </summary>
		/// <param name="obj">The object to be created</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/> that will yield the created object of type T</returns>
        /// <remarks>
		/// Requests are sent using HttpVerb POST
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task<T> CreateAsync(T obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			CancellationTokenSource src = new CancellationTokenSource((int)this.Options.Timeout);

			return await CreateAsync(obj, this.Options.Controller, null, src.Token);
		}

		/// <summary>
		/// Send an object to the Web Api to be created
		/// </summary>
		/// <param name="obj">The object to be created</param>
		/// <param name="token">The <see cref="System.Threading.CancellationToken"/> to be used to cancel the call</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/> that will yield the created object of type T</returns>
        /// <remarks>
		/// Requests are sent using HttpVerb POST
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task<T> CreateAsync(T obj, CancellationToken token)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			if (token == null) throw new ArgumentNullException("token");

			return await CreateAsync(obj, this.Options.Controller, null, token);
		}

		/// <summary>
		/// Send an object to the Web Api to be created
		/// </summary>
		/// <param name="obj">The object to be created</param>
		/// <param name="action">Explicit define the action that will be called</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/> that will yield the created object of type T</returns>
        /// <remarks>
		/// Requests are sent using HttpVerb POST
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task<T> CreateAsync(T obj, string action)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");

			CancellationTokenSource src = new CancellationTokenSource((int)this.Options.Timeout);

			return await CreateAsync(obj, this.Options.Controller, action, src.Token);
		}

		/// <summary>
		/// Send an object to the Web Api to be created
		/// </summary>
		/// <param name="obj">The object to be created</param>
		/// <param name="action">Explicit define the action that will be called</param>
		/// <param name="token">The <see cref="System.Threading.CancellationToken"/> to be used to cancel the call</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/> that will yield the created object of type T</returns>
        /// <remarks>
		/// Requests are sent using HttpVerb POST
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task<T> CreateAsync(T obj, string action, CancellationToken token)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");
			if (token == null) throw new ArgumentNullException("token");

			return await CreateAsync(obj, this.Options.Controller, action, token);
		}

		/// <summary>
		/// Send an object to the Web Api to be created
		/// </summary>
		/// <param name="obj">The object to be created</param>
		/// <param name="controller">The controller that will be called</param>
		/// <param name="action">Explicit define the action that will be called</param>
		/// <param name="token">The <see cref="System.Threading.CancellationToken"/> to be used to cancel the call</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/> that will yield the created object of type T</returns>
        /// <remarks>
		/// Requests are sent using HttpVerb POST
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		protected async virtual Task<T> CreateAsync(T obj, string controller, string action, CancellationToken token)
		{
			try
			{
#if DEBUG
				//When debugging timeouts, sometimes the operation completes so fast that is impossible to recreate the scenario
				await Task.Delay(50);
#endif

                //Authenticates if needed
                Authenticate();

                //Loads the object into the content's buffer. If we call PutAsync passing obj instead of content, an error occurs
				var content = new ObjectContent<T>(obj, this.Options.Formatter);
				await content.LoadIntoBufferAsync();

				//Makes the async call 
				var response = await this.Client.PostAsync(GenerateUri(controller, action, null), content, token);

				if (!response.IsSuccessStatusCode)
				{
                    //Packs the error response into a WebApiClientException
                    throw GetException(response);
                }

                try
                {
                    //Returns the object
                    return await response.Content.ReadAsAsync<T>(token);
                }
                catch
                {
                    return default(T);
                }
            }
			catch (OperationCanceledException)
			{
				//Timeout or operation canceled by user
                throw new WebApiClientException(HttpStatusCode.RequestTimeout, "Task canceled due to timeout or token cancellation");
            }
			catch (WebApiClientException e)
			{
				throw e;
			}
			catch (Exception e)
			{
				//Generic exception
				throw new WebApiClientException(HttpStatusCode.InternalServerError, e);
			}
		}
    }
}
