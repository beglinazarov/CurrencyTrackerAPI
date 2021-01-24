using System;
using System.Xml.Serialization;

namespace CurrencyTracker.API.ViewModels
{
	[XmlRoot("Currency")]
	public class CurrencyViewModel
	{
		/*
		<Currency CrossOrder=\"0\" Kod=\"USD\" CurrencyCode=\"USD\">
			<Unit>1</Unit>
			<Isim>ABD DOLARI</Isim>
			<CurrencyName>US DOLLAR</CurrencyName>
			<ForexBuying>7.3985</ForexBuying>
			<ForexSelling>7.4118</ForexSelling>
			<BanknoteBuying>7.3933</BanknoteBuying>
			<BanknoteSelling>7.4229</BanknoteSelling>
			<CrossRateUSD/>
			<CrossRateOther/>
		</Currency>
		*/

		public string Unit { get; set; }

		public string CurrencyName { get; set; }

		public float ForexBuying { get; set; } = 0.00f;

		public string ForexSelling { get; set; }
		[XmlElement("BanknoteBuying")]
		public string BanknoteBuying { get; set; }
		public string BanknoteSelling { get; set; }

		public string CrossRateUSD { get; set; }
		public string CrossRateOther { get; set; }
	}
}
