using CurrencyTracker.API.Contracts;
using CurrencyTracker.API.FilterModels;
using CurrencyTracker.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyTracker.API.Services
{
	public interface ICurrencyService
	{
		Task<CurrencyResponseModel> CurrencyRequester();
		Task<CurrencyResponseModel> GetCurrencies(CurrencyFilterModel filter);
	}
}
