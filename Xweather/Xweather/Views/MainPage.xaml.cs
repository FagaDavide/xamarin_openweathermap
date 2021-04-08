using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xweather.Views;


namespace Xweather
{
    public partial class MainPage : TabbedPage
    {
    

        public MainPage()
        {
            this.Children.Add(new Home());
            this.Children.Add(new Map());
            this.Children.Add(new Pollution());
            InitializeComponent();
        }
    }
}
