using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Weather
{
    public class WeatherModel
    {
        public Current current { get; set; }
        public Forecast forecast { get; set; }
        public Location location { get; set; }
    }
}
