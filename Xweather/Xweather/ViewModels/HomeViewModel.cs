using System;
using System.Collections.Generic;
using System.Text;
using Xweather.Models;

namespace Xweather.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        private CurrentWeather cw;
        public HomeViewModel()
        {
            cw = new CurrentWeather
            {
                name = "ville TOto",
                weatherInfoTmp = string.Empty,
            };
        }

        public string name
        {
            get { return cw.name; }
            set
            {
                cw.name = value;
                OnPropertyChanged("name");
            }
        }
        public string weatherInfoTmp
        {
            get { return cw.weatherInfoTmp; }
            set
            {
                cw.weatherInfoTmp = value;
                OnPropertyChanged("weatherInfoTmp");
            }
        }
    }
}
