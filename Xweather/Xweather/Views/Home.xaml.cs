using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Xweather.ApiOpenWeather;
using Xweather.Services;
using Xweather.ViewModels;

namespace Xweather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        HomeViewModel hvm;
        ApiRequestor ar;
        /* this is a workaround : 
         * Nito.Mvvm.CalculatedProperties of StephenCleary@github
         * don't like update value through task.run and Device.BeginInvokeOnMainThread with GETTER*/
        string workaroundCity;
        public Home()
        {
            InitializeComponent();
            this.Title = "HOME";

            ar = new ApiRequestor();
            hvm = HomeViewModel.GetInstance();
            BindingContext = hvm;

            label1.IsVisible = false;
            label2.IsVisible = false;

            if (Device.RuntimePlatform == Device.UWP)
                button2.IsVisible = false;
        }

        private void OnClickSendRequest(object sender, EventArgs args)
        {
            hvm.Map.MyLocationEnabled = false;
            label1.IsVisible = false;
            label2.IsVisible = false;
            workaroundCity = hvm.SearchCity;
            Task.Run(() => updateAllData());
        }

        private void updateAllData()
        {
            var tk1 = Task.Run(() => ar.GetCurrentWeather(workaroundCity));
            var tk2 = Task.Run(() => ar.GetForecast(workaroundCity));
            tk1.Wait();
            tk2.Wait();

            /*big sweat to understand that this is the key
              to refresh UI with dataBinding during a Task.Run*/
            Device.BeginInvokeOnMainThread(() => {
                hvm.Wr = tk1.Result;
                hvm.Fr = tk2.Result;
                hvm.updateGroupedDataForcast();
                hvm.updateChartAndGroupedDataChart();
            });

            var tk3 = Task.Run(() => ar.GetWeatherAreaByCity(workaroundCity));
            tk3.Wait();
            Device.BeginInvokeOnMainThread(() => {
                hvm.Mr = tk3.Result;
                hvm.Map.MoveToRegion(new MapSpan(new Position(hvm.Mr.list[0].coord.lat, hvm.Mr.list[0].coord.lon), 1, 1));
                hvm.updateMap();
            });
        }

        private void OnClickSendRequestByGPS(object sender, EventArgs e)
        {
            hvm.Map.MyLocationEnabled = true;
            label1.IsVisible = true;
            label2.IsVisible = true;
            Task.Run(() => updateAllDataLatLon());
        }

        private async Task updateAllDataLatLon()
        {
            var loc = await Task.Run(() => GeoLoc.GetCurrentLocAsync());
            var tk1 = Task.Run(() => ar.GetWeatherAreaLatLon(loc.Latitude.ToString(), loc.Longitude.ToString()));
            var tk2 = Task.Run(() => ar.GetCurrentWeatherLatLon(loc.Latitude.ToString(), loc.Longitude.ToString()));
            var tk3 = Task.Run(() => ar.GetForecastLatLon(loc.Latitude.ToString(), loc.Longitude.ToString()));
            Device.BeginInvokeOnMainThread(() => {
                hvm.Map.MoveToRegion(new MapSpan(new Position(loc.Latitude, loc.Longitude), 1, 1));
                hvm.Location = loc;
            });

            tk1.Wait();
            Device.BeginInvokeOnMainThread(() => {
                hvm.Mr = tk1.Result;
                hvm.SearchCity = hvm.Mr.list[0].name;
                hvm.updateMap();
            });

            tk2.Wait();
            tk3.Wait();
            Device.BeginInvokeOnMainThread(() => {
                hvm.Wr = tk2.Result;
                hvm.Fr = tk3.Result;
                hvm.updateGroupedDataForcast();
                hvm.updateChartAndGroupedDataChart();
            });
        }
    }
}