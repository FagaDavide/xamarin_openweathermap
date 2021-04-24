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
            this.Children.Add(new Map());
            this.Children.Add(new Pollution());
            InitializeComponent();
        }
    }
}
