using System;
using NUnit.Framework;
using WebApiRestService.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiRestService;
using System.Net;
using System.Web.Http;
using Dto = WebApiRestService.Dto;
using WebApiRestService.Api.Database;

namespace WebApiRestService.Tests
{
    [TestFixture]
    [Category("Json")]
    [Category("Json.Gets")]
    public class TestJsonGetCalls
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
        public async Task TestGetAll()
        {
            //act
            List<Restaurant> restaurants = await client.GetManyAsync("get");

            //assert
            Assert.IsNotEmpty(restaurants);
            Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
        }

        [Test]
        public async Task TestGetAllWithNoElements()
        {
            //act
            List<Restaurant> restaurants = await client.GetManyAsync("GetAllWithNoElements");

            //assert
            Assert.IsEmpty(restaurants);
            Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
        }

        [Test]
        public async Task TestGetAllWithError()
        {
            try
            {
                //act
                List<Restaurant> restaurants = await client.GetManyAsync("GetAllWithError");

                Assert.Fail("WebApiException expected");
            }
            catch (WebApiClientException e)
            {
                Console.WriteLine(e.Message);
                Assert.AreEqual(HttpStatusCode.InternalServerError, e.StatusCode);
            }
        }

        [Test]
        public async Task TestGet()
        {
            //arrange
            int id = 1;

            //act
            var rest = await this.client.GetOneAsync(id, "get");

            //assert
            Assert.IsNotNull(rest);
            Assert.AreEqual(id, rest.Id);
        }

        [Test]
        public async Task TestGetInvalidId()
        {
            //arrange
            int id = 9999;

            try
            {
                //act
                var rest = await this.client.GetOneAsync(id, "get");

                Assert.Fail("WebApiException expected");
            }
            catch (WebApiClientException e)
            {
                Console.WriteLine(e.Message);
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }

        [Test]
        public async Task TestGetGeneralError()
        {
            //arrange
            int id = 9999;

            try
            {
                //act
                var rest = await this.client.GetOneAsync(id, "GetWithError");

                Assert.Fail("WebApiException expected");
            }
            catch (WebApiClientException e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
                Assert.IsNotNull(e.Details);
                Assert.AreEqual("Invalid Id", e.Details.Reason);
                Assert.AreEqual(string.Format("The id {0} was not found in the database", id), e.Details.Message);

                Console.WriteLine(e.Details);
            }
        }

        [Test]
        public void TestGetString()
        {
            string values = "";
            var options = new TestConfig().DefaultOptions;
            options.Controller = "values";

            using (WebApiClient<string> client = new WebApiClient<string>(options))
            {
                values = client.GetOneAsync(1).Result;
            }

            Assert.IsNotNullOrEmpty(values);
        }
    }
}
