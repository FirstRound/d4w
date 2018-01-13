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
using D4w_cross.Dependencies;
using D4w_cross.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency(typeof(CustomDIalogAlertImplementation))]
namespace D4w_cross.Droid.Dependencies
{
    class CustomDIalogAlertImplementation : ICustomDIalogAlert
    {


        public void Show(string title, string text, string okText)
        {

        }

        public bool Show(string title, string text, string okText, string cancelText)
        {
            return true;
        }
    }
}