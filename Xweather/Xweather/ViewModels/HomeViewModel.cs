using Nito.Mvvm.CalculatedProperties; // one true <3 to  StephenCleary@github 
using System;
using System.ComponentModel;
using Xweather.Models;

namespace Xweather.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private readonly PropertyHelper Property;
        public event PropertyChangedEventHandler PropertyChanged;
        public HomeViewModel()
        {
            Property = new PropertyHelper(RaisePropertyChanged);
            wr = new WeatherRoot();
        }

        private WeatherRoot wr;
        public WeatherRoot Wr
        {
            get { return Property.Get(wr); }
            set { Property.Set(value); }
        }

        private string searchCity = "Neuchatel";
        public string SearchCity
        {
            get { return searchCity; }
            set 
            { 
                searchCity = value;
                OnPropertyChanged("searchCity");
            }
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
