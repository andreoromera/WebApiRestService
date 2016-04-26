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
		/// Calls the Web Api to delete the object of type T
		/// </summary>
		/// <param name="param">Can be a simple type like int, string, etc or an anonymous type defining many properties</param>
		/// <remarks>
		/// Requests are sent using HttpVerb DELETE
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task DeleteAsync(object param)
		{
			if (param == null) throw new ArgumentNullException("param");

			CancellationTokenSource src = new CancellationTokenSource((int)this.Options.Timeout);

			await DeleteAsync(param, this.Options.Controller, null, src.Token);
		}

		/// <summary>
		/// Calls the Web Api to delete the object of type T
		/// </summary>
		/// <param name="param">Can be a simple type like int, string, etc or an anonymous type defining many properties</param>
		/// <param name="token">The <see cref="System.Threading.CancellationToken"/> to be used to cancel the call</param>
		/// <remarks>
		/// Requests are sent using HttpVerb DELETE
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task DeleteAsync(object param, CancellationToken token)
		{
			if (param == null) throw new ArgumentNullException("param");
			if (token == null) throw new ArgumentNullException("token");

			await DeleteAsync(param, this.Options.Controller, null, token);
		}

		/// <summary>
		/// Calls the Web Api to delete the object of type T
		/// </summary>
		/// <param name="param">Can be a simple type like int, string, etc or an anonymous type defining many properties</param>
		/// <param name="action">Explicit define the action that will be called</param>
		/// <remarks>
		/// Requests are sent using HttpVerb DELETE
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task DeleteAsync(object param, string action)
		{
			if (param == null) throw new ArgumentNullException("param");
			if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");

			CancellationTokenSource src = new CancellationTokenSource((int)this.Options.Timeout);

			await DeleteAsync(param, this.Options.Controller, action, src.Token);
		}

		/// <summary>
		/// Calls the Web Api to delete the object of type T
		/// </summary>
		/// <param name="param">Can be a simple type like int, string, etc or an anonymous type defining many properties</param>
		/// <param name="action">Explicit define the action that will be called</param>
		/// <param name="token">The <see cref="System.Threading.CancellationToken"/> to be used to cancel the call</param>
		/// <remarks>
		/// Requests are sent using HttpVerb DELETE
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		public async virtual Task DeleteAsync(object param, string action, CancellationToken token)
		{
			if (param == null) throw new ArgumentNullException("param");
			if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");
			if (token == null) throw new ArgumentNullException("token");

			await DeleteAsync(param, this.Options.Controller, action, token);
		}

		/// <summary>
		/// Calls the Web Api to delete the object of type T
		/// </summary>
		/// <param name="param">Can be a simple type like int, string, etc or an anonymous type defining many properties</param>
		/// <param name="controller">The controller that will be called</param>
		/// <param name="action">Explicit define the action that will be called</param>
		/// <param name="token">The <see cref="System.Threading.CancellationToken"/> to be used to cancel the call</param>
		/// <remarks>
		/// Requests are sent using HttpVerb DELETE
		/// </remarks>
		/// <exception cref="WebApiRestService.WebApiClientException" />
		/// <exception cref="System.ArgumentNullException" />
		protected async virtual Task DeleteAsync(object param, string controller, string action, CancellationToken token)
		{
			try
			{
#if DEBUG
				//When debugging timeouts, sometimes the operation completes so fast that is impossible to recreate the scenario
				await Task.Delay(50);
#endif

                //Authenticates if needed
                Authenticate();

                //Makes the async call 
				var response = await this.Client.DeleteAsync(GenerateUri(controller, action, param), token);

				if (!response.IsSuccessStatusCode)
				{
                    //Packs the error response into a WebApiClientException
                    throw GetException(response);
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
