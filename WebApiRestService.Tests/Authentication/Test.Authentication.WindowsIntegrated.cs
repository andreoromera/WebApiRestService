using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiRestService;
using System.Net;
using System.Web.Http;
using WebApiRestService.Dto;
using WebApiRestService.Authentication;
using System.IO;
using System.Net.Http;

namespace WebApiRestService.Tests
{
	[TestFixture]
	[Category("Authentication")]
	public class TestWindowsIntegratedAuthentication
	{
        private const string userName = "TestUser";
        private const string password = "123456";
        private WebApiClientOptions options = new TestConfig().DefaultOptions;

        [Test]
        public async Task TestDefaultCredentialsAuthGetAll()
        {
            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                //act
                var restaurants = await client.GetManyAsync("GetWithAuth");

                //assert
                Assert.IsNotEmpty(restaurants);
                Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
            }
        }

        [Test]
        public async Task TestDefaultCredentialsAuthGetAllUnauthorized()
        {
            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, new HttpClientHandler() { UseDefaultCredentials = false }))
            {
                try
                {
                    //act
                    var restaurants = await client.GetManyAsync("GetWithAuth");
                    Assert.Fail("Expected Unauthorized exception");
                }
                catch (WebApiClientException e)
                {
                    //assert
                    Assert.AreEqual(HttpStatusCode.Unauthorized, e.StatusCode);
                }
            }
        }

        [Test]
        public async Task TestDefaultAuthGetAll()
        {
            //arrange
            options.Authentication = new WindowsIntegratedAuthentication(userName, password);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                //act
                var restaurants = await client.GetManyAsync("GetWithAuth");

                //assert
                Assert.IsNotEmpty(restaurants);
                Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
            }
        }

        [Test]
        public async Task TestDefaultAuthGetAllUnauthorized()
        {
            //arrange
            options.Authentication = new WindowsIntegratedAuthentication("xxx", "yyy");

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                try
                {
                    //act
                    var restaurants = await client.GetManyAsync("GetWithAuth");
                    Assert.Fail("Expected Unauthorized exception");
                }
                catch (WebApiClientException e)
                {
                    //assert
                    Assert.AreEqual(HttpStatusCode.Unauthorized, e.StatusCode);
                }
            }
        }

        [Test]
        public async Task TestNegotiateGetAll()
        {
            //arrange
            CredentialCache cc = new CredentialCache();
            cc.Add(new Uri(options.BaseAddress), "Negotiate", new NetworkCredential(userName, password));
            options.Authentication = new WindowsIntegratedAuthentication(cc);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                //act
                var restaurants = await client.GetManyAsync("GetWithAuth");

                //assert
                Assert.IsNotEmpty(restaurants);
                Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
            }
        }

        [Test]
        public async Task TestNegotiateGetAllUnauthorized()
        {
            //arrange
            CredentialCache cc = new CredentialCache();
            cc.Add(new Uri(options.BaseAddress), "Negotiate", new NetworkCredential("xxx", "yyyy"));
            options.Authentication = new WindowsIntegratedAuthentication(cc);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                try
                {
                    //act
                    var restaurants = await client.GetManyAsync("GetWithAuth");
                    Assert.Fail("Expected Unauthorized exception");
                }
                catch (WebApiClientException e)
                {
                    //assert
                    Assert.AreEqual(HttpStatusCode.Unauthorized, e.StatusCode);
                }
            }
        }

        [Test]
        public async Task TestNtlmGetAll()
        {
            //arrange
            CredentialCache cc = new CredentialCache();
            cc.Add(new Uri(options.BaseAddress), "NTLM", new NetworkCredential(userName, password));
            options.Authentication = new WindowsIntegratedAuthentication(cc);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                //act
                var restaurants = await client.GetManyAsync("GetWithAuth");

                //assert
                Assert.IsNotEmpty(restaurants);
                Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
            }
        }

        [Test]
        public async Task TestNtlmGetAllUnauthorized()
        {
            //arrange
            CredentialCache cc = new CredentialCache();
            cc.Add(new Uri(options.BaseAddress), "NTLM", new NetworkCredential(userName, "yyyy"));
            options.Authentication = new WindowsIntegratedAuthentication(cc);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                try
                {
                    //act
                    var restaurants = await client.GetManyAsync("GetWithAuth");
                    Assert.Fail("Unauthorized exception expected");
                }
                catch (WebApiClientException e)
                {
                    //assert
                    Assert.AreEqual(HttpStatusCode.Unauthorized, e.StatusCode);
                }
            }
        }

        [Test]
        public async Task TestBasicGetAll()
        {
            //arrange
            CredentialCache cc = new CredentialCache();
            cc.Add(new Uri(options.BaseAddress), "Basic", new NetworkCredential(userName, password));
            options.Authentication = new WindowsIntegratedAuthentication(cc);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                //act
                var restaurants = await client.GetManyAsync("GetWithAuth");

                //assert
                Assert.IsNotEmpty(restaurants);
                Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
            }
        }

        [Test]
        public async Task TestBasicGetAllUnauthorized()
        {
            //arrange
            CredentialCache cc = new CredentialCache();
            cc.Add(new Uri(options.BaseAddress), "Basic", new NetworkCredential(userName, "yyyy"));
            options.Authentication = new WindowsIntegratedAuthentication(cc);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                try
                {
                    //act
                    var restaurants = await client.GetManyAsync("GetWithAuth");
                    Assert.Fail("Unauthorized exception expected");
                }
                catch (WebApiClientException e)
                {
                    //assert
                    Assert.AreEqual(HttpStatusCode.Unauthorized, e.StatusCode);
                }
            }
        }
    }
}
