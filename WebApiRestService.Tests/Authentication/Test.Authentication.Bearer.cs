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

namespace WebApiRestService.Tests
{
	[TestFixture]
	[Category("Authentication")]
	public class TestBearerAuthentication
	{
        private const string userName = "testUser";
        private const string password = "123456";
        private string tokenUri;
        private WebApiClientOptions options = new TestConfig().DefaultOptions;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            this.tokenUri = Path.Combine(this.options.BaseAddress, "token");
        }

        [Test]
        public async Task TestBearerAuthGetAll()
		{
            //arrange
            options.Authentication = new BearerTokenAuthentication(userName, password, this.tokenUri);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                //act
                List<Restaurant> restaurants = await client.GetManyAsync("GetWithAuth");

                //assert
                Assert.IsNotEmpty(restaurants);
                Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
            }
		}

        [Test]
        public async Task TestBearerAuthGetAllUnauthorized()
        {
            //arrange
            options.Authentication = new BearerTokenAuthentication("xxx", "yyy", this.tokenUri);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                try
                {
                    //act
                    List<Restaurant> restaurants = await client.GetManyAsync("GetWithAuth");
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
        public async Task TestBearerAuthGetAllInvalidTokenUri()
        {
            //arrange
            options.Authentication = new BearerTokenAuthentication(userName, password, "http://localhost/api/invalidTokenUri");

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                try
                {
                    //act
                    List<Restaurant> restaurants = await client.GetManyAsync("GetWithAuth");
                    Assert.Fail("Expected ServiceUnavailable exception");
                }
                catch (WebApiClientException e)
                {
                    //assert
                    Assert.AreEqual(HttpStatusCode.ServiceUnavailable, e.StatusCode);
                }
            }
        }

        [Test]
        public async Task TestBearerAuthGetAllSameToken()
        {
            //arrange
            options.Authentication = new BearerTokenAuthentication(userName, password, this.tokenUri);

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                //act
                List<Restaurant> restaurants = await client.GetManyAsync("GetWithAuth");
                Assert.IsNotEmpty(restaurants);

                var auth = options.Authentication as BearerTokenAuthentication;
                Assert.IsTrue(auth.HasToken);
                Assert.IsNotNullOrEmpty(auth.Token);
                
                restaurants = await client.GetManyAsync("Get");
                Assert.IsNotEmpty(restaurants);
            }
        }

        [Test]
        public async Task TestBearerAuthGetAllDifferentTokens()
        {
            //arrange
            var auth1 = new BearerTokenAuthentication(userName, password, this.tokenUri);
            var auth2 = new BearerTokenAuthentication(userName, password, this.tokenUri);

            options.Authentication = auth1;
            using (WebApiClient<Restaurant> client1 = new WebApiClient<Restaurant>(options))
            {
                //act
                List<Restaurant> restaurants = await client1.GetManyAsync("GetWithAuth");
                
                Assert.IsNotEmpty(restaurants);
                Assert.IsTrue(auth1.HasToken);
                Assert.IsNotNullOrEmpty(auth1.Token);
            }

            options.Authentication = auth2;
            using (WebApiClient<Restaurant> client2 = new WebApiClient<Restaurant>(options))
            {
                //act
                List<Restaurant> restaurants = await client2.GetManyAsync("GetWithAuth");
                Assert.IsNotEmpty(restaurants);

                Assert.IsTrue(auth2.HasToken);
                Assert.IsNotNullOrEmpty(auth2.Token);
            }
            
            Assert.AreNotEqual(auth1.Token, auth2.Token);
        }
    }
}
