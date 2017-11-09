using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using FluentAssertions;
using System.Net.Http.Headers;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WeatherApi2;

namespace WeatherApi2Test
{
    public class WeatherControllerIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public WeatherControllerIntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>() );
            _client = _server.CreateClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task Get_Friendly_Message_With_No_LonLat()
        {
            var response = await _client.GetAsync("/api/weather");
            response.EnsureSuccessStatusCode();
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            weath.Should().ContainKey("message");
            weath["message"].ToString().Should().Contain("now try");

        }

        [Fact]
        public async Task Return_Error_With_One_Query_Lon()
        {
            var response = await _client.GetAsync("/api/weather?lon=26.73321547");
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            weath.Should().ContainKey("error");
            weath["error"].ToString().Should().Be("Pleas check your querystring make sure you have lon and lat");
        }
        
        [Fact]
        public async Task Return_Error_With_One_Query_Lat()
        {
            var response = await _client.GetAsync("/api/weather?lat=26.73321547");
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            weath.Should().ContainKey("error");
            weath["error"].ToString().Should().Be("Pleas check your querystring make sure you have lon and lat");
        }
        
        
        [Fact]
        public async Task Return_Other_message_with_null_Lon_And_Lat()
        {
            var response = await _client.GetAsync("/api/weather?lat=&lon");
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            weath.Should().ContainKey("error");
            weath["error"].ToString().Should().Be("Pleas check your querystring make sure they are(correct) numbers: lat =>  lon => ");
        }
        
        [Fact]
        public async Task Containes_Error_message_with_lon_lon()
        {
            var response = await _client.GetAsync("/api/weather?lon=&lon");
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            weath.Should().ContainKey("error");
            weath["error"].ToString().Should().Be("Pleas check your querystring make sure you have lon and lat");

        }
        
        [Fact]
        public async Task Containes_Error_message_with_lat_lat()
        {
            var response = await _client.GetAsync("/api/weather?lat=&lat");
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            weath.Should().ContainKey("error");
            weath["error"].ToString().Should()
                .Be("Pleas check your querystring make sure you have lon and lat");
        }
        
        [Fact]
        public async Task Succeful_Call()
        {
            var response = await _client.GetAsync("/api/weather?lat=42.023949&lon=-93.647595");
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            var t1 = (double) weath["temperatureOne"];
            var t2 = (double) weath["temperatureTwo"];
            var avg =  weath["average"];
            
            Assert.Equal((t1 + t2)/ 2.0, avg );
            weath.Count.Should().Be(5);
            weath.Should().ContainKey("name")
                .And.ContainKey("average")
                .And.ContainKey("temperatureTwo")
                .And.ContainKey("temperatureOne");
        }

        [Fact]
        public async Task QuerryString_With_String_Instead_of_Number()
        {
            var response = await _client.GetAsync("/api/weather?lon=343&lat=mutunda");
            var resposnseString = await response.Content.ReadAsStringAsync();
            var weath = JObject.Parse(resposnseString);
            weath.Should().ContainKey("error");
            weath["error"].ToString().Should()
                .Contain("Pleas check your querystring make sure they are(correct) numbers: lat =>");
        }







    }
}