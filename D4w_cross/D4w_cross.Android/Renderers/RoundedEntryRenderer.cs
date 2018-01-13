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
using Xamarin.Forms.Platform.Android;
using D4w_cross.Models.Views;
using D4w_cross.Droid.Helpers;
using Xamarin.Forms;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]

namespace D4w_cross.Droid.Helpers
{
   
    public class RoundedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                /*GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(25);
                Control.SetBackgroundColor(Android.Graphics.Color.Black);
                
                Control.Background = gd;*/
                Control.SetPadding(0, 20, 0, 0);
                if (e.NewElement.IsPassword)
                    Control.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
                Control.SetBackgroundResource(Resource.Layout.RoundedEntry);
            }
            //Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}