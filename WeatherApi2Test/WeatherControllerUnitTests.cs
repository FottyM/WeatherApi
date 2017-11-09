using System;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using WeatherApi2.Controllers;
using Moq;
using FluentAssertions;

namespace WeatherApi2Test
{
    public class WeatherControllerUnitTests
    {
        [Fact]
        public  void Value_at_Get()
        {
            var controller = new WeatherController();
            var response =  controller.GetAll("-23.1234", "23.122");
            response.Should().NotBeNull();
        }

        [Fact]
        public void True_FACT(){
            Assert.Equal(2,2);
        }
        
    }
}
