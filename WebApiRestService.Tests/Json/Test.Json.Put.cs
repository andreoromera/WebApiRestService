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
	[Category("Json.Puts")]
	public class TestJsonPutCalls
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
		public async Task TestPut()
		{
			//arrange
			int id = 1;
			Restaurant originalRest = await this.client.GetOneAsync(id, "get");

			string originalName = originalRest.Name;
			originalRest.Name = "Restaurant Temp";

			//act
            var updatedRest = await client.EditAsync(originalRest);

			//assert that updated restaurant has the same name as the original
			Assert.AreEqual(originalRest.Name, updatedRest.Name);
            
			//update the restaurant with its original name
			updatedRest.Name = originalName;
            updatedRest = await client.EditAsync(updatedRest);

			//assert that updated restaurant has the very first name
			Assert.AreEqual(originalName, updatedRest.Name);

		}

		[Test]
		public async Task TestPutWithError()
		{
			//arrange
			int id = 1;
			Restaurant rest = await this.client.GetOneAsync(id, "get");
			rest.Name = "invalid name".PadRight(70);
            rest.Address = "invalid address".PadRight(550);

			try
			{
				//act
				await client.EditAsync(rest);

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
