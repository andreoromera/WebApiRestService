using System;
using NUnit.Framework;
using WebApiRestService.Api.Database;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebApiRestService.Tests
{
    [TestFixture]
    [Category("Cookies")]
    public class TestCookies
    {
        private WebApiClientOptions options = new TestConfig().DefaultOptions;

        [Test]
        public async Task TestCookieUsingExplicitHeader()
        {
            int id = 1;

            var handler = new HttpClientHandler() { UseCookies = false };
            
            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, handler))
            {
                client.Headers.Add("Cookie", "restaurantId=" + id);

                var rest = await client.GetOneAsync("getUsingCookie");

                //assert
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

            }
        }

        [Test]
        public async Task TestCookieUsingExplicitContainer()
        {
            int id = 1;

            var handler = new HttpClientHandler() { UseCookies = true };
            
            var cc = new CookieContainer();
            cc.Add(new Cookie("restaurantId", id.ToString(), "/", "localhost"));
            handler.CookieContainer = cc;

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, handler))
            {
                var rest = await client.GetOneAsync("getUsingCookie");

                //assert
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

            }
        }

        [Test]
        public async Task TestCookieUsingImplicitContainer()
        {
            int id = 1;
            var handler = new HttpClientHandler() { UseCookies = true };

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, handler))
            {
                client.AddCookie(new Cookie("restaurantId", id.ToString(), "/", "localhost"));
                
                var rest = await client.GetOneAsync("getUsingCookie");

                //assert
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

            }
        }

        [Test]
        public async Task TestCookieUsingImplicitHeader()
        {
            int id = 1;
            var handler = new HttpClientHandler() { UseCookies = false };

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, handler))
            {
                client.AddCookie(new Cookie("restaurantId", id.ToString(), "/", "localhost"));

                var rest = await client.GetOneAsync("getUsingCookie");

                //assert
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

            }
        }

        [Test]
        public async Task TestCookieIgnoringHandler()
        {
            int id = 1;

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                client.AddCookie(new Cookie("restaurantId", id.ToString(), "/", "localhost"));

                var rest = await client.GetOneAsync("getUsingCookie");

                //assert
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

            }
        }

        [Test]
        public async Task TestCookieCollection()
        {
            int id = 1;

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                client.AddCookie(new Cookie("restaurantId", id.ToString(), "/", "localhost"));
                client.AddCookie(new Cookie("dummyCookie1", "dummy value 1", "/", "localhost"));
                client.AddCookie(new Cookie("dummyCookie2", "dummy value 2", "/", "localhost"));

                var rest = await client.GetOneAsync("getUsingCookie");

                //assert
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

            }
        }

        [Test]
        public async Task TestCookieGetFeedbackUsingImplicitContainer()
        {
            int id = 1;
            var handler = new HttpClientHandler() { UseCookies = true };

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, handler))
            {
                client.AddCookie(new Cookie("restaurantId", id.ToString(), "/", "localhost"));

                var rest = await client.GetOneAsync("getUsingCookie");
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

                var col = client.Handler.CookieContainer.GetCookies(new Uri(this.options.BaseAddress));
                Assert.IsNotEmpty(col);
                Assert.AreEqual("12345", col["session-id"].Value);
                Assert.AreEqual("11111", col["dummy1"].Value);
                Assert.AreEqual("22222", col["dummy2"].Value);
                Assert.AreEqual("33333", col["dummy3"].Value);
            }
        }

        [Test]
        public async Task TestCookieGetFeedbackUsingImplicitHeader()
        {
            int id = 1;
            var handler = new HttpClientHandler() { UseCookies = false };
            
            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options, handler))
            {
                client.AddCookie(new Cookie("restaurantId", id.ToString(), "/", "localhost"));

                var rest = await client.GetOneAsync("getUsingCookie");
                Assert.IsNotNull(rest);
                Assert.AreEqual(id, rest.Id);

                var col = handler.CookieContainer.GetCookies(new Uri(this.options.BaseAddress));
                Assert.IsEmpty(col, "It is not possible to get cookies when not using CookieContainer");
            }
        }

        [Test]
        public async Task TestCookieNotInformed()
        {
            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                try
                {
                    var rest = await client.GetOneAsync("getUsingCookie");
                }
                catch (WebApiClientException e)
                {
                    Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
                    Assert.IsNotNull(e.Details);
                    Assert.AreEqual("Cookie not present", e.Details.Message);
                }
            }
        }
        
        [Test]
        public async Task TestCookieNotFound()
        {
            int id = 9999;

            using (WebApiClient<Restaurant> client = new WebApiClient<Restaurant>(options))
            {
                client.AddCookie(new Cookie("restaurantId", id.ToString(), "/", "localhost"));

                try
                {
                    var rest = await client.GetOneAsync("getUsingCookie");
                }
                catch (WebApiClientException e)
                {
                    Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
                    Assert.IsNotNull(e.Details);
                    Assert.AreEqual("Restaurant not found", e.Details.Message);
                }
            }
        }
    }
}
