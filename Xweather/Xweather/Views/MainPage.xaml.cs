using System;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xweather.Views;


namespace Xweather
{
    public partial class MainPage : TabbedPage
    {
        private bool isPermissionAllowed;
        public MainPage()
        {
            BarBackgroundColor = Color.DodgerBlue;
            BarTextColor = Color.White;
            Children.Add(new Home());
            Children.Add(new Forecast());
            Children.Add(new ChartView());
            if(Device.RuntimePlatform == Device.Android)
            {
                initPermission();
                if(isPermissionAllowed)
                    Children.Add(new MapView());
            }
            Children.Add(new Pollution());
            InitializeComponent();
        }

        /* source : https://forums.xamarin.com/discussion/168778/need-a-fix-on-current-location-access-on-xamarin-forms
           thank you ColeX */
        private async void initPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    isPermissionAllowed = true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    isPermissionAllowed = false;
                }
            }
            catch (Exception ex)
            {
                isPermissionAllowed = false;
            }
        }
    }
}
