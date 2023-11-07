using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wheatherly.Models;  // Replace with your actual model classes.
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Wheatherly.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public class WeatherData
        {
            public Location Location { get; set; }
            public CurrentData Current { get; set; }

            public WeatherData()
            {
                Location = new Location();
                Current = new CurrentData();
            }
        }

        public class Location
        {
            public string Name { get; set; } = "";
            public string Region { get; set; } = "";
            public string Country { get; set; } = "";
            public double Lat { get; set; } = 0.0;
            public double Lon { get; set; } = 0.0;
            public string Tz_id { get; set; } = "";
            public long Localtime_epoch { get; set; } = 0;
            public string Localtime { get; set; } = "";
        }

        public class CurrentData
        {
            public long Last_updated_epoch { get; set; } = 0;
            public string Last_updated { get; set; } = "";
            public double Temp_c { get; set; } = 0.0;
            public double Temp_f { get; set; } = 0.0;
            public int Is_day { get; set; } = 0;
            public Condition Condition { get; set; } = new Condition();
            public double Wind_mph { get; set; } = 0.0;
            public double Wind_kph { get; set; } = 0.0;
            public int Wind_degree { get; set; } = 0;
            public string Wind_dir { get; set; } = "";
            public double Pressure_mb { get; set; } = 0.0;
            public double Pressure_in { get; set; } = 0.0;
            public double Precip_mm { get; set; } = 0.0;
            public double Precip_in { get; set; } = 0.0;
            public int Humidity { get; set; } = 0;
            public int Cloud { get; set; } = 0;
            public double Feelslike_c { get; set; } = 0.0;
            public double Feelslike_f { get; set; } = 0.0;
            public double Vis_km { get; set; } = 0.0;
            public double Vis_miles { get; set; } = 0.0;
            public double Uv { get; set; } = 0.0;
            public double Gust_mph { get; set; } = 0.0;
            public double Gust_kph { get; set; } = 0.0;
        }

        public class Condition
        {
            public string Text { get; set; } = "";
            public string Icon { get; set; } = "";
            public int Code { get; set; } = 0;
        }


        [HttpGet]
        public async Task<IActionResult> GetWeather(string location, string unit)
        {
            try
            {
                var apiKey = "a1569387c72e47d4a4f141504230611";
                var baseUrl = "https://api.weatherapi.com/v1/current.json";
                using var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync($"{baseUrl}?key={apiKey}&q={location}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(json);

                    if (weatherData != null && !string.IsNullOrEmpty(weatherData.Location?.Name))
                    {
                        return Ok(weatherData);
                    }
                    else
                    {
                        return BadRequest("Location not available in the weather data.");
                    }
                }

                return BadRequest("Failed to retrieve weather data from the external API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:", ex);
                return StatusCode(500, "An error occurred while fetching weather data.");
            }
        }


    }
}
