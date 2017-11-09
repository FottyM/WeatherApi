using System;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using WeatherApi2.Controllers;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApi2Test
{
    public class WeatherControllerUnitTests
    {
        [Fact]
        public  void Value_at_Get()
        {
         
            var controller = new WeatherController();
            var response = controller.GetAll("58.3829152", "26.7320442");
            Assert.Contains("Tartu", response.ToString());

        }
        

        [Fact]
        public void True_FACT(){
            Assert.Equal(2,2);
        }
        
    }
}
