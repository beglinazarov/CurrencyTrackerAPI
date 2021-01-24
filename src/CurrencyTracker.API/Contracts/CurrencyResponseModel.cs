using CurrencyTracker.API.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CurrencyTracker.API.Contracts
{
	[Serializable, XmlRoot("Tarih_Date")]
	public class CurrencyResponseModel
	{
		[XmlAttribute]
		public string Tarih { get; set; }
		[XmlElement]
		public List<CurrencyViewModel> Currency { get; set; }
	}
	
}
