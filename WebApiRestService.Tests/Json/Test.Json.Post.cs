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
	[Category("Json.Posts")]
	public class TestJsonPostCalls
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
		public async Task TestPost()
		{
			//arrange
			Restaurant rest = new Restaurant();
			rest.Name = "Xpto";
			rest.CountryId = 1;
			rest.Address = "1234, Road Street";

			//act
			var ret = await client.CreateAsync(rest);

			//assert
			Assert.AreEqual(rest.Name, ret.Name);
		}

		[Test]
		public async Task TestPostWithError()
		{
			//arrange
			Restaurant rest = new Restaurant();
			rest.Name = "invalid name".PadRight(70);
			rest.CountryId = 1;
			rest.Address = "1234, Road Street";

			try
			{
				//act
				var ret = await client.CreateAsync(rest);

				Assert.Fail("WebApiException expected");
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
                Assert.IsNotEmpty(e.Details.ModelState);

                foreach (var item in e.Details.ModelState)
                {
                    foreach (var error in item.Value)
                    {
                        Console.WriteLine(string.Format("Parameter: {0} - Error: {1}", item.Key, error));
                    }
                }
            }
		}
	}
}
