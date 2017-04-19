using APIXULib;
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

namespace WeatherApp.ViewModels
{
    public class SearchCityPageViewModel : ViewModelBase
    {
        public SearchCityPageViewModel()
        {
            CityTips = new ObservableCollection<Location>();
        }

        #region Commands

        private RelayCommand _showWeatherButtonCommand;
        public RelayCommand ShowWeatherButtonCommand =>
            _showWeatherButtonCommand ?? (_showWeatherButtonCommand = new RelayCommand(() => ShowWeatherButton_Click()));

        private RelayCommand _searchCityNameTextBoxFocusLostCommand;
        public RelayCommand SearchCityNameTextBoxFocusLostCommand =>
            _searchCityNameTextBoxFocusLostCommand ?? (_searchCityNameTextBoxFocusLostCommand = new RelayCommand(() => SearchTextBox_FocusLost()));

        #endregion

        private bool _isTipSelected;

        #region Properties
        private bool _isCityTipOpen;
        public bool IsCitiesTipOpen
        {
            get => _isCityTipOpen;
            set
            {
                if (CityTips.Count() > 0)
                    return;
                OnPropertyChanged(ref _isCityTipOpen, value);
            }
        }

        private ObservableCollection<Location> _cityTips;
        public ObservableCollection<Location> CityTips
        {
            get => _cityTips;
            set => OnPropertyChanged(ref _cityTips, value);
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
                SearchTextChangedAsync(value);
                OnPropertyChanged(ref _searchCityName, value);
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
                    _selectedTipCity = value;
                    OnPropertyChanged(ref _selectedTipCity, value);
                }
            }
        }
        #endregion

        private void ShowWeatherButton_Click()
        {
            MVVMMessagerService.SendMessage(typeof(WeatherWindowViewModel), new Views.CityWeatherPage());
            MVVMMessagerService.SendMessage(typeof(CityWeatherPageViewModel), SelectedTipCity);
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

        private void SearchTextBox_FocusLost()
        {
            IsCitiesTipOpen = false;
        }
    }
}
