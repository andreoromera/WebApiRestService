using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace WebApiRestService
{
	/// <summary>
	/// Defines extension methods for the Object type
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Converts an anonymous type into a dictionary
		/// </summary>
		/// <param name="obj">The object to be transformed</param>
		/// <returns>A dictionary of KeyValuePairs</returns>
		/// <example>
		/// <code>
		/// var x = new { id = 1, name = "foo", lastName = "bar" };
		/// 
		/// IDictionary{string, object} values = x.ParseAnonymousObject();
		/// 
		/// foreach (var item in values)
		///	{
		///		Console.WriteLine(string.Format("{0}={1}", item.Key, item.Value));
		///	}
		///	</code>
		/// </example>
		public static IDictionary<string, object> ParseAnonymousObject(this object obj)
		{
			var dic = from property in obj.GetType().GetRuntimeProperties()
					  select new KeyValuePair<string, object>(property.Name, property.GetValue(obj));

			return dic.ToDictionary(d => d.Key, d => d.Value);
		}
	}
}
