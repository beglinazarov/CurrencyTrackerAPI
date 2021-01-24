namespace CurrencyTracker.API.FilterModels
{
	public class CurrencyFilterModel
	{
		public string CurrencyName { get; set; }
		public string SortByForexBuying { get; set; }
		public string SortByForexSelling { get; set; }
		public string ExportFormat { get; set; }
		public string ExportFilePath { get; set; }
		public bool DownloadResponseInExportedFile { get; set; }
	}
}
