using CurrencyTracker.API.Extensions;
using CurrencyTracker.API.FilterModels;
using CurrencyTracker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.API.Controllers
{
	[ApiController]
	[Route("[action]")]
	public class CurrencyTrackerApiController : ControllerBase
	{
		private readonly ILogger<CurrencyTrackerApiController> _logger;
		private readonly ICurrencyService _currencyService;

		public CurrencyTrackerApiController(
			ILogger<CurrencyTrackerApiController> logger,
			ICurrencyService currencyService
			)
		{
			_logger = logger;
			_currencyService = currencyService;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GetCurrencies(CurrencyFilterModel filter)
		{
			var response = await _currencyService.GetCurrencies(filter);

			if (filter.DownloadResponseInExportedFile)
			{
				if (response != null && !filter.ExportFormat.Equals("") && !filter.ExportFormat.Equals("string"))
				{
					
					if (filter.ExportFormat.ToLower().Equals("csv"))
					{
						return File(
							Encoding.UTF8.GetBytes(ExportResponseFormatterExtension.ExportToCsv(response.Currency, filter.ExportFilePath).ToString()),
							"text/csv", "currency.csv");
					}
					if (filter.ExportFormat.ToLower().Equals("xl"))
					{
						return File(
						ExportResponseFormatterExtension.ExportToXL(response.Currency, filter.ExportFilePath),
						"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"currency.xlsx");
					}
					if (filter.ExportFormat.ToLower().Equals("xml"))
					{
						return File(
							ExportResponseFormatterExtension.ExportToXML(response.Currency, filter.ExportFilePath),
							 "application/xml", "currency.xml");
					}
				}
			}

			return Ok(response);
		}

	}
}
