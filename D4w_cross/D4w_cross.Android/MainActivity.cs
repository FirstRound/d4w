using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using System.Threading.Tasks;
using System.IO;
using Android.Content;
using Android.Graphics;
using Java.IO;
using FFImageLoading.Forms.Droid;

namespace D4w_cross.Droid
{
	[Activity (Label = "D4w_cross", Icon = "@drawable/LogoHiRes", Theme="@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }
        public event Action<int, Result, Intent> ActivityResult;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (this.ActivityResult != null)
                this.ActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);
                    //HUJNA EBANAYA
                    Bitmap image = BitmapFactory.DecodeStream(stream);
                    int maxSize = 300;
                    int outWidth;
                    int outHeight;
                    int inWidth = image.Width;
                    int inHeight = image.Height;
                    if (inWidth > inHeight)
                    {
                        outWidth = maxSize;
                        outHeight = (inHeight * maxSize) / inWidth;
                    }
                    else
                    {
                        outHeight = maxSize;
                        outWidth = (inWidth * maxSize) / inHeight;
                    }
              
                    var bitmapScalled = Bitmap.CreateScaledBitmap(image, outWidth, outHeight, false);
                    byte[] bigPicBytes = new byte[bitmapScalled.Width * bitmapScalled.Height * 4];
                    MemoryStream str = new MemoryStream(bigPicBytes);
                    bitmapScalled.Compress(Bitmap.CompressFormat.Png, 80, str);
                    str.Seek(0, SeekOrigin.Begin);
                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(str);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
            CachedImageRenderer.Init(true);

            //  CarouselViewRenderer.Init();
            LoadApplication(new D4w_cross.App ());
           // Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));

        }
    }
}

