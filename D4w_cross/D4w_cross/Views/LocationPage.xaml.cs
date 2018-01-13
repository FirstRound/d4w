using D4w_cross.Helpers;
using D4w_cross.Models.API.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D4w_cross.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
	{
        private const int NEARBY = 0;
        private const int EVERYWHERE = 1;

        private Dictionary<string, Point> coord = null;

		public LocationPage()
		{
			InitializeComponent ();
            var types = new List<String>
            {
                Application.Current.Resources["nearby"].ToString(),
                Application.Current.Resources["everywhere"].ToString()
            };
            LocationTypes.ItemsSource = types;

            var popular = new List<String>
            {
                "Kazan", "Moscow", "Saint Petersburg", "Ekaterinburg"
            };

            coord = new Dictionary<string, Point>();
            coord.Add("Kazan", new Point(55.830431, 49.066081));
            coord.Add("Moscow", new Point(55.755826, 37.617300));
            coord.Add("Saint Petersburg", new Point(59.934280, 30.335099));
            coord.Add("Ekaterinburg", new Point(56.838926, 60.605703));

            Popular.ItemsSource = popular;
        }

        void OnLocationType(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; 
            }
            var index = (LocationTypes.ItemsSource as List<String>).IndexOf(e.SelectedItem.ToString());
            
            if (index == NEARBY)
            {
                var pos = LocationHelper.GetLocation();
                GlobalObjectsHelper.CwSearchOptions.Lat = pos.Latitude;
                GlobalObjectsHelper.CwSearchOptions.Lng = pos.Longitude;
            }
            else if (index == EVERYWHERE)
            {
                GlobalObjectsHelper.CwSearchOptions.Lat = null;
                GlobalObjectsHelper.CwSearchOptions.Lng = null;
                GlobalObjectsHelper.CwSearchOptions.Address = null;
            }
            else
            {
                GlobalObjectsHelper.CwSearchOptions.Address = e.SelectedItem.ToString();
                //GlobalObjectsHelper.CwSearchOptions.Lat = coord[e.SelectedItem.ToString()].X;
                //GlobalObjectsHelper.CwSearchOptions.Lng = coord[e.SelectedItem.ToString()].Y;
            }
            Navigation.PopAsync();
        }

        void OnPopular(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            else
            {
                GlobalObjectsHelper.CwSearchOptions.Address = e.Item.ToString();
                GlobalObjectsHelper.CwSearchOptions.Lat = coord[e.Item.ToString()].X;
                GlobalObjectsHelper.CwSearchOptions.Lng = coord[e.Item.ToString()].Y;
                Navigation.PopAsync();
            }
        }
    }
}