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
        #region Init
        public CityWeatherPageViewModel()
        {
            MVVMMessagerService.RegisterReceiver<ShowWeatherOfCityMessage>(typeof(ShowWeatherOfCityMessage), ShowWeatherForCityAsync);
        }

        private WeatherModel _weather;
        public WeatherModel Weather
        {
            get => _weather;
            set => OnPropertyChanged(ref _weather, value);
        }
        private CityWeatherPage _cityWeatherPage;

        #endregion

        #region Commands
        private RelayCommand<object> _pageLoadedCommand;
        public RelayCommand<object> PageLoadedCommand =>
            _pageLoadedCommand ?? (_pageLoadedCommand = new RelayCommand<object>(o => PageLoaded(o)));
        #endregion

        #region Properties

        private List<Forecastday> _forecastDays;
        public List<Forecastday> ForecastDays
        {
            get => _forecastDays;
            set => OnPropertyChanged(ref _forecastDays, value);
        }

        private List<Hour> _selectedForecastDayHours;
        public List<Hour> SelectedForecastDayHours
        {
            get => _selectedForecastDayHours;
            set => OnPropertyChanged(ref _selectedForecastDayHours, value);
        }

        private List<Hour> _currentDayHours;
        public List<Hour> CurrentDayHours
        {
            get => _currentDayHours;
            set => OnPropertyChanged(ref _currentDayHours, value);
        }

        private Forecastday _selectedForecastDay;
        public Forecastday SelectedForecastDay
        {
            get => _selectedForecastDay;
            set
            {
                OnPropertyChanged(ref _selectedForecastDay, value);
                SelectedForecastDayHours = value?.hour;
            }
        }

        private bool _isProgressRingActive;
        public bool IsProgressRingActive
        {
            get => _isProgressRingActive;
            set => OnPropertyChanged(ref _isProgressRingActive, value);
        }
        #endregion

        private void PageLoaded(object page)
        {
            _cityWeatherPage = (dynamic)page;
            SetMainGridVisibility(Visibility.Collapsed);
        }

        private void LoadPage()
        {
            SetPageContent();
            SetMainGridVisibility(Visibility.Visible);
            IsProgressRingActive = false;
            StartLoadAnimation();
        }

        private void SetPageContent()
        {
            ForecastDays = Weather.forecast.forecastday.Skip(1).ToList();
            CurrentDayHours = Weather.forecast.forecastday.First().hour;
        }

        private void StartLoadAnimation()
        {
            ((dynamic)_cityWeatherPage.Resources["PageLoadedGridAnimation"]).Begin();
        }

        private async void ShowWeatherForCityAsync(ShowWeatherOfCityMessage message)
        {
            Weather = message.WeatherModel;
            IsProgressRingActive = true;
            Weather = await APIXUWeatherService.GetWeatherFromCityAsync(Weather.location.name);
            LoadPage();
        }

        private void SetMainGridVisibility(Visibility vis)
        {
            (((dynamic)_cityWeatherPage).Content).Parent.Visibility = vis;
        }
    }
}
