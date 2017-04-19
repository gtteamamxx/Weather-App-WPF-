using APIXULib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Models.Application;

namespace WeatherApp.ViewModels
{
    class CityWeatherPageViewModel : ViewModelBase
    {
        public CityWeatherPageViewModel()
        {
            MVVMMessagerService.RegisterReceiver<Location>(typeof(CityWeatherPageViewModel), ShowWeatherForCity);
        }

        private void ShowWeatherForCity(Location obj)
        {
            ;
        }
    }
}
