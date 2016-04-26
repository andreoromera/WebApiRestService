using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WebApiRestService
{
	/// <summary>
	/// Defines extension methods for the Type type
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Verifies whether it is an anonymous type object
		/// </summary>
		/// <param name="type">The object type to be tested</param>
		/// <returns>true if it is an anonymous type object</returns>
		/// <example>
		/// <code>
		/// var x = new { id = 1, name = "foo", lastName = "bar" };
		/// 
		/// bool result = x.GetType().IsAnonymousType(); //true
		/// </code>
		/// </example>
		public static Boolean IsAnonymousType(this Type type)
		{
			Boolean hasCompilerGeneratedAttribute = type.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() != null;
			Boolean nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
			Boolean isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

			return isAnonymousType;
		}
	}
}
