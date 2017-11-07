using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    
    {
        private readonly WebClient _webClient = new WebClient();
        
        [HttpGet]
        public IActionResult GetAll([FromQuery] double lon, [FromQuery] double lat )
        {
            /*
             * 1. Get location by lat and long from both acc and map
             * 2. Parse the two to extract the temperatures
             * 3. Find the average of the two temps
             * 4. Return the data to the user in json acc mam and avg 
             */
            
            dynamic res = JsonConvert.DeserializeObject(GetTemperatureByLocation(lat, lon).Result);
            return new ObjectResult(res["main"]["temp"]);
            
        }

        [HttpGet("{id}")]
        public IActionResult GetById()
        {
            string[] strings = {"Balance", "Mutunda"};
            return new ObjectResult(strings);
        }

        private async Task<string> GetTemperatureByLocation(double lon, double lat)
        {
            var openWeatherMap = await _webClient.DownloadStringTaskAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&APPID=2dbf2d5f4c4ca8b8ec9d0ac3e13faed2&units=imperial");   
            return openWeatherMap;
        }
        
        
    }
}