using CurrencyTracker.API.FilterModels;
using CurrencyTracker.API.Services;
using CurrencyTracker.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

		[HttpPost]
		public async Task<IActionResult> GetCurrencies(CurrencyFilterModel filter)
		{
			var response = await _currencyService.GetCurrencies(filter);
	
			return Ok(response);
		}
	}
}
