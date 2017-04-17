using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WeatherApp.Models;
using WeatherApp.Models.Application;

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

        private Page _Page2;
        public Page Page2
        {
            get => _Page2;
            set => OnPropertyChanged(ref _Page2, value);
        }

        public int FramePosition;

        private void WindowLoaded()
        {
            MVVMMessagerService.RegisterReceiver<Page>(typeof(WeatherWindowViewModel), PageChangeRequest);
            Page = new Views.LoadingPage();
        }

        private void PageChangeRequest(Page destPage)
        {
            FramePosition = FramePosition == 0 ? 1 : 0;

            MVVMMessagerService.SendMessage(
                typeof(Behaviors.StackPanelFramesAnimationBehavior), FramePosition);

            if (FramePosition == 0)
            {
                Page = destPage;
            }
            else
            {
                Page2 = destPage;
            }
        }
    }
}
