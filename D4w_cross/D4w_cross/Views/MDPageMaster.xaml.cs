using D4w_cross.Helpers;
using D4w_cross.Models.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D4w_cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MDPageMaster : ContentPage
    {
        private const int MIN_COUNT = 1;
        private const int MAX_COUNT = 3;

        private int count = MIN_COUNT;
      
        private HashSet<String> amenties;

        public MDPageMaster()
        {
            InitializeComponent();
            amenties = new HashSet<String>();
        }

        public void OnPlus(object sender, EventArgs e)
        {
            if (count < MAX_COUNT)
                count++;
            SeatsNum.Text = count.ToString();
        }

        public void OnMinus(object sender, EventArgs e)
        {
            if (count > MIN_COUNT)
                count--;
            SeatsNum.Text = count.ToString();
        }

        public void OnToggle(object sender, EventArgs e)
        {
            bool isEnabled = DependencyService.Get<IPermissionRequester>().AskForGPS();
            if (isEnabled)
            {
               
            }
            else
            {
                SortNear.IsToggled = false;
            }
        }

        public void OnAmenty(object sender, EventArgs e)
        {
            var checkbox = (e as CheckBoxEventArgs).CheckBox;
            if (checkbox.IsChecked)
            {
                amenties.Add(checkbox.Value);
            }
            else
            {
                amenties.Remove(checkbox.Value);
            }
            
        }

        public void OnShow(object sender, EventArgs e)
        {
            var options = GlobalObjectsHelper.CwSearchOptions;

            if (SortNear.IsToggled)
            {
                var pos = LocationHelper.GetLocation();
                options.Lat = pos.Latitude;
                options.Lng = pos.Longitude;
            }
            else
            {
                options.Lat = null;
                options.Lng = null;
            }

            options.FreeCount = count;
            //options.Amenties = amenties.ToList();

            DBHelper.Instance.SaveOptions(GlobalObjectsHelper.CwSearchOptions);
            //AAA SUKAAA
            var tab = App.Current.MainPage as TabbedPage;
            var mp = tab.CurrentPage as MDPage;
            var np = mp.Detail as NavigationPage;
            var cwrk = np.Navigation.NavigationStack.Last() as CoworkingsListPage;
            cwrk.UpdateCoworkingList();
            mp.IsPresented = false;
        }
    }
}