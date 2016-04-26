using WebApiRestService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using WebApiRestService.Api.Database;
using System.Data.Entity;
using System.Net.Http.Headers;
using System.Web;

namespace WebApiRestService.Api.Controllers
{
	public class RestaurantsController : ApiController
	{
		DatabaseContext context = new DatabaseContext();

        public IHttpActionResult Get()
		{
			var rests = context.Restaurants.AsEnumerable();

			return Ok(rests);
		}

        [Authorize]
        public IHttpActionResult GetWithAuth()
        {
            var rests = context.Restaurants.AsEnumerable();

            return Ok(rests);
        }

        public IHttpActionResult GetByCountry(int countryId)
		{
			var rests = from r in context.Restaurants
						where r.CountryId == countryId
						select r;

			return Ok(rests);
		}

        public IHttpActionResult GetFirstRestaurantByCountry(int countryId)
		{
			var rests = from r in context.Restaurants
						where r.CountryId == countryId
						select r;

			return Ok(rests.FirstOrDefault());
		}

        public IHttpActionResult GetByCountryAndNumberOfReviews(int countryId, int minReviews)
		{
			var rests = from r in context.Restaurants.Include("Reviews")
						where r.CountryId == countryId && r.Reviews.Count >= minReviews
						select r;

			return Ok(rests);
		}

        public IHttpActionResult GetDto()
		{
			var rests = context.Restaurants.AsEnumerable();

			return Ok(Mapper.Map<IEnumerable<Dto.Restaurant>>(rests));
		}

        public IHttpActionResult GetAllWithError()
		{
            throw new InvalidProgramException("GetAllWithError generated an exception");
		}

        public IHttpActionResult GetAllWithNoElements()
		{
			var rests = context.Restaurants.Where(r => r.Id == 0);

			return Ok(rests);
		}

        public IHttpActionResult GetWithError(int id)
		{
            var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(string.Format("The id {0} was not found in the database", id)),
                ReasonPhrase = "Invalid Id"
            };
            
            throw new HttpResponseException(message);
        }

		public IHttpActionResult Get(int id)
		{
			var rest = context.Restaurants.Find(id);

			if (rest == null)
			{
				return NotFound();
			}

            return Ok(rest);
		}
        
        public HttpResponseMessage GetUsingCookie()
        {
            var cookie = Request.Headers.GetCookies("restaurantId").SingleOrDefault();
            
            if (cookie == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cookie not present");
            }

            int id = Convert.ToInt32(cookie["restaurantId"].Value);

            var rest = context.Restaurants.Find(id);

            if (rest == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Restaurant not found");
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, rest);

            response.Headers.AddCookies(new CookieHeaderValue[] { 
                            new CookieHeaderValue("session-id", "12345") { Expires = DateTimeOffset.Now.AddDays(1), Domain = Request.RequestUri.Host, Path = "/" },
                            new CookieHeaderValue("dummy1", "11111") { Expires = DateTimeOffset.Now.AddDays(1), Domain = Request.RequestUri.Host, Path = "/" },
                            new CookieHeaderValue("dummy2", "22222") { Expires = DateTimeOffset.Now.AddDays(1), Domain = Request.RequestUri.Host, Path = "/" },
                            new CookieHeaderValue("dummy3", "33333") { Expires = DateTimeOffset.Now.AddDays(1), Domain = Request.RequestUri.Host, Path = "/" }
            });
            
            return response;
        }

        public IHttpActionResult GetDto(int id)
		{
			var rest = context.Restaurants.Include(r => r.Country).SingleOrDefault(r => r.Id == id);

			if (rest == null)
			{
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("The id {0} was not found in the database", id)),
                    ReasonPhrase = "Invalid Id"
                };

                throw new HttpResponseException(message);
			}

			return Ok(Mapper.Map<Dto.Restaurant>(rest));
		}
		
		public IHttpActionResult Post(Restaurant restaurant)
		{
			if (!ModelState.IsValid)
			{
                return BadRequest(ModelState);
            }
            
			try
			{
				context.Restaurants.Add(restaurant);
				context.SaveChanges();

                return Created(Url.Link("DefaultApiWithoutId", new { controller = "restaurants" }), restaurant);
			}
			catch (Exception e)
			{
                return InternalServerError(e);
			}
		}

        public IHttpActionResult Put(Restaurant restaurant)
		{
			if (!ModelState.IsValid)
			{
                return BadRequest(ModelState);
			}

			try
			{
                context.Entry(restaurant).State = EntityState.Modified;
				context.SaveChanges();

                return Ok(restaurant);
			}
			catch(Exception e)
			{
                return InternalServerError(e);
			}
		}

		public IHttpActionResult Delete(int id)
		{
			var rest = context.Restaurants.Find(id);

			if (rest == null)
			{
                return NotFound();
			}

			try
			{
				context.Restaurants.Remove(rest);
				context.SaveChanges();

                return Ok();
			}
			catch (Exception e)
			{
                return InternalServerError(e);
			}
		}

		protected override void Dispose(bool disposing)
		{
			context.Dispose();
			base.Dispose(disposing);
		}
	}
}