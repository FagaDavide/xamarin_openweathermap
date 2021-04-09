using Nito.Mvvm.CalculatedProperties; // one true <3 to  StephenCleary@github 
using System.ComponentModel;
using Xamarin.Forms;
using Xweather.Models;

namespace Xweather.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private static HomeViewModel instance;
        private readonly PropertyHelper Property;
        public event PropertyChangedEventHandler PropertyChanged;

        private HomeViewModel()
        {
            Property = new PropertyHelper(RaisePropertyChanged);
            wr = new WeatherRoot();
            fr = new ForecastRoot();
        }

        public static HomeViewModel GetInstance()
        {
            if (instance == null)
                instance = new HomeViewModel();
            return instance;
        }

        private ForecastRoot fr;
        public ForecastRoot Fr {
            get { return Property.Get(fr); }
            set { Property.Set(value); }
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

        private Color selectedBackgroundColor;
        public Color SelectedBackgroundColor {
            set 
            {
                if (selectedBackgroundColor != value)
                {
                    selectedBackgroundColor = value;
                    OnPropertyChanged("selectedBackgroundColor");
                }
            }
            get 
            {
                return selectedBackgroundColor;
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
