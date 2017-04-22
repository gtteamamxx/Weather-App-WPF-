using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models.Weather;

namespace WeatherApp.Models.Internet
{
    public class APIXUWeatherService
    {
        public const string API_KEY = "6636b95c6663415b913135418160412";

        public async static Task<List<Location>> GetAutoCompleteCityNamesAsync(string value)
        {
            string urlToCall = $"http://api.apixu.com/v1/search.json?key={API_KEY}&q={value}";

            using (WebClient webClient = new WebClient())
            {
                using (var streamReader = new StreamReader( await webClient.OpenReadTaskAsync(
                        new Uri(urlToCall)).ConfigureAwait(false)))
                {
                    string responseJson = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                    return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Location>>(responseJson)).ConfigureAwait(false);
                }
            }
        }

        public async static Task<WeatherModel> GetWeatherFromCityAsync(string cityName)
        {
            string urlToCall = $"http://api.apixu.com/v1/forecast.json?key={API_KEY}&q={cityName}&days=10";
            using (WebClient webClient = new WebClient())
            {
                using (var streamReader = new StreamReader(await webClient.OpenReadTaskAsync(
                        new Uri(urlToCall)).ConfigureAwait(false)))
                {
                    string responseJson = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                    return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WeatherModel>(responseJson)).ConfigureAwait(false);
                }
            }
        }
    }
}
