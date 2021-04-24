using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using Microcharts;
using System;
using System.Linq;
using Xweather.ViewModels;
using Xweather.Models;

/// <summary>
/// Gro il reste a faire que on met tout dans HVM niveau FN. pis que on binde tout ca
/// </summary>
namespace Xweather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartView : ContentPage
    {
        HomeViewModel hvm;
        public ChartView()
        {
            InitializeComponent();
            this.Title = "CHART";

            hvm = HomeViewModel.GetInstance();
            this.BindingContext = hvm;

            //MyListview.ItemsSource = hvm.MyCharts;
        }
    }
}