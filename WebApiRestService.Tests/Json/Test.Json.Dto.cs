﻿using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiRestService;
using System.Net;
using System.Web.Http;
using WebApiRestService.Dto;

namespace WebApiRestService.Tests
{
	[TestFixture]
	[Category("Json")]
	[Category("Json.Dtos")]
	public class TestJsonDtoCalls
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
		public async Task TestDtoGetAll()
		{
			//act
			List<Restaurant> restaurants = await client.GetManyAsync("getDto");

			//assert
			Assert.IsNotEmpty(restaurants);
			Console.WriteLine(string.Format("Number of restaurants returned: {0}", restaurants.Count));
		}

		[Test]
		public async Task TestDtoGet()
		{
			//arrange
			int id = 1;

			//act
			var rest = await this.client.GetOneAsync(id, "getDto");

			//assert
			Assert.IsNotNull(rest);
			Assert.AreEqual(id, rest.Id);
			Assert.AreEqual("restaurant 1", rest.Name.ToLower());
		}

		[Test]
		public async Task TestDtoChildObject()
		{
			//arrange
			int id = 1;

			//act
			var rest = await this.client.GetOneAsync(id, "getDto");

			//assert
			Assert.IsNotNull(rest);
			Assert.AreEqual(id, rest.Id);
			Assert.IsNotNull(rest.Country);
			Assert.AreEqual(4, rest.Country.Id);
		}
	}
}
