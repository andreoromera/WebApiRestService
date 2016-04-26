using System;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApiRestService.Api;
using System.Net;
using System.Collections.Generic;
using WebApiRestService.Api.Database;

namespace WebApiRestService.Tests
{
	[TestFixture]
	[Category("Inheritance")]
	public class TestRestaurantRestClient
	{
		private RestaurantRestClient client;

		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			this.client = new RestaurantRestClient();
		}

		[TestFixtureTearDown]
		public void TearDownFixture()
		{
			this.client.Dispose();
		}

		[Test]
		public async Task TestGetRestaurant()
		{
			int id = 1;

			Restaurant r = await this.client.GetRestaurantById(id);

			Assert.NotNull(r);
			Assert.AreEqual(id, r.Id);
		}

		[Test]
		public async Task TestGetRestaurants()
		{
			List<Restaurant> list = await this.client.GetRestaurants();

			Assert.NotNull(list);
			Assert.IsNotEmpty(list);
		}

		[Test]
		public async Task TestGetRestaurantsByCountry()
		{
			int countryId = 1;

			List<Restaurant> list = await this.client.GetRestaurantByCountry(countryId);

			Assert.NotNull(list);
			Assert.IsNotEmpty(list);
		}

		[Test]
		public async Task TestGetRestaurantsByCountryAndReviews()
		{
			int countryId = 4;
			int minReviews = 1;

			List<Restaurant> list = await this.client.GetRestaurantByCountryAndNumberOfReviews(countryId, minReviews);

			Assert.NotNull(list);
			Assert.IsNotEmpty(list);
		}

		[Test]
		public void TestCreateRestaurant()
		{
			Restaurant rest = new Restaurant();
			rest.Name = "Xpto";
			rest.CountryId = 1;
			rest.Address = "1234, Road Street";

			Restaurant newRest = null;

			Assert.DoesNotThrow(async () => newRest = await this.client.CreateRestaurant(rest));

			Assert.NotNull(newRest);
		}

		[Test]
		public async Task TestEditRestaurant()
		{
			Restaurant createdRest = new Restaurant();
			createdRest.Name = "Xpto";
			createdRest.CountryId = 1;
			createdRest.Address = "1234, Road Street";

			createdRest = await this.client.CreateRestaurant(createdRest);
			Assert.NotNull(createdRest);

			createdRest.Name = "Xpto edited";
			var editedRest = await this.client.EditRestaurant(createdRest);

            Assert.NotNull(editedRest);
			Assert.AreEqual(createdRest.Name, editedRest.Name);
		}

		[Test]
		public async Task TestDeleteRestaurant()
		{
			Restaurant createdRest = new Restaurant();
			createdRest.Name = "Xpto";
			createdRest.CountryId = 1;
			createdRest.Address = "1234, Road Street";

			createdRest = await this.client.CreateRestaurant(createdRest);
			Assert.NotNull(createdRest);

			Assert.DoesNotThrow(async () => await this.client.DeleteRestaurant(createdRest.Id));
		}
	}
}
