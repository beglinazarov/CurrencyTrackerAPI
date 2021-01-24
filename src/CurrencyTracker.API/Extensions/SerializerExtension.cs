using CurrencyTracker.API.ViewModels;
using System;
using System.IO;
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
	}
}
