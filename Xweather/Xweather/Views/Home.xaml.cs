using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Home()
        {
            InitializeComponent();
            this.Title = "HOME";

            hvm = new HomeViewModel();
            this.BindingContext = hvm;
        }

        private void OnClickSendRequest(object sender, EventArgs args)
        {
            var ar = new ApiRequestor();
            var tk = Task.Run(() => ar.GetCurrentWeather(hvm.SearchCity));
            tk.Wait();
            hvm.Wr = tk.Result;
        }

        async void OnClickGpsRequest(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Pas encore fait", "OK");
        }
    }
}