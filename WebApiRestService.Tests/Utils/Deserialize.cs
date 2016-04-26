using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WebApiRestService.Tests
{
	public static class Deserialize
	{
		public static T Parse<T>(this string str, ContentType format) where T : class
		{
			switch (format)
			{
                case ContentType.Xml:
					return str.ParseXml<T>();

                case ContentType.Json:
				default:
					return str.ParseJson<T>();
			}
		}

		public static Stream ToStream(this string str)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(str);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public static T ParseXml<T>(this string str) where T : class
		{
			var reader = XmlReader.Create(str.Trim().ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
			return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
		}

		public static T ParseJson<T>(this string str) where T : class
		{
			return JsonConvert.DeserializeObject<T>(str.Trim());
		}
	}
}
