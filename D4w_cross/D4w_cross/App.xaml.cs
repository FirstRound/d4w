using D4w_cross.Helpers;
using D4w_cross.Models.API.Search;
using D4w_cross.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace D4w_cross
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            bool isEnabled = false;//DependencyService.Get<IPermissionRequester>().IsGPSEnabled();
            GlobalObjectsHelper.CwSearchOptions = DBHelper.Instance.GetOptions();
            GlobalObjectsHelper.CwSearchOptions.Update();

            if (isEnabled)
            {
                var ret = LocationHelper.GetLocation();
                GlobalObjectsHelper.CwSearchOptions.Lat = ret.Latitude;
                GlobalObjectsHelper.CwSearchOptions.Lng = ret.Longitude;
            }

            if (DBHelper.Instance.GetToken() == null)
            {
                MainPage = new NavigationPage(new LoginPage());
                
            }
            else
            {

                MainPage = new MainPage();
            }
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
