using System;
using NUnit.Framework;
using System.Collections.Generic;
using WebApiRestService.Api;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using WebApiRestService.Api.Database;

namespace WebApiRestService.Tests
{
	[TestFixture]
	[Category("Timeout")]
	public class TestTimeout
	{
		private WebApiClient<Restaurant> client;

		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			var config = new TestConfig();
			config.DefaultOptions.Timeout = 1;

			this.client = new WebApiClient<Restaurant>(config.DefaultOptions);
		}
		
		[TestFixtureTearDown]
		public void TearDownFixture()
		{
			this.client.Dispose();
		}

		[Test]
		public async Task TestGetTooShortTimeoutUsingOptions()
		{
			int id = 1;

			try
			{
				Restaurant restaurant = await client.GetOneAsync(id, "get");
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestGetTooShortTimeoutUsingToken()
		{
			int id = 1;

			try
			{
				CancellationTokenSource src = new CancellationTokenSource(1);
				Restaurant restaurant = await client.GetOneAsync(id, "get", src.Token);
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestGetAllTooShortTimeoutUsingOptions()
		{
			try
			{
				List<Restaurant> restaurant = await client.GetManyAsync("get");
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestGetAllTooShortTimeoutUsingToken()
		{
			try
			{
				CancellationTokenSource src = new CancellationTokenSource(1);
				List<Restaurant> restaurant = await client.GetManyAsync("get", src.Token);
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestCreateTooShortTimeoutUsingOptions()
		{
			//arrange
			Restaurant rest = new Restaurant();
			rest.Name = "Xpto";
			rest.CountryId = 1;
			rest.Address = "1234, Road Street";
			
			try
			{
				var resp = await client.CreateAsync(rest);
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestCreateTooShortTimeoutUsingToken()
		{
			//arrange
			Restaurant rest = new Restaurant();
			rest.Name = "Xpto";
			rest.CountryId = 1;
			rest.Address = "1234, Road Street";
			
			try
			{
				CancellationTokenSource src = new CancellationTokenSource(1);
				await client.CreateAsync(rest, src.Token);
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestEditTooShortTimeoutUsingOptions()
		{
			//arrange
			Restaurant rest = new Restaurant();
			rest.Id = 209;

			try
			{
				await client.EditAsync(rest);
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestEditTooShortTimeoutUsingToken()
		{
			//arrange
			Restaurant rest = new Restaurant();
			rest.Id = 209;

			try
			{
				CancellationTokenSource src = new CancellationTokenSource(1);
				await client.EditAsync(rest, src.Token);
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public async Task TestDeleteTooShortTimeoutUsingToken()
		{
			int id = 1;

			try
			{
				CancellationTokenSource src = new CancellationTokenSource(1);
				await client.DeleteAsync(id, src.Token);
				Assert.Fail();
			}
			catch (WebApiClientException e)
			{
				Assert.AreEqual(HttpStatusCode.RequestTimeout, e.StatusCode);
			}
		}

		[Test]
		public void TestCancelTask()
		{
			int id = 1;
			CancellationTokenSource src = new CancellationTokenSource();

			try
			{
				Task<Restaurant> task = client.GetOneAsync(id, "get",  src.Token);
				src.Cancel();
				task.Wait();
				Assert.IsNull(task.Result);
			}
			catch (AggregateException e)
			{
				e.Handle(ex =>
				{
					if (ex is WebApiClientException)
					{
						var timeoutException = ex as WebApiClientException;

						Assert.AreEqual(HttpStatusCode.RequestTimeout, timeoutException.StatusCode);

						return true;
					}

					Assert.Fail(ex.Message);

					return false;
				});
			}
			catch (Exception e)
			{
				Assert.Fail(e.GetType().Name);
			}
		}
	}
}
