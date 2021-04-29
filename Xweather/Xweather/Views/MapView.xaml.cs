using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xweather.ViewModels;

namespace Xweather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentPage
    {
        private HomeViewModel hvm;
     
        public MapView()
        {
            hvm = HomeViewModel.GetInstance();
            BindingContext = hvm;

            InitializeComponent();

            Content = hvm.Map;
            Title = "MAP";

       
        }
    }
}