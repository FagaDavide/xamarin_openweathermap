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
            var tk = Task.Run(() => ar.GetCurrentWeather(hvm.SearchCity));
            tk.Wait();
            hvm.Wr = tk.Result;
        }

        private void OnClickGpsRequest(object sender, EventArgs e)
        {
            var tk = Task.Run(() => ar.GetForecast());
            tk.Wait();
            hvm.Fr = tk.Result;
        }
    }
}