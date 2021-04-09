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
    public partial class Forecast : ContentPage
    {
        HomeViewModel hvm;
        ApiRequestor ar;
        public Forecast()
        {
            InitializeComponent();
            this.Title = "FORECAST";

            ar = new ApiRequestor();
            hvm = HomeViewModel.GetInstance();
            this.BindingContext = hvm;
        }
    }
}