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
    public partial class Home : ContentPage
    {
        HomeViewModel hvm;
        ApiRequestor ar; 
        public Home()
        {
            InitializeComponent();
            this.Title = "HOME";

            ar = new ApiRequestor();
            hvm = HomeViewModel.GetInstance();
            this.BindingContext = hvm;
        }

        private void OnClickSendRequest(object sender, EventArgs args)
        {
            var tk1 = Task.Run(() => ar.GetCurrentWeather(hvm.SearchCity));
            var tk2 = Task.Run(() => ar.GetForecast(hvm.SearchCity));
           
            tk1.Wait();
            tk2.Wait();
      
            hvm.Wr = tk1.Result;
            hvm.Fr = tk2.Result;

            var lat = hvm.Wr.coord.lat;
            var lon = hvm.Wr.coord.lon;

            var tk3 = Task.Run(() => ar.GetForecastLatLon(lat.ToString(), lon.ToString()));

            hvm.updateGroupedDataForcast();
            hvm.updateChartAndGroupedDataChart();
         
            tk3.Wait();
            hvm.Mr = tk3.Result;

            hvm.Map.MoveToRegion(new MapSpan(new Position(lat, lon), 1, 1));
            hvm.updateMap();
        }

        private void OnClickGpsRequest(object sender, EventArgs e)
        {
        

        }
    }
}