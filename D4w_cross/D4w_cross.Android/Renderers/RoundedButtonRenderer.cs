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
using D4w_cross.Models.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using D4w_cross.Droid.Helpers;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]

namespace D4w_cross.Droid.Helpers
{
    class RoundedButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.SetBackgroundResource(Resource.Layout.RoundedButton);
            }
        }
    }
}