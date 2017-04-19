using System.Net;
using System.Threading.Tasks;

namespace WeatherApp.Models.Internet
{
    public class Network
    {
        private const string _API_URL = "https://www.apixu.com";
        private const string _TEST_URL = "http://google.pl";

        public static async Task<bool> IsInternetAvailableAsync(string url = _TEST_URL)
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (await client.OpenReadTaskAsync(url))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> IsApiAvailable()
        {
            return await IsInternetAvailableAsync(_API_URL);
        }
    }
}