using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WeatherApp.Models;
using ZS1Planv2.Model.Application;

namespace WeatherApp.ViewModels
{
    class WeatherWindowViewModel : ViewModelBase
    {
        private RelayCommand _WindowLoadedCommand;
        public RelayCommand WindowLoadedCommand =>
            _WindowLoadedCommand ?? (_WindowLoadedCommand = new RelayCommand(WindowLoaded));

        private Page _Page;
        public Page Page
        {
            get => _Page;
            set => OnPropertyChanged(ref _Page, value);
        }

        private void WindowLoaded()
        {
            MVVMMessagerService.RegisterReceiver<Page>(typeof(WeatherWindowViewModel), PageChangeRequest);
            Page = new Views.LoadingPage();
        }

        private void PageChangeRequest(Page destPage)
        {
            Page = destPage;
        }
    }
}
