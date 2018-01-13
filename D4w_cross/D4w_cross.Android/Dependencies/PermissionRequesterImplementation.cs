using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using D4w_cross.Helpers;
using Android.Locations;
using Xamarin.Forms;
using D4w_cross.Droid.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(PermissionRequesterImplementation))]
namespace D4w_cross.Droid.Helpers
{
    class PermissionRequesterImplementation : IPermissionRequester
    {

        public PermissionRequesterImplementation() { }

        public bool IsGPSEnabled()
        {
            LocationManager locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        public bool AskForGPS()
        {
            bool res = IsGPSEnabled();
            if (res == false)
            {
                Intent gpsSettingIntent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                Forms.Context.StartActivity(gpsSettingIntent);
            }
        
            return res;
        }
    }
}