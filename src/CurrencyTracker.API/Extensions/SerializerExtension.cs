using CurrencyTracker.API.ViewModels;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace CurrencyTracker.API.Extensions
{
	public static class SerializerExtension
	{
		public static T Deserialize<T>(Stream stream) 
		{
			if (stream == null) 
			{
				throw new ArgumentNullException(nameof(stream));
			}

			using (stream)
			{
				var serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(XmlReader.Create(stream));
			}
		}

		public static Stream Serialize<T>(T Content)
		{
			if (Content == null)
			{
				throw new ArgumentNullException(nameof(Content));
			}
			var setting = new XmlWriterSettings
			{
				Indent = false,
				NewLineHandling = NewLineHandling.None
			};
			var serializer = new XmlSerializer(typeof(T));
			var ms = new MemoryStream();
			using (var sw = new StreamWriter(ms, Encoding.UTF8, 1024, true))
			using (var xmlWriter = XmlWriter.Create(sw,setting))
			{
				serializer.Serialize(xmlWriter,Content);
				xmlWriter.Flush();
			}
			ms.Seek(0, SeekOrigin.Begin);
			return ms;
		}
	}
}
