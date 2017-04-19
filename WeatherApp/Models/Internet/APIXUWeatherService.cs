using APIXULib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Internet
{
    public class APIXUWeatherService
    {
        public async static Task<List<Location>> GetAutoCompleteCityNamesAsync(string value)
        {
            string urlToCall = $"http://api.apixu.com/v1/search.json?key=6636b95c6663415b913135418160412&q={value}";

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
    }
}
