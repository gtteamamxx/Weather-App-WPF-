using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WeatherApp.Models;
using WeatherApp.Models.Application;
using WeatherApp.Models.Internet;
using WeatherApp.Models.Weather;
using WeatherApp.MVVMMessages;
using WeatherApp.Views;

namespace WeatherApp.ViewModels
{
    class CityWeatherPageViewModel : ViewModelBase
    {
        public CityWeatherPageViewModel()
        {
            MVVMMessagerService.RegisterReceiver<ShowWeatherOfCityMessage>(typeof(ShowWeatherOfCityMessage), ShowWeatherForCity);
        }
        private WeatherModel _weather;
        public WeatherModel Weather
        {
            get => _weather;
            set => OnPropertyChanged(ref _weather, value);
        }
        private CityWeatherPage _cityWeatherPage;

        #region Commands
        private RelayCommand<object> _pageLoadedCommand;
        public RelayCommand<object> PageLoadedCommand =>
            _pageLoadedCommand ?? (_pageLoadedCommand = new RelayCommand<object>(o => PageLoaded(o)));
        #endregion

        #region Properties

        #endregion

        private void PageLoaded(object page)
        {
            _cityWeatherPage = (dynamic)page;
            SetMainGridVisibility(Visibility.Collapsed);
        }

        private void LoadPage()
        {
            SetMainGridVisibility(Visibility.Visible);
            SetPageContent();
            StartLoadAnimation();
        }

        private void SetPageContent()
        {
           
        }

        private void StartLoadAnimation()
        {
            ((dynamic)_cityWeatherPage.Resources["PageLoadedGridAnimation"]).Begin();
        }

        private async void ShowWeatherForCity(ShowWeatherOfCityMessage message)
        {
            Weather = message.WeatherModel;
            Weather = await APIXUWeatherService.GetWeatherFromCityAsync(Weather.location.name);
            LoadPage();
        }

        private void SetMainGridVisibility(Visibility vis)
        {
            (((dynamic)_cityWeatherPage).Content).Parent.Visibility = vis;
        }
    }
}
