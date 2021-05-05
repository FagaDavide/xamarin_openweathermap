using System;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xweather.Views;


namespace Xweather
{
    public partial class MainPage : TabbedPage
    {
        private Home home = new Home();
        public MainPage()
        {
            BarBackgroundColor = Color.DodgerBlue;
            BarTextColor = Color.White;

            Children.Add(home);
            Children.Add(new Forecast());
            Children.Add(new ChartView());
            if(Device.RuntimePlatform == Device.Android)
                initPermission();

            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                DisplayAlert("-=[ INFO ]=-", "Internet connectivity not found", "OK");

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
                        await DisplayAlert("-=[ INFO ]=-", "Xweather needs access to GPS, please allow location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    //Allowed
                    Children.Add(new MapView());
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //deny
                    home.btnGPS.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
