using APIXULib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Models.Internet;

namespace WeatherApp.ViewModels
{
    class SearchCityPageViewModel : ViewModelBase
    {
        public SearchCityPageViewModel()
        {
            CityTips = new ObservableCollection<Location>();
        }

        private bool _IsTipSelected = false;
        #region Properties
        private bool _IsCityTipOpen;
        public bool IsCityTipOpen
        {
            get => _IsCityTipOpen;
            set
            {
                if (CityTips.Count() > 0)
                    return;
                OnPropertyChanged(ref _IsCityTipOpen, value);
            }
        }

        private ObservableCollection<Location> _CityTips;
        public ObservableCollection<Location> CityTips
        {
            get => _CityTips;
            set => OnPropertyChanged(ref _CityTips, value);
        }

        private string _SearchCityName;
        public string SearchCityName
        {
            get => _SearchCityName;
            set
            {
                if (_SearchCityName == value)
                    return;
                _SearchCityName = value;
                SearchTextChanged(value);
                OnPropertyChanged(ref _SearchCityName, value);
            }
        }

        private Location _SelectedTipCity;
        public Location SelectedTipCity
        {
            get => _SelectedTipCity;
            set
            {
                if(value != null)
                {
                    _IsTipSelected = true;
                    SearchCityName = value.name;
                }
                OnPropertyChanged(ref _SelectedTipCity, value);
            }
        }
        #endregion

        private void SearchTextChanged(string searchText)
        {
            CityTips.Clear();

            if (_IsTipSelected ||  string.IsNullOrEmpty(searchText))
            {
                if (_IsTipSelected)
                    _IsTipSelected = false;

                IsCityTipOpen = false;
                return;
            }

            IsCityTipOpen = true;

            List<Location> suggestedLocations = Task.Run(() => APIXUWeatherService.GetAutoCompleteCityNamesAsync(searchText)).Result;

            if(suggestedLocations.Count() == 0)
            {
                IsCityTipOpen = false;
                return;
            }

            foreach (Location suggestedLocation in suggestedLocations)
            {
                CityTips.Add(suggestedLocation);
            }
        }

    }
}
