using Nito.Mvvm.CalculatedProperties; // one true <3 to  StephenCleary@github 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xweather.Models;

namespace Xweather.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private static HomeViewModel instance;
        private readonly PropertyHelper Property;
        public event PropertyChangedEventHandler PropertyChanged;
        public IList<WeatherRoot> Items { get; private set; }
       

        private HomeViewModel()
        {
            Property = new PropertyHelper(RaisePropertyChanged);
            wr = new WeatherRoot();
            fr = new ForecastRoot();

           
        }


        private List<ObservableGroupCollection<string, WeatherRoot>> groupedData;
        public List<ObservableGroupCollection<string, WeatherRoot>> GroupedData {

            get { return Property.Get(groupedData); }
            set { Property.Set(value); }
        }
    
         
        public void updateGroupedDataForcast()
        {
            GroupedData = Fr.list.OrderBy(p => p.dt)
               .GroupBy(p => p.GetDateDay.ToString())
               .Select(p => new ObservableGroupCollection<string, WeatherRoot>(p)).ToList();
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

    public class ObservableGroupCollection<S, T> : ObservableCollection<T>
    {
        private readonly S _key;
        public ObservableGroupCollection(IGrouping<S, T> group)
            : base(group)
        {
            _key = group.Key;
        }
        public S Key {
            get { return _key; }
        }
    }

}
