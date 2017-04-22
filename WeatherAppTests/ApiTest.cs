using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models.Weather;

namespace WeatherAppTests
{
    [TestFixture]
    public class WeatherApiTest
    {
        [Test]
        public async Task check_if_search_autoresponse_service_is_returning_matches_valuesAsync()
        {
            if(!await WeatherApp.Models.Internet.Network.IsApiAvailable())
            {
                Assert.Pass("no internet");
                return;
            }

            string cityTest = "Wojkowice";
            int exceptedCitiesResult = 10;
            List<Location> testList = await WeatherApp.Models.Internet.APIXUWeatherService.GetAutoCompleteCityNamesAsync(cityTest);
            Assert.AreEqual(exceptedCitiesResult, testList.Count());
        }

        [Test]
        public async Task check_if_weather_is_downloading_correctly()
        {
            if (!await WeatherApp.Models.Internet.Network.IsApiAvailable())
            {
                Assert.Pass("no internet");
                return;
            }

            string cityTest = "Wojkowice";
            Assert.IsNotNull(await WeatherApp.Models.Internet.APIXUWeatherService.GetWeatherFromCityAsync(cityTest));
        }

        [Test]
        public async Task check_if_no_internet_functions_returns_false()
        {
            //before test please plug off internet cable
            Assert.AreEqual(false, await WeatherApp.Models.Internet.Network.IsInternetAvailableAsync());
        }
    }
}
