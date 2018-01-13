﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Touch;
using Google.Maps;



namespace D4w_cross.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{

        const string MapsApiKey = "AIzaSyC3IHoNYdCGcnbfA2-RdWpVO2nbZrOeUBs";

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
            MapServices.ProvideAPIKey(MapsApiKey);
            global::Xamarin.Forms.Forms.Init();
            CarouselViewRenderer.Init();

            LoadApplication(new D4w_cross.App());
            var res = base.FinishedLaunching(app, options);
            app.KeyWindow.TintColor = UIColor.White;
            UITabBar.Appearance.TintColor = UIColor.FromRGB(223, 112, 13);

            CachedImageRenderer.Init();

            return res;
		}

	}
}
