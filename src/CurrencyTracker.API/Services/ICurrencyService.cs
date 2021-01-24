using CurrencyTracker.API.Contracts;
using CurrencyTracker.API.FilterModels;
using System.Threading.Tasks;

namespace CurrencyTracker.API.Services
{
	public interface ICurrencyService
	{
		Task<CurrencyResponseModel> CurrencyRequester();
		Task<CurrencyResponseModel> GetCurrencies(CurrencyFilterModel filter);
	}
}
