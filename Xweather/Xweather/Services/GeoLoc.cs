using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Xweather.Services
{
    class GeoLoc
    { 
        public static async Task<Location> GetCurrentLocAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                    return location;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GeoLoc - " + ex);
            }
            return null;
        }
    }
}
