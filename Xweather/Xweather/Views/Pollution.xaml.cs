using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xweather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pollution : ContentPage
    {
        public Pollution()
        {
            InitializeComponent();
            this.Title = "Pollution";
        }
    }
}