using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.KeyVault.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    
    {
        private readonly WebClient _webClient = new WebClient();
        private const string OpenWeatherMapUrl = "https://api.openweathermap.org/data/2.5/weather?APPID=2dbf2d5f4c4ca8b8ec9d0ac3e13faed2&units=imperial&";
        private const string DarkSky = "https://api.darksky.net/forecast/b9a41cd9c98e4230e85bd9e96629757c/";

        [HttpGet]
        public IActionResult GetAll([FromQuery] QueryString  queryString )
        {
            /*
             * 1. Get location by lat and long from both acc and map
             * 2. Parse the two to extract the temperatures
             * 3. Find the average of the two temps
             * 4. Return the data to the user in json acc mam and avg 
             */
            
            dynamic res = JsonConvert.DeserializeObject(GetTemperatureByLocation( queryString.lat , queryString.lon ).Result);
            return new ObjectResult(res);
            
        }

        
        private async Task<string> GetTemperatureByLocation(double lat, double lon)
        {
            string[] urls = {OpenWeatherMapUrl, DarkSky};
            dynamic darkCloud = "";
            dynamic openMap = "";

            foreach (var url in urls)
            {

                if (url.Equals(""))
                {
                     return null;
                }
                else if (url.Contains("openweathermap"))
                {
                    openMap = await _webClient.DownloadStringTaskAsync($"{OpenWeatherMapUrl}lat={lat}&lon={lon}");
                }
                else
                {
                    darkCloud =  await _webClient.DownloadStringTaskAsync($"{DarkSky}{lat},{lon}");
                }

            }

            darkCloud = JObject.Parse(darkCloud);
            openMap = JObject.Parse(openMap);
            
            var average = CompareTempsAndFindAvg(darkCloud, openMap);

            return "{ temperatureOne:" + darkCloud.currently.temperature + ", temperatureTwo:" + openMap.main.temp + ", average:"+ average  + " }";

        }



        private static double CompareTempsAndFindAvg(dynamic darkCloud, dynamic openMap)
        {
            
            double x = darkCloud.currently.temperature;
            double y = openMap.main.temp;
            
            if (Math.Abs(x) > 0 && Math.Abs(y) > 0)
            {
                return Math.Abs((y + x) / 2);
            }

            return 0;
        }


    }

    public class QueryString
    {
        [BindRequired]
        public double lat { get; set; }
        public double lon { get; set; }
    }
}