using Caliburn.Micro;
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
    class CityWeatherPageViewModel : PropertyChangedBase
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
            set
            {
                _weather = value;
                NotifyOfPropertyChange(() => Weather);
            }
        }
        private CityWeatherPageView _cityWeatherPage;

        #endregion

        #region Properties

        private List<Forecastday> _forecastDays;
        public List<Forecastday> ForecastDays
        {
            get => _forecastDays;
            set
            {
                _forecastDays = value;
                NotifyOfPropertyChange(() => ForecastDays);
            }
        }

        private List<Hour> _selectedForecastDayHours;
        public List<Hour> SelectedForecastDayHours
        {
            get => _selectedForecastDayHours;
            set
            {
                _selectedForecastDayHours = value;
                NotifyOfPropertyChange(() => SelectedForecastDayHours);
            }
        }

        private List<Hour> _currentDayHours;
        public List<Hour> CurrentDayHours
        {
            get => _currentDayHours;
            set
            {
                _currentDayHours = value;
                NotifyOfPropertyChange(() => CurrentDayHours);
            }
        }

        private Forecastday _selectedForecastDay;
        public Forecastday SelectedForecastDay
        {
            get => _selectedForecastDay;
            set
            {
                _selectedForecastDay = value;
                NotifyOfPropertyChange(() => SelectedForecastDay);
                SelectedForecastDayHours = value?.hour;
            }
        }

        private bool _isProgressRingActive;
        public bool IsProgressRingActive
        {
            get => _isProgressRingActive;
            set
            {
                _isProgressRingActive = value;
                NotifyOfPropertyChange(() => IsProgressRingActive);
            }
        }
        #endregion

        public void PageLoaded(object page)
        {
            _cityWeatherPage = (dynamic)page;
            SetMainGridVisibility(Visibility.Collapsed);
        }

        private void LoadPage()
        {
            SetMainGridVisibility(Visibility.Visible);
            SetPageContent();
            StartLoadAnimation();
            IsProgressRingActive = false;
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
