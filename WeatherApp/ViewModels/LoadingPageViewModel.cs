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
using ZS1Planv2.Model.Application;

namespace WeatherApp.ViewModels
{
    class LoadingPageViewModel : ViewModelBase
    {
        #region Commands
        private RelayCommand _PageLoadedCommand;
        public RelayCommand PageLoadedCommand =>
            _PageLoadedCommand ?? (_PageLoadedCommand = new RelayCommand(async () => await PageLoadedAsync()));

        #endregion

        #region Properties

        private double _LoadingValue;
        public double LoadingValue
        {
            get => _LoadingValue;
            set => OnPropertyChanged(ref _LoadingValue, value);
        }

        #endregion
        private async Task PageLoadedAsync()
        {
            (bool, int) resultState = await new ActionFactory()
                .ContinueWith(async () => await CheckInternetConnectionAsync())
                .ContinueWith(async () => await CheckApiConnectionAsync())
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
            MVVMMessagerService.SendMessage(typeof(WeatherWindowViewModel), new Views.LoadingPage());
        }

        private async static Task<bool> CheckApiConnectionAsync()
        {
            return !await Models.Internet.Network.IsApiAvailable();
        }

        private async static Task<bool> CheckInternetConnectionAsync()
        {
            return !await Models.Internet.Network.IsInternetAvailableAsync();
        }
    }
}
