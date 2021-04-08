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

        private void OnClick(object sender, EventArgs args)
        {
            var ar = new ApiRequestor();
            var t = Task.Run(() => ar.getCurrentWeather());
            t.Wait();
            System.Diagnostics.Debug.WriteLine(t.Result);

            //idée c'est de faire un json et de le mettre dans currentWeather

            hvm.weatherInfoTmp = t.Result;


        }
    }
}