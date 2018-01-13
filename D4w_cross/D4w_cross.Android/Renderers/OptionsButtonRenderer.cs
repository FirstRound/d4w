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
using D4w_cross.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(OptionsButton), typeof(OptionsButtonRenderer))]

namespace D4w_cross.Droid.Helpers
{
    class OptionsButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                
                Control.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;
            }
        }
    }
}