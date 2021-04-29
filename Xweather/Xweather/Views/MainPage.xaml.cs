using Xamarin.Forms;
using Xweather.Views;


namespace Xweather
{
    public partial class MainPage : TabbedPage
    {
    

        public MainPage()
        {
            this.BarBackgroundColor = Color.DodgerBlue;
            this.BarTextColor = Color.White;
            this.Children.Add(new Home());
            this.Children.Add(new Forecast());
            this.Children.Add(new ChartView());
           // if(Device.RuntimePlatform == Device.Android)
                this.Children.Add(new MapView());
            this.Children.Add(new Pollution());
            InitializeComponent();
        }
    }
}
