using APIXULib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Models.Application;
using WeatherApp.MVVMMessages;

namespace WeatherApp.ViewModels
{
    class CityWeatherPageViewModel : ViewModelBase
    {
        public CityWeatherPageViewModel()
        {
            MVVMMessagerService.RegisterReceiver<ShowWeatherOfCityMessage>(typeof(ShowWeatherOfCityMessage), ShowWeatherForCity);
        }

        private void ShowWeatherForCity(ShowWeatherOfCityMessage message)
        {
            Location cityToShow = message.City;

        }
    }
}
