using Nito.Mvvm.CalculatedProperties; // one true <3 to  StephenCleary@github 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xweather.Models;
using SkiaSharp;
using Microcharts;
using System;

namespace Xweather.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {

        private readonly PropertyHelper Property;
        public event PropertyChangedEventHandler PropertyChanged;
 

        /* CONSTRUCTOR */
        private HomeViewModel()
        {
            Property = new PropertyHelper(RaisePropertyChanged);
            wr = new WeatherRoot();
            fr = new ForecastRoot();

            initCharts();
           
            MyCharts.Add(new MyChart() { ChartData = ChartTemperature, NatureData = "Temperature" });
            MyCharts.Add(new MyChart() { ChartData = ChartPressure, NatureData = "Pression Atmo" });
            MyCharts.Add(new MyChart() { ChartData = ChartHumidity, NatureData = "Humidité" });
            MyCharts.Add(new MyChart() { ChartData = ChartWind, NatureData = "Vent" });
        }

      

        /* SINGLETON */
        private static HomeViewModel instance;
        public static HomeViewModel GetInstance()
        {
            if (instance == null)
                instance = new HomeViewModel();
            return instance;
        }

        /* GroupedDataForcast showed in forcast*/
        private List<ObservableGroupCollection<string, WeatherRoot>> groupedDataForcast;
        public List<ObservableGroupCollection<string, WeatherRoot>> GroupedDataForcast {

            get { return Property.Get(groupedDataForcast); }
            set { Property.Set(value); }
        }

        public void updateGroupedDataForcast()
        {
            if (Fr != null)
            {
                GroupedDataForcast = Fr.list.OrderBy(p => p.dt)
                   .GroupBy(p => p.GetDateDay.ToString())
                   .Select(p => new ObservableGroupCollection<string, WeatherRoot>(p)).ToList();
            }
        }

        /* Elements from openwather and bind in differents view*/
        private ForecastRoot fr;
        public ForecastRoot Fr {
            get { return Property.Get(fr); }
            set { Property.Set(value); }
        }

        private WeatherRoot wr;
        public WeatherRoot Wr {
            get { return Property.Get(wr); }
            set { Property.Set(value); }
        }

        /* USER INPUT */
        private string searchCity = "Neuchatel";
        public string SearchCity {
            get { return searchCity; }
            set {
                searchCity = value;
                OnPropertyChanged("searchCity");
            }
        }

        /* Data Binding system */
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

        /* Charts */

        /* GroupedDataChart showed in chart tab*/
        private List<ObservableGroupCollection<string, MyChart>> groupedDataChart;
        public List<ObservableGroupCollection<string, MyChart>> GroupedDataChart {

            get { return Property.Get(groupedDataChart); }
            set { Property.Set(value); }
        }

        public void updateChartAndGroupedDataChart()
        {
            updateChartData();
            if (MyCharts != null)
            {
                GroupedDataChart = MyCharts
                    .GroupBy(p => p.NatureData)
                    .Select(p => new ObservableGroupCollection<string, MyChart>(p)).ToList();
            }
        }
        private void initCharts()
        {
            ChartPressure = new LineChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                EnableYFadeOutGradient = true,
                BackgroundColor = SKColors.White,
                Entries = EntriesPressure,
            };

            ChartTemperature = new LineChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                EnableYFadeOutGradient = true,
                BackgroundColor = SKColors.White,
                Entries = EntriesTemperature,
            };

            ChartHumidity = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                //EnableYFadeOutGradient = true,
                BackgroundColor = SKColors.White,
                Entries = EntriesHumidity,
            };

            ChartWind = new BarChart() {
                Margin = 40,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesWind,
            };
        }

        private List<MyChart> myCharts = new List<MyChart>();
        public List<MyChart> MyCharts {
            get { return Property.Get(myCharts); }
            set { Property.Set(value); }
        }
        

        private List<ChartEntry> entriesPressure = new List<ChartEntry>();
        public List<ChartEntry> EntriesPressure {

            get { return Property.Get(entriesPressure); }
            set { Property.Set(value); }
        }
        private List<ChartEntry> entriesTemperature = new List<ChartEntry>();
        public List<ChartEntry> EntriesTemperature {

            get { return Property.Get(entriesTemperature); }
            set { Property.Set(value); }
        }

        private List<ChartEntry> entriesHumidity = new List<ChartEntry>();
        public List<ChartEntry> EntriesHumidity {

            get { return Property.Get(entriesHumidity); }
            set { Property.Set(value); }
        }

        private List<ChartEntry> entriesWind = new List<ChartEntry>();
        public List<ChartEntry> EntriesWind {

            get { return Property.Get(entriesWind); }
            set { Property.Set(value); }
        }

        private Chart chartPressure = null;
        public Chart ChartPressure {
            get { return Property.Get(chartPressure); }
            set { Property.Set(value); }
        }

        private Chart chartTemperature = null;
        public Chart ChartTemperature {
            get { return Property.Get(chartTemperature); }
            set { Property.Set(value); }
        }

        private Chart chartHumidity = null;
        public Chart ChartHumidity {
            get { return Property.Get(chartHumidity); }
            set { Property.Set(value); }
        }

        private Chart chartWind = null;
        public Chart ChartWind {
            get { return Property.Get(chartWind); }
            set { Property.Set(value); }
        }

        public void updateChartData()
        {
            EntriesPressure.Clear();
            EntriesTemperature.Clear();
            EntriesHumidity.Clear();
            EntriesWind.Clear();

            if (Fr != null)
            {
                Random rnd;
                var i = 0; //limite nb of value show in chart
                Fr.list.ForEach(el => {
                    rnd = new Random();
                    var skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                    i++;
   
                    if (i < 10)
                    {
                        EntriesPressure.Add(new ChartEntry((float)el.main.pressure) {
                            Label = el.GetDateHourH,
                            ValueLabel = String.Format("{0:0}hPa", el.main.pressure),
                            ValueLabelColor = skcolor,
                            Color = skcolor,
                        });

                        EntriesWind.Add(new ChartEntry((float)el.wind.speed) {
                            Label = el.GetDateHourH,
                            ValueLabel = String.Format("{0:0.0}m/s", el.wind.speed),
                            ValueLabelColor = skcolor,
                            Color = skcolor,
                        });

                    }
                
                    if (i < 20)
                    {
                        EntriesTemperature.Add(new ChartEntry((float)el.main.temp) {
                            Label = el.GetDateHourH,
                            ValueLabel = String.Format("{0:0}°", el.main.temp),
                            ValueLabelColor = skcolor,
                            Color = skcolor,
                        });

                        EntriesHumidity.Add(new ChartEntry((float)el.main.humidity) {
                            Label = el.GetDateHourH,
                            ValueLabel = String.Format("{0:0}%", el.main.humidity),
                            ValueLabelColor = skcolor,
                            Color = skcolor,
                        });
                    }
                });

            }
        }
    }

}

/* Observable class use to group by key with GroupedData */
/* source : https://www.c-sharpcorner.com/article/grouping-in-listviews-with-xamarin-forms/ */
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