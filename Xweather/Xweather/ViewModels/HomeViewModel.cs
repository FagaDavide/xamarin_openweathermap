using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xweather.Models;
using SkiaSharp;
using Microcharts;
using System;
using Xamarin.Forms.GoogleMaps; //isn't the real googleMaps like you think, is xamarin.forms.maps improved
using Xamarin.Forms;
using System.Net;
using System.IO;

namespace Xweather.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        /* CONSTRUCTOR */
        private HomeViewModel()
        {
            wr = new WeatherRoot(); //current weather
            fr = new ForecastRoot(); // list of weather with same city different date to show futur prevision
            mr = new ForecastRoot(); // list of weather with same date differents cities to show in map current position and neiboors cities
            ar = new ForcastAirPollutionRoot();

            initMap();
            initCharts();
            AddCharts();
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
                GroupedDataForcast = Fr.list
                   .OrderBy(p => p.dt)
                   .GroupBy(p => p.GetDateDay.ToString())
                   .Select(p => new ObservableGroupCollection<string, WeatherRoot>(p)).ToList();
            }
        }

        /* Elements root from openwather and bind in differents view*/
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

        private ForcastAirPollutionRoot ar;
        public ForcastAirPollutionRoot Ar {
            get { return Property.Get(ar); }
            set { Property.Set(value); }
        }

        private ForecastRoot mr;
        public ForecastRoot Mr {
            get { return Property.Get(mr); }
            set { Property.Set(value); }
        }

        /* USER INPUT */
        private string searchCity = "Neuchâtel";
        public string SearchCity {
            get { return Property.Get(searchCity); }
            set { Property.Set(value); }
        }


        /* MAP System */
        private Map map;
        public Map Map {
            get { return Property.Get(map); }
            set { Property.Set(value); }
        }

        private void initMap()
        {
            Position CortaillodPosition = new Position(46.9333, 6.85);
            MapSpan mapSpan = new MapSpan(CortaillodPosition, 1, 1);
            Map = new Map();
            Map.MoveToRegion(mapSpan, true);
            Map.MapType = MapType.Satellite;

            /* two choices  
             * or button +- on map with small icon
             * or no button +- with big icon 
             Why? Scale is a zoom on the map object not zoom on the map, :-/ 
             * also the button disappear. in simulator you can make double click or ctrl double click ton zoom*/

            //no button+- big icon, comment the next line if you want the button +-
            Map.Scale = 2;
        }

        public void updateMap()
        {
            var webClient = new WebClient();
            byte[] imageBytes;
            Stream stream;
            Image image = new Image();
            Map.Pins.Clear();

            if (Device.RuntimePlatform == Device.Android)
            {
                // clean some data useless
                var rejectedWords = new List<string> { "Canton", "District"};
                var rejectList = new List<WeatherRoot>();
                rejectedWords.ForEach(wd => {
                    rejectList.AddRange(Mr.list.Where(i => i.name.Contains(wd)).ToList());
                });
                var cleanDistritAndCanton = Mr.list.Except(rejectList);
                var distinctItems = cleanDistritAndCanton.GroupBy(x => x.name).Select(y => y.First()).ToList();
                
                distinctItems.ForEach(el => {
                    imageBytes = webClient.DownloadData(el.weather[0].GetIconBig);
                    stream = new MemoryStream(imageBytes);

                    var imgSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    image.Source = imgSource;

                    var pin = new Pin() {
                        Position = new Position(el.coord.lat, el.coord.lon),
                        Label = el.name + " " + String.Format("{0:#,0}°", el.main.temp),
                        Icon = BitmapDescriptorFactory.FromStream(stream),
                    };
                    Map.Pins.Add(pin);
                });
            }
        }

        /* GeoLoc System */
        private Xamarin.Essentials.Location location;
        public Xamarin.Essentials.Location Location {
            get { return Property.Get(location); }
            set { Property.Set(value); }
        }
      
        
        /* Charts System */
        private List<MyChart> myCharts = new List<MyChart>();
        public List<MyChart> MyCharts {
            get { return Property.Get(myCharts); }
            set { Property.Set(value); }
        }

        private Chart ChartPressure { get; set; } = null;
        private Chart ChartTemperature { get; set; } = null;
        private Chart ChartHumidity { get; set; } = null;
        private Chart ChartWind { get; set; } = null;
        private Chart ChartAirCurrentGeneral { get; set; } = null;
        private Chart ChartAirCO { get; set; } = null;
        private Chart ChartAirNO2 { get; set; } = null;
        private Chart ChartAirO3 { get; set; } = null;
        private Chart ChartAirSO2 { get; set; } = null;
        private Chart ChartAirPM2_5 { get; set; } = null;
        private Chart ChartAirPM10 { get; set; } = null;
        private Chart ChartAirNH3 { get; set; } = null;


        public List<ChartEntry> EntriesPressure { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesTemperature { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesHumidity { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesWind { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirCO { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirNO2 { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirO3 { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirSO2 { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirPM2_5 { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirPM10 { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirNH3 { get; set; } = new List<ChartEntry>();
        public List<ChartEntry> EntriesAirCurrentGeneral { get; set; } = new List<ChartEntry>();


        /* GroupedDataChart showed in chart tab*/
        private List<ObservableGroupCollection<string, MyChart>> groupedDataChart;
        public List<ObservableGroupCollection<string, MyChart>> GroupedDataChart {
            get { return Property.Get(groupedDataChart); }
            set { Property.Set(value); }
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
                IsAnimated = false,
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

            ChartAirCurrentGeneral = new RadarChart() {
                Margin = 40,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirCurrentGeneral,
            };

            ChartAirCO = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirCO,
            };

            ChartAirSO2 = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirSO2,
            };

            ChartAirNO2 = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirNO2,
            };

            ChartAirO3 = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirO3,
            };

            ChartAirNH3 = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirNH3,
            };

            ChartAirPM2_5 = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirPM2_5,
            };

            ChartAirPM10 = new PointChart() {
                Margin = 20,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(0, 0, 20),
                LabelTextSize = 60f,
                LabelColor = SKColors.DodgerBlue,
                BackgroundColor = SKColors.White,
                Entries = EntriesAirPM10,
            };
        }
        private void AddCharts()
        {
            MyCharts.Add(new MyChart() { ChartData = ChartTemperature, NatureData = "Température" });
            MyCharts.Add(new MyChart() { ChartData = ChartHumidity, NatureData = "Humidité" });
            MyCharts.Add(new MyChart() { ChartData = ChartWind, NatureData = "Vent" });
            MyCharts.Add(new MyChart() { ChartData = ChartPressure, NatureData = "Pression Atmo" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirCurrentGeneral, NatureData = "Pollution Actuelle" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirCO, NatureData = "Monoxyde de carbone" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirNO2, NatureData = "Dioxyde d'azote" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirO3, NatureData = "Ozone" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirSO2, NatureData = "Dioxyde de soufre" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirNH3, NatureData = "Ammoniac" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirPM10, NatureData = "Particule fine (PM10)" });
            MyCharts.Add(new MyChart() { ChartData = ChartAirPM2_5, NatureData = "Particule super fine (PM2.5)" });
        }

        public void updateChartData()
        {
            EntriesPressure.Clear();
            EntriesTemperature.Clear();
            EntriesHumidity.Clear();
            EntriesWind.Clear();
            EntriesAirCurrentGeneral.Clear();
            EntriesAirCO.Clear();
            EntriesAirSO2.Clear();
            EntriesAirNO2.Clear();
            EntriesAirNH3.Clear();
            EntriesAirO3.Clear();
            EntriesAirPM2_5.Clear();
            EntriesAirPM10.Clear();

            if (Ar != null)
            {
                var rnd = new Random();
                var skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                var currentAr = Ar.GetCurrentPollution;

                EntriesAirCurrentGeneral.Add(new ChartEntry((float)(currentAr.components.co/1000.0)) {
                    Label = "CO",
                    ValueLabel = String.Format("{0:0.0}mg/m3", (currentAr.components.co / 1000.0)),
                    ValueLabelColor = skcolor,
                    Color = skcolor,
                });

                skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                EntriesAirCurrentGeneral.Add(new ChartEntry((float)currentAr.components.no2) {
                    Label = "NO2",
                    ValueLabel = String.Format("{0:0.0}μg/m3", currentAr.components.no2),
                    ValueLabelColor = skcolor,
                    Color = skcolor,
                });

                skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                EntriesAirCurrentGeneral.Add(new ChartEntry((float)(currentAr.components.o3/1000.0)) {
                    Label = "O3",
                    ValueLabel = String.Format("{0:0.0}mg/m3", (currentAr.components.o3 / 1000.0)),
                    ValueLabelColor = skcolor,
                    Color = skcolor,
                });

                skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                EntriesAirCurrentGeneral.Add(new ChartEntry((float)currentAr.components.so2) {
                    Label = "SO2",
                    ValueLabel = String.Format("{0:0.0}μg/m3", currentAr.components.so2),
                    ValueLabelColor = skcolor,
                    Color = skcolor,
                });

                skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                EntriesAirCurrentGeneral.Add(new ChartEntry((float)currentAr.components.pm2_5) {
                    Label = "PM2.5",
                    ValueLabel = String.Format("{0:0.0}μg/m3", currentAr.components.pm2_5),
                    ValueLabelColor = skcolor,
                    Color = skcolor,
                });

                skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                EntriesAirCurrentGeneral.Add(new ChartEntry((float)currentAr.components.pm10) {
                    Label = "PM10",
                    ValueLabel = String.Format("{0:0.0}μg/m3", currentAr.components.pm10),
                    ValueLabelColor = skcolor,
                    Color = skcolor,
                });

                skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                EntriesAirCurrentGeneral.Add(new ChartEntry((float)currentAr.components.nh3) {
                    Label = "NH3",
                    ValueLabel = String.Format("{0:0.0}μg/m3", currentAr.components.nh3),
                    ValueLabelColor = skcolor,
                    Color = skcolor,
                });
              
                var i = 0; //limie nb of value show in chart
                var datenow = DateTimeOffset.Now.ToUnixTimeSeconds(); //i don't know why, but forecast pollution also gives some data in the past

                Ar.list.ForEach(el => {
                    rnd = new Random();
                    skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                   

                    if (el.dt >= datenow)
                    {
                        i++;
                        if (i > 28)
                            return;

                        if (i % 3 == 0)
                        {
                            EntriesAirCO.Add(new ChartEntry((float)el.components.co) {
                                Label = el.GetDateHourH,
                                ValueLabel = String.Format("{0:0}μg/m3", el.components.co),
                                ValueLabelColor = skcolor,
                                Color = skcolor,
                            });

                            EntriesAirNO2.Add(new ChartEntry((float)el.components.no2) {
                                Label = el.GetDateHourH,
                                ValueLabel = String.Format("{0:0}μg/m3", el.components.no2),
                                ValueLabelColor = skcolor,
                                Color = skcolor,
                            });

                            EntriesAirNH3.Add(new ChartEntry((float)el.components.nh3) {
                                Label = el.GetDateHourH,
                                ValueLabel = String.Format("{0:0}μg/m3", el.components.nh3),
                                ValueLabelColor = skcolor,
                                Color = skcolor,
                            });

                            EntriesAirO3.Add(new ChartEntry((float)el.components.o3) {
                                Label = el.GetDateHourH,
                                ValueLabel = String.Format("{0:0}μg/m3", el.components.o3),
                                ValueLabelColor = skcolor,
                                Color = skcolor,
                            });

                            EntriesAirSO2.Add(new ChartEntry((float)el.components.so2) {
                                Label = el.GetDateHourH,
                                ValueLabel = String.Format("{0:0}μg/m3", el.components.so2),
                                ValueLabelColor = skcolor,
                                Color = skcolor,
                            });

                            EntriesAirPM2_5.Add(new ChartEntry((float)el.components.pm2_5) {
                                Label = el.GetDateHourH,
                                ValueLabel = String.Format("{0:0}μg/m3", el.components.pm2_5),
                                ValueLabelColor = skcolor,
                                Color = skcolor,
                            });

                            EntriesAirPM10.Add(new ChartEntry((float)el.components.pm10) {
                                Label = el.GetDateHourH,
                                ValueLabel = String.Format("{0:0}μg/m3", el.components.pm10),
                                ValueLabelColor = skcolor,
                                Color = skcolor,
                            });
                        }
                    }
                });
            }
            
            if (Fr != null)
            {
                Random rnd;
                var i = 0; //limie nb of value show in chart
                Fr.list.ForEach(el => {
                    rnd = new Random();
                    var skcolor = SKColor.Parse(String.Format("#{0:X6}", rnd.Next(0x1000000)));
                    i++;

                    if (i <= 9)
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

                    if (i <= 17)
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