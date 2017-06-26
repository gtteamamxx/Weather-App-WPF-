using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherApp.Models;
using WeatherApp.Models.Factory;
using WeatherApp.Properties;
using WeatherApp.Models.Application;
using WeatherApp.MVVMMessages;
using Caliburn.Micro;

namespace WeatherApp.ViewModels
{
    class LoadingPageViewModel : PropertyChangedBase
    {
        #region Commands
        private RelayCommand _pageLoadedCommand;
        public RelayCommand PageLoadedCommand =>
            _pageLoadedCommand ?? (_pageLoadedCommand = new RelayCommand(async () => await PageLoadedAsync()));

        #endregion

        #region Properties

        private double _loadingValue;
        public double LoadingValue
        {
            get => _loadingValue;
            set
            {
                _loadingValue = value;
                NotifyOfPropertyChange(() => LoadingValue);
            }
        }

        #endregion
        private async Task PageLoadedAsync()
        {
            (bool, int) resultState = await new ActionFactory()
                .ContinueWith(async () => !await IsInternetConnectionAvailableAsync())
                .ContinueWith(async () => !await IsApiConnectionAvailableAsync())
                .ProgressChanged(ChangeLoadingValue)
                .StartAsync().ConfigureAwait(false);

            if (resultState.Item1)
            {
                MessageBox.Show("Wystąpił problem podczas próby łączenia z internetem", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                await Task.Delay(500);
                CloseApplication();
                return;
            }

            await Task.Delay(300);
            App.Current.Dispatcher.Invoke(() => ShowWeatherWindow(selectCity: Settings.Default.FirstRun));
        }

        private void ChangeLoadingValue(double percent)
        {
            App.Current.Dispatcher.Invoke(() => LoadingValue = percent);
        }

        private static void CloseApplication()
        {
            App.Current.Dispatcher.Invoke(() => App.Current.MainWindow.Close());
        }

        private static void ShowWeatherWindow(bool selectCity = false)
        {
            dynamic pageToShow = selectCity ? (object)new Views.SearchCityPageView() : new Views.CityWeatherPageView();

            MVVMMessagerService.SendMessage(typeof(PageChangeMessage), new PageChangeMessage()
            {
                PageToChange = pageToShow
            });
        }

        private async static Task<bool> IsApiConnectionAvailableAsync()
        {
            return await Models.Internet.Network.IsApiAvailable();
        }

        private async static Task<bool> IsInternetConnectionAvailableAsync()
        {
            return await Models.Internet.Network.IsInternetAvailableAsync();
        }
    }
}
