using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase

    {
        private readonly WebClient _webClient = new WebClient();

        private const string OpenWeatherMapUrl =
            "https://api.openweathermap.org/data/2.5/weather?APPID=2dbf2d5f4c4ca8b8ec9d0ac3e13faed2&units=imperial&";

        private const string DarkSky = "https://api.darksky.net/forecast/b9a41cd9c98e4230e85bd9e96629757c/";

        [HttpGet]
        public IActionResult GetAll([FromQuery] string lat, [FromQuery] string lon)
        {
            var regx = new Regex(@"^[+-]?[0-9]{1,9}(?:\.[0-9]{1,9})?$");
            var errorMessage = new Dictionary<string, string>()
            {
                {
                    "error",
                    $"Pleas check your querystring make sure they are (correct) numbers: lat => {lat} lon => {lon}"
                }
            };

            lon = lon.Trim();
            lat = lat.Trim();

            if (regx.IsMatch(lat) && regx.IsMatch(lon))
            {
                return new ObjectResult(JsonConvert.DeserializeObject(GetTemperatureByLocation(lat, lon).Result));
            }
            
                return new NotFoundObjectResult(errorMessage);
            
        }


        private async Task<string> GetTemperatureByLocation(string lat, string lon)
        {
            string[] urls = {OpenWeatherMapUrl, DarkSky};
            var sb = new StringBuilder();
            dynamic darkCloud = null;
            dynamic openMap = null;
            var average = 0.0;


            foreach (var url in urls)
            {
                if (url.Equals(""))
                {
                    return null;
                }
                else if (url.Contains("openweathermap"))
                {
                    openMap = await _webClient.DownloadStringTaskAsync($"{OpenWeatherMapUrl}lat={lat}&lon={lon}");
                    openMap = JObject.Parse(openMap);
                }
                else
                {
                    darkCloud = await _webClient.DownloadStringTaskAsync($"{DarkSky}{lat},{lon}");
                    darkCloud = JObject.Parse(darkCloud);
                }
            }

            average = CompareTempsAndFindAvg(darkCloud, openMap);

            sb.Append("{").Append($"temperatureOne: {darkCloud.currently.temperature},")
                .Append($"temperatureTwo: {openMap.main.temp},")
                .Append($"average: {average}, ").Append($"timezone:\"{darkCloud.timezone}\",")
                .Append($"name:\"{openMap.name}\"").Append("}");

            return sb.ToString();
        }


        private static double CompareTempsAndFindAvg(dynamic darkCloud, dynamic openMap)
        {
            double x = darkCloud.currently.temperature;
            double y = openMap.main.temp;

            if (x != 0 && y != 0)
            {
                return (y + x) / 2.0;
            }

            return 0;
        }
    }
}