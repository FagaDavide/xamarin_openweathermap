using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Xweather.ApiOpenWeather;
using Xweather.ViewModels;

namespace Xweather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentPage
    {
        private HomeViewModel hvm;
        private ApiRequestor ar;

        public MapView()
        {
            ar = new ApiRequestor();
            hvm = HomeViewModel.GetInstance();
            BindingContext = hvm;

            InitializeComponent();

            hvm.Map.MapClicked += (object sender, MapClickedEventArgs e) => {
                var lat = e.Point.Latitude.ToString();
                var lon = e.Point.Longitude.ToString();
                var tk3 = Task.Run(() => ar.GetForecastLatLon(lat, lon));

                tk3.Wait();
                hvm.Mr = tk3.Result;

                hvm.updateMap();
            };

            Content = hvm.Map;
            Title = "MAP";
        }
    }
}