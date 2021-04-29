using Android.Graphics;
using Java.Net;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;
using AndroidBitmapDescriptorFactory = Android.Gms.Maps.Model.BitmapDescriptorFactory;

// source : https://nerd-corner.com/how-to-add-custom-icon-pins-to-google-maps-xamarin-app/
namespace Xweather.Droid
{

    public sealed class BitmapConfig : IBitmapDescriptorFactory
    {
        public AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor)
        {
            int iconId = 0;
            switch (descriptor.Id)
            {
                case "castle":
                    //iconId = Resource.Drawable.castle;
                    break;
                case "beer":
                    //iconId = Resource.Drawable.beer;
                    break;
            }
            var url = new URL("https://s3-eu-west-1.amazonaws.com/mediapool.starticket.ch/wwwroot/ticketing/img/events/667x375_konzertfabrik_z7_190721_1600x900.jpg");
            Bitmap bmp = BitmapFactory.DecodeStream(url.OpenConnection().InputStream);
            return AndroidBitmapDescriptorFactory.FromBitmap(bmp);// FromAsset("MrG.bmp"); //FromPath("https://people.he-arc.ch/photos/GrunenwaldDavid.jpg");//.FromResource(iconId);
        }
    }
}