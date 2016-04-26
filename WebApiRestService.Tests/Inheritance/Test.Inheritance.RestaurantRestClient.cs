using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApiRestService.Api;
using WebApiRestService.Api.Database;

namespace WebApiRestService.Tests
{
	public class RestaurantRestClient : WebApiClient<Restaurant>
	{
		private static WebApiClientOptions options = new WebApiClientOptions() {
				BaseAddress = ConfigurationManager.AppSettings["baseAddress"],
				ContentType = ContentType.Json,
				Timeout = 30000,
				Controller = "restaurants"
		};

        /// <summary>
        /// Creates an instance of RestaurantRestClient using default options
        /// </summary>
		public RestaurantRestClient() : this(options)
		{
		}

        /// <summary>
        /// Creates an instance of RestaurantRestClient using explicit options
        /// </summary>
        private RestaurantRestClient(WebApiClientOptions options) : base(options)
		{
		}

		public async Task<Restaurant> GetRestaurantById(int id)
		{
			try
			{
				return await this.GetOneAsync(id, "get");
			}
			catch (WebApiClientException e)
			{
				if (e.StatusCode == HttpStatusCode.NotFound)
				{
					return null;
				}
				
				throw e;
			}
		}
		
		public async Task<List<Restaurant>> GetRestaurants()
		{
			return await this.GetManyAsync("get");
		}

		public async Task<Restaurant> CreateRestaurant(Restaurant rest)
		{
			return await this.CreateAsync(rest);
		}

		public async Task<Restaurant> EditRestaurant(Restaurant rest)
		{
			return await this.EditAsync(rest);
		}

		public async Task DeleteRestaurant(int id)
		{
			await this.DeleteAsync(id);
		}

		public async Task<List<Restaurant>> GetRestaurantByCountry(int countryId)
		{
			return await this.GetManyAsync(new { countryId = countryId }, "GetByCountry");
		}

		public async Task<List<Restaurant>> GetRestaurantByCountryAndNumberOfReviews(int countryId, int minReviews)
		{
			return await this.GetManyAsync(new { countryId = countryId, minReviews = minReviews });
		}
	}
}
