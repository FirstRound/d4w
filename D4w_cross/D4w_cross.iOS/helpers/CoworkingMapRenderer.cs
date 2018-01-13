using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using D4w_cross.Helpers;
using D4w_cross.Helpers.Other;
using D4w_cross.iOS.Map;
using D4w_cross.Models.CustomEventArgs;
using D4w_cross.Models.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Google.Maps;

[assembly: ExportRenderer(typeof(CoworkingsMap), typeof(CoworkingsMapRenderer))]
namespace D4w_cross.iOS.Map
{
    class CoworkingsMapRenderer : MapRenderer
    {
    }
}