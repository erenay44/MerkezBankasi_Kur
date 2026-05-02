namespace Currency.DataApi.Services
{
	public interface ITcmbDataService{

		Task FetchAndSaveLastTwoMonthsRatesAsync();
	}


}