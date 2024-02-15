using Microsoft.AspNetCore.Mvc;

namespace HttpClientExample.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IConfiguration config;
        private readonly IWeatherService weatherService;

        public WeatherForecastController(IHttpClientFactory clientFactory, IConfiguration config, IWeatherService weatherService)
        {
            this.clientFactory = clientFactory;
            this.config = config;
            this.weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFromSimpleClient([FromQuery] string cityName)
        {
           var client = clientFactory.CreateClient();
           string apiKey = config.GetValue<string>("ApiKey");
           string url = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}&aqi=yes";
           var response = await client.GetAsync(url);
           return new JsonResult(await response.Content.ReadAsStringAsync());          
        }
        [HttpGet]
        public async Task<IActionResult> GetFromNamedClient([FromQuery] string cityName)
        {
            var client = clientFactory.CreateClient("weather");
            string apiKey = config.GetValue<string>("ApiKey");
            string url = $"?key={apiKey}&q={cityName}&aqi=yes";
            var response = await client.GetAsync(url);
            return new JsonResult(await response.Content.ReadAsStringAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetFromTypedClient([FromQuery] string cityName)
        {            
            return new JsonResult(await weatherService.GetWeatherData(cityName));
        }
    }
}
