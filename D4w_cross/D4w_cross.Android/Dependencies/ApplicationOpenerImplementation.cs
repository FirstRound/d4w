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
using Xamarin.Forms;
using D4w_cross.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency(typeof(ApplicationOpenerImplementation))]
namespace D4w_cross.Droid.Dependencies
{
    class ApplicationOpenerImplementation : IApplicationOpener
    {
        public ApplicationOpenerImplementation() { }

        public void OpenMapApp(double latTo, double lngTo)
        {
            var gmmIntentUri = Android.Net.Uri.Parse(String.Format("google.navigation:q={0},{1}", latTo.ToString("0.00000", System.Globalization.CultureInfo.InvariantCulture), lngTo.ToString("0.00000", System.Globalization.CultureInfo.InvariantCulture)));
            Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
            mapIntent.SetPackage("com.google.android.apps.maps");
            (Forms.Context as Activity).StartActivity(mapIntent);
        }

        public void OpenMapApp(string address)
        {
            var gmmIntentUri = Android.Net.Uri.Parse(String.Format("google.navigation:q={0}", address));
            Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
            mapIntent.SetPackage("com.google.android.apps.maps");
            (Forms.Context as Activity).StartActivity(mapIntent);
        }
    }
}