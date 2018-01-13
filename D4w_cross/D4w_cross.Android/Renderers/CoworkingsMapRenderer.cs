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
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using static Android.Gms.Maps.GoogleMap;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Util;
using D4w_cross.Helpers;
using D4w_cross.Models.CustomEventArgs;
using System.IO;
using Java.IO;
using Android.Content.Res;
using Android.Graphics.Drawables;
using D4w_cross.Helpers.Other;
using static Android.Widget.ImageView;

[assembly: ExportRenderer(typeof(CoworkingsMap), typeof(CoworkingsMapRenderer))]
namespace D4w_cross.Droid.Helpers
{
    class CoworkingsMapRenderer : MapRenderer, IInfoWindowAdapter
    {
        private List<Pin> pins = new List<Pin>();
        private bool mapDrawn; 

        public event EventHandler CoworkingClicked;
        private CoworkingsMap formsMap;
        private Marker me;

        protected override void OnMapReady(GoogleMap map)
        {
            if (mapDrawn) return;
            base.OnMapReady(map);

            NativeMap.SetInfoWindowAdapter(this);

            NativeMap.MarkerClick += OnMarkerClick;
            NativeMap.InfoWindowClick += OnInfoWindowClick;

            pins = new List<Pin>(formsMap.Pins);
            CoworkingClicked += formsMap.OnCoworking;
            formsMap.PinsUpdated += OnPinsUpdate;

            bool isEnabled = DependencyService.Get<IPermissionRequester>().IsGPSEnabled();

            if (me != null)
                me.Remove();

            if (isEnabled)
            {
                var ret = LocationHelper.GetLocation();
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(ret.Latitude, ret.Longitude));

                var bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.HereIcon);
                marker.SetIcon(BitmapDescriptorFactory.FromBitmap(Bitmap.CreateScaledBitmap(bitmap, 350, 200, false)));
                marker.Draggable(false);
                me = NativeMap.AddMarker(marker);
            }

            mapDrawn = true;
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.MarkerClick -= OnMarkerClick;
            }

            if (e.NewElement != null)
            {
                formsMap = (CoworkingsMap)e.NewElement;
                Control.GetMapAsync(this);
            }
        }

        void OnMarkerClick(object sender, MarkerClickEventArgs e)
        {
            e.Marker.ShowInfoWindow();
        }

        void OnPinsUpdate(object sender, EventArgs e)
        {
            //NativeMap.Clear();
            var formsMap = sender as CoworkingsMap;
            pins = new List<Pin>(formsMap.Pins);
           
        }

        void OnInfoWindowClick(object sender, InfoWindowClickEventArgs e)
        {
            var pin = GetMapPin(e.Marker);
            CoworkingClicked.Invoke(sender, new CoworkingMapEventArgs { CoworkingId = pin.CoworkingId });
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            return null;
          
        }

        private Bitmap GetBitmap(Xamarin.Forms.ImageSource image)
        {
            var task = Task.Run<Bitmap>(async () => {
                return await new ImageLoaderSourceHandler().LoadImageAsync(image, Context);
             });
            
            //var x = t.Result;
            return task.Result;
        }


        public Android.Views.View GetInfoWindow(Marker marker) {
            int cnt = 0;

            var pin = GetMapPin(marker);
            if (pin == null)
                return null;

            foreach (var amenty in pin.Amenties)
            {
                var imgSource = IconGetter.GetAmentyIcon(amenty);

                if (imgSource != null)
                {
                    cnt++;
                }
            }

            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            var view = inflater.Inflate(Resource.Layout.CoworkingWindowInfo, null);
            if (cnt != 0)
                view = inflater.Inflate(Resource.Layout.CoworkingWindowInfoAmenty, null);
            var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
            var image = view.FindViewById<ImageView>(Resource.Id.CoworkingImage);
            var workTime = view.FindViewById<TextView>(Resource.Id.WorkTime);
            var distance = view.FindViewById<TextView>(Resource.Id.Distance);

            if (cnt != 0)
            {
                var amenties = view.FindViewById<LinearLayout>(Resource.Id.Linear);
                foreach (var amenty in pin.Amenties)
                {
                    var imgSource = IconGetter.GetAmentyIcon(amenty);
                    if (imgSource != null)
                    {
                        ImageView imageView = new ImageView(Context);
                        imageView.SetPadding(0, 0, 0, 0);
                        imageView.SetAdjustViewBounds(true);
                        int id = (int)typeof(Resource.Drawable).GetField(imgSource.Replace(".png", "")).GetValue(null);
                        imageView.SetImageBitmap(BitmapFactory.DecodeResource(Resources, id));
                        amenties.AddView(imageView);
                    }
                }
            }
  
            if (pin.Cover != null)
            {
                var compressed = ImageResizer.ResizeImage(pin.Cover, 300, 170);
                var decodedString = Base64.Decode(compressed.Base64, Base64.Default);
                Bitmap decodedByte = BitmapFactory.DecodeByteArray(decodedString, 0, decodedString.Length);
                image.SetImageBitmap(decodedByte);
            }
            else
            {
                var bmp = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Noimage);
                image.SetImageBitmap(bmp);
            }
        
            workTime.Text = pin.Time;
            distance.Text = pin.Distance;

            infoTitle.Text = marker.Title;
            return view;
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var mapPin = pin as CoworkingPin;
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(mapPin.Position.Latitude, mapPin.Position.Longitude));
            marker.SetTitle(mapPin.Label);
            marker.SetSnippet(mapPin.Address);
            //marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.Pin));
            marker.Draggable(true);

            return marker;
        }

        CoworkingPin GetMapPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in pins)
            {
                if (pin.Position == position)
                {
                    return pin as CoworkingPin;
                }
            }
            return null;
        }

    }
}