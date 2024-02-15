namespace HttpClientExample
{
    public interface IWeatherService
    {
        Task<string> GetWeatherData(string cityName);
    }
    public class WeatherService : IWeatherService
    {
        private HttpClient httpClient;
        private readonly IConfiguration config;

        public WeatherService(HttpClient httpClient,IConfiguration config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }

        public async Task<string> GetWeatherData(string cityName)
        {
            string apiKey = config.GetValue<string>("ApiKey");
            string url = $"?key={apiKey}&q={cityName}&aqi=yes";
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
