using Caliburn.Micro;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WeatherApp.Models;
using WeatherApp.Models.Application;
using WeatherApp.MVVMMessages;

namespace WeatherApp.ViewModels
{
    class WeatherWindowViewModel : PropertyChangedBase
    {
        private RelayCommand _windowLoadedCommand;
        public RelayCommand WindowLoadedCommand =>
            _windowLoadedCommand ?? (_windowLoadedCommand = new RelayCommand(WindowLoaded));

        private Page _page;
        public Page Page
        {
            get => _page;
            set
            {
                _page = value;
                NotifyOfPropertyChange();
            }
        }

        private Page _page2;
        public Page Page2
        {
            get => _page2;
            set
            {
                _page2 = value;
                NotifyOfPropertyChange();
            }
        }

        public int FramePosition { get; set; }

        private void WindowLoaded()
        {
            MVVMMessagerService.RegisterReceiver<PageChangeMessage>(typeof(PageChangeMessage), PageChangeRequest);
            MVVMMessagerService.RegisterReceiver<PageChangeFinishedMessage>(typeof(PageChangeFinishedMessage), PageChangeFinished);

            LoadWeather();
        }

        private void LoadWeather()
        {
            Page = new Views.LoadingPageView();
        }

        private void PageChangeFinished(PageChangeFinishedMessage obj)
        {
            if(obj.FrameIndex == 0)
            {
                Page2 = null;
            }
            else
            {
                Page = null;
            }
        }

        private void PageChangeRequest(PageChangeMessage message)
        {
            Page destPage = message.PageToChange;
            FramePosition = FramePosition == 0 ? 1 : 0;

            MVVMMessagerService.SendMessage(
                typeof(AnimatePageChangingMessage), new AnimatePageChangingMessage()
                {
                    FrameToAnimate = FramePosition
                });

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
