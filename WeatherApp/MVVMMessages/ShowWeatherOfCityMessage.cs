using APIXULib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.MVVMMessages
{
    public class ShowWeatherOfCityMessage : MessageBase
    {
        public Location City
        {
            get
            {
                return (Location)base.FirstObject;
            }
            set
            {
                base.FirstObject = value;
            }
        }
    }
}
