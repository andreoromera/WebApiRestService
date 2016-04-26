using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiRestService.Api;

namespace WebApiRestService.Tests
{
	public class TestConfig
	{
		public WebApiClientOptions DefaultOptions { get; set; }

		public TestConfig()
		{
            this.DefaultOptions = new WebApiClientOptions(ConfigurationManager.AppSettings["baseAddress"], "restaurants");

			AutoMapperConfiguration.Configure();
		}
	}
}
