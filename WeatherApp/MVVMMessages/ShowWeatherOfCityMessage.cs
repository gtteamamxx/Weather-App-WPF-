﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models.Weather;

namespace WeatherApp.MVVMMessages
{
    public class ShowWeatherOfCityMessage : MessageBase
    {
        public WeatherModel WeatherModel
        {
            get
            {
                return (WeatherModel)base.FirstObject;
            }
            set
            {
                base.FirstObject = value;
            }
        }
    }
}
