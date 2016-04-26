using System;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApiRestService.Api;
using System.Net;
using WebApiRestService.Api.Database;

namespace WebApiRestService.Tests
{
	[TestFixture]
	[Category("Json")]
	[Category("Json.Deletes")]
	public class TestJsonDeleteCalls
	{
		private WebApiClient<Restaurant> client;

		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			this.client = new WebApiClient<Restaurant>(new TestConfig().DefaultOptions);
		}

		[TestFixtureTearDown]
		public void TearDownFixture()
		{
			this.client.Dispose();
		}

		[Test]
		public async Task TestDelete()
		{
			//arrange
			Restaurant rest = new Restaurant();
			rest.Name = "Test DeleteAsync Restaurant";
			rest.CountryId = 1;
			rest.Address = "1234, Road Street";

			//Creates the new restaurant
			var ret = await client.CreateAsync(rest);

			//act
			Assert.DoesNotThrow(async () => await client.DeleteAsync(ret.Id));

			try
			{
				//assert that deleted restaurant doesn't exist anymore
				var test = await this.client.GetOneAsync(ret.Id, "get");
				
				Assert.Fail("WebApiException expected");
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
			}
		}

		[Test]
		public async Task TestDeleteInvalidId()
		{
			//arrange
			int id = 9999;

			try
			{
				//act
				await client.DeleteAsync(id);

				Assert.Fail("WebApiException expected");
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
			}
		}
	}
}
