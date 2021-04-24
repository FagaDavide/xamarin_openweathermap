using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xweather.ViewModels;

namespace Xweather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Forecast : ContentPage
    {
        HomeViewModel hvm;
        public Forecast()
        {
            InitializeComponent();
            this.Title = "FORECAST";

            hvm = HomeViewModel.GetInstance();
            this.BindingContext = hvm;
        }
    }
}