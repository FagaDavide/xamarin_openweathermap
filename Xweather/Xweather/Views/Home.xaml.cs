using System;
using System.Threading.Tasks;

using Xamarin.Forms;
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
            var lat = 46.9333;
            var lon = 6.85;
            var tk1 = Task.Run(() => ar.GetCurrentWeather(hvm.SearchCity));
            var tk2 = Task.Run(() => ar.GetForecast(hvm.SearchCity));
            var tk3 = Task.Run(() => ar.GetForecastLatLon(lat.ToString(), lon.ToString()));

            tk1.Wait();
            tk2.Wait();
            tk3.Wait();

            hvm.Wr = tk1.Result;
            hvm.Fr = tk2.Result;
            hvm.Mr = tk3.Result;

            hvm.updateGroupedDataForcast();
            hvm.updateChartAndGroupedDataChart();
            hvm.updateMap();

        }

        private void OnClickGpsRequest(object sender, EventArgs e)
        {
        

        }
    }
}