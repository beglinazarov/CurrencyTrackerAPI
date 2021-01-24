using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CurrencyTracker.API.Contracts;
using CurrencyTracker.API.Extensions;
using CurrencyTracker.API.FilterModels;
using CurrencyTracker.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyTracker.API.Services
{
	public class CurrencyManager : ICurrencyService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public CurrencyManager(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
		}

		public async Task<CurrencyResponseModel> CurrencyRequester()
		{
			string baseUrl = "https://www.tcmb.gov.tr";
			string requestUrl = $"https://www.tcmb.gov.tr/kurlar/today.xml";

			var httpClient = _httpClientFactory.CreateClient();
			httpClient.BaseAddress = new Uri($"{ baseUrl}");

			var httpResponse = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);

			httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

			if (httpResponse.Content is object)
			{
				var contentStream = httpResponse.Content.ReadAsStream();

				try
				{
					return SerializerExtension.Deserialize<CurrencyResponseModel>(contentStream);
				}
				catch (JsonException)
				{
					Console.WriteLine("Invalid JSON.");
				}
			}
			else
			{
				Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
			}


			return null;
		}


		public async Task<CurrencyResponseModel> GetCurrencies(CurrencyFilterModel filter)
		{
			var Currencies = await CurrencyRequester();

			// Filtering By CurrencyName
			if (!filter.CurrencyName.Equals("") && !filter.CurrencyName.Equals("string")) 
			{
				return await Task.FromResult(new CurrencyResponseModel
				{
					Tarih = Currencies.Tarih,
					Currency = Currencies.Currency
						.Where(x => x.CurrencyName == filter.CurrencyName).ToList(),
				});
			}

			// SortByForexBuying
			if (filter.SortByForexBuying.Equals("asc") || filter.SortByForexBuying.Equals("desc"))
			{
				return await Task.FromResult(new CurrencyResponseModel
				{
					Tarih = Currencies.Tarih,
					Currency =	filter.SortByForexBuying.Equals("asc") ?
									Currencies.Currency.OrderBy(x => x.ForexBuying).ToList() :
								filter.SortByForexBuying.Equals("desc")?
									Currencies.Currency.OrderByDescending(desc => desc.ForexBuying).ToList() : 
								Currencies.Currency.ToList()
				});
			}
			
			// SortByForexSelling
			if (filter.SortByForexSelling.Equals("asc") || filter.SortByForexSelling.Equals("desc"))
			{
				return await Task.FromResult(new CurrencyResponseModel
				{
					Tarih = Currencies.Tarih,
					Currency = filter.SortByForexSelling.Equals("asc") ?
									Currencies.Currency.OrderBy(x => x.ForexSelling).ToList() :
								filter.SortByForexSelling.Equals("desc") ?
									Currencies.Currency.OrderByDescending(desc => desc.ForexSelling).ToList() :
								Currencies.Currency.ToList()
				});
			}

			return await Task.FromResult(Currencies);
		}
	}
}
