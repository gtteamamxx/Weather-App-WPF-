using Caliburn.Micro;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Models.Application;
using WeatherApp.Models.Internet;
using WeatherApp.Models.Weather;
using WeatherApp.MVVMMessages;

namespace WeatherApp.ViewModels
{
    public class SearchCityPageViewModel : PropertyChangedBase
    {
        public SearchCityPageViewModel()
        {
            CityTips = new ObservableCollection<Location>();
        }
        private bool _isTipSelected;

        #region Properties
        private bool _isCitiesTipOpen;
        public bool IsCitiesTipOpen
        {
            get => _isCitiesTipOpen;
            set
            {
                _isCitiesTipOpen = value;
                NotifyOfPropertyChange();
            }
        }

        private ObservableCollection<Location> _cityTips;
        public ObservableCollection<Location> CityTips
        {
            get => _cityTips;
            set
            {
                _cityTips = value;
                NotifyOfPropertyChange(() => CityTips);
            }
        }

        private string _searchCityName = "(Nazwa miasta, Kraj)";
        public string SearchCityName
        {
            get => _searchCityName;
            set
            {
                if (_searchCityName == value)
                    return;

                _searchCityName = value;
                _temporarySelectedCity = null;

                SearchTextChangedAsync(value);
                _searchCityName = value;
                NotifyOfPropertyChange();
            }
        }

        private Location _selectedTipCity;
        public Location SelectedTipCity
        {
            get => _selectedTipCity;
            set
            {
                if(value != null)
                {
                    _isTipSelected = true;

                    SearchCityName = value.name;
                    _selectedTipCity = _temporarySelectedCity = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private bool _isShowWeatherButtonVisible;
        public bool IsShowWeatherButtonVisible
        {
            get => _isShowWeatherButtonVisible;
            set
            {
                _isShowWeatherButtonVisible = value;
                NotifyOfPropertyChange();
            }
        }

        private Location __temporarySelectedCity;
        private Location _temporarySelectedCity
        {
            get => __temporarySelectedCity;
            set
            {
                __temporarySelectedCity = value;
                IsShowWeatherButtonVisible = value != null;
            }
        }

        #endregion

        public void ShowWeatherButton_Click()
        {
            MVVMMessagerService.SendMessage(typeof(PageChangeMessage), new PageChangeMessage()
            {
                PageToChange = new Views.CityWeatherPageView()
            });
            MVVMMessagerService.SendMessage(typeof(ShowWeatherOfCityMessage), new ShowWeatherOfCityMessage()
            {
                WeatherModel = new WeatherModel()
                {
                    location = _temporarySelectedCity
                }
            });
        }

        private async void SearchTextChangedAsync(string searchText)
        {
            if (!IsAbleToDownloadNewSugesstionList(searchText))
                return;

            if (!IsCitiesTipOpen)
                IsCitiesTipOpen = true;

            List<Location> suggestedLocations = await APIXUWeatherService.GetAutoCompleteCityNamesAsync(searchText);
            CityTips.Clear();

            if (suggestedLocations.Count() == 0)
            {
                IsCitiesTipOpen = false;
                return;
            }

            foreach (Location suggestedLocation in suggestedLocations)
            {
                CityTips.Add(suggestedLocation);
            }
        }

        private bool IsAbleToDownloadNewSugesstionList(string searchText)
        {
            if (_isTipSelected || string.IsNullOrEmpty(searchText))
            {
                if (_isTipSelected)
                    _isTipSelected = false;

                IsCitiesTipOpen = false;
                CityTips.Clear();
                return false;
            }
            return true;
        }
    }
}
