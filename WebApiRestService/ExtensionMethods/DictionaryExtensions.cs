using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiRestService
{
    /// <summary>
    /// Defines extension methods for the IDictionary type
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts the dictionary into a queryString type string
        /// </summary>
        /// <param name="values">The dictionary containing key/value pairs</param>
        /// <returns>A string formatted as ?param1=value1&amp;param2=value2</returns>
        public static string ToQueryString(this IDictionary<string, object> values)
        {
            string queryString = string.Empty;

            if (values != null && values.Count > 0)
            {
                queryString += "?";

                foreach (var item in values)
                {
                    //Each property should be in the query string format
                    queryString += string.Format("{0}={1}&", item.Key, item.Value);
                }

                //Remove the last & sign
                queryString = queryString.TrimEnd('&');
            }

            return queryString;
        }
    }
}
