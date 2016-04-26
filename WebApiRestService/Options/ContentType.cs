using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiRestService
{
	/// <summary>
	/// Defines the content type for the http request and response 
	/// </summary>
	public enum ContentType
	{
		/// <summary>
		/// Uses the Json content type
		/// </summary>
		Json,

		/// <summary>
		/// Uses the Xml content type
		/// </summary>
		Xml
	}
}
