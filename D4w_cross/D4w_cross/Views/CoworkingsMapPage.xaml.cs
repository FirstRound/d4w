using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using D4w_cross.Helpers;
using D4w_cross.Models.API.Search;
using D4w_cross.Helpers.API;
using D4w_cross.Models.Views;
using D4w_cross.Models.CustomEventArgs;
using D4w_cross.Dependencies;

namespace D4w_cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoworkingsMapPage : ContentPage
	{

        private const double DEFAULT_LAT = 55.77580f;
        private const double DEFAULT_LNG = 49.12201f;
        private const int DEFAULT_ZOOM = 5000;

        private const int MIN_COUNT = 1;
        private const int MAX_COUNT = 3;
        private int count = MIN_COUNT;

        public CoworkingsMapPage ()
		{
			InitializeComponent ();

            TopPopup.IsVisible = false;
            
            InitMap();
            CwMap.CoworkingClicked += OnCoworking;

            Task.Run(() => UpdateCoworkingList());
            ToolbarFilters.Command = new Command(() => { OnPopup(null, null); });
            ToolbarOptions.Command = new Command(() => { OnCoworkingList(); });
        }

        public async void UpdateCoworkingList()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Loading.IsVisible = true;
            });

            GlobalObjectsHelper.CwSearchOptions.Limit = 100;
            GlobalObjectsHelper.CwSearchOptions.Offset = 0;
            GlobalObjectsHelper.CwSearchOptions.Radius = 50;
            GlobalObjectsHelper.CwSearchOptions.CorrectDates(new TimeSpan(00, 00, 00), new TimeSpan(23, 59, 59));

            GlobalObjectsHelper.UpdateCoworkings();
            GlobalObjectsHelper.AllCoworkings = GlobalObjectsHelper.Coworkings;

            AddPins();

            Device.BeginInvokeOnMainThread(() =>
            {
                Loading.IsVisible = false;
            });
        }

        private void InitMap()
        {
            var lat = DEFAULT_LAT;
            var lng = DEFAULT_LNG;
            var zoom = DEFAULT_ZOOM;
            if (GlobalObjectsHelper.CwSearchOptions.Lat != null && GlobalObjectsHelper.CwSearchOptions.Lng != null)
            {
                lat = GlobalObjectsHelper.CwSearchOptions.Lat.Value;
                lng = GlobalObjectsHelper.CwSearchOptions.Lng.Value;
                zoom = 2;
            }
            var sp = MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(zoom));
            CwMap.MoveToRegion(sp);
            bool isEnabled = false; //DependencyService.Get<IPermissionRequester>().IsGPSEnabled();
            if (isEnabled)
            {
                CwMap.IsShowingUser = true;
            }
        }

        public void AddPins()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                CwMap.Pins.Clear();
                CwMap.UpdatePins(CwMap, new EventArgs());
            });

            foreach (var coworking in GlobalObjectsHelper.AllCoworkings)
            {
                if (coworking.Lat != null && coworking.Lng != null)
                {
                    var pin = new CoworkingPin();
                    pin.CoworkingId = coworking.Id;
                    if (coworking.Images.Count > 0)
                    {
                        pin.Cover = new LocalStorage().GetImage(coworking.Images[0].Id);
                    }

                    pin.Time = coworking.FormattedTime;
                    pin.Distance = coworking.FormattedDistance;
                    pin.Label = coworking.FullName;
                    pin.Position = new Position(coworking.Lat.Value, coworking.Lng.Value);
                    pin.Amenties = coworking.Amenties.Select((a) => a.RawName).ToList();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CwMap.Pins.Add(pin);
                    });
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                CwMap.UpdatePins(CwMap, new EventArgs());
            });
        }

        private void UpdateTop()
        {
            List<String> arr = new List<String>();
            if (GlobalObjectsHelper.CwSearchOptions.Address != null)
            {
                arr.Add(GlobalObjectsHelper.CwSearchOptions.Address);
            }
            else if (GlobalObjectsHelper.CwSearchOptions.Lat != null && GlobalObjectsHelper.CwSearchOptions.Lng != null)
            {
                arr.Add("Near");
            }
            else
            {
                arr.Add("Anywhere");
            }
            AddressFilter.Text = arr.Last();
            if (GlobalObjectsHelper.CwSearchOptions.BeginDate != null)
            {
                arr.Add(GlobalObjectsHelper.CwSearchOptions.BeginDate.Value.ToString("dd/MM"));
                DateFilter.Text = arr.Last();
            }
            if (GlobalObjectsHelper.CwSearchOptions.BeginWork != null && GlobalObjectsHelper.CwSearchOptions.EndWork != null)
            {
                var from = GlobalObjectsHelper.CwSearchOptions.BeginWork.Value.ToString(@"hh\:mm");
                var to = GlobalObjectsHelper.CwSearchOptions.EndWork.Value.ToString(@"hh\:mm");
                arr.Add(from + " - " + to);
                TimeFilter.Text = from + "       " + to;
            }
            var text = String.Join(" / ", arr);
            Parking.IsChecked = GlobalObjectsHelper.CwSearchOptions.Parking;
            Printing.IsChecked = GlobalObjectsHelper.CwSearchOptions.Printing;
            FreePrinting.IsChecked = GlobalObjectsHelper.CwSearchOptions.FreePrinting;
            FreeParking.IsChecked = GlobalObjectsHelper.CwSearchOptions.FreeParking;
            Food.IsChecked = GlobalObjectsHelper.CwSearchOptions.Food;
            Coffee.IsChecked = GlobalObjectsHelper.CwSearchOptions.Coffee;

            //DependencyService.Get<IActionBarChanger>().ChangeTitle(text);
            ToolbarFilters.Name = text;
        }

        public void OnCoworking(object sender, EventArgs e)
        {
            var args = e as CoworkingMapEventArgs;
            var coworking = GlobalObjectsHelper.AllCoworkings.Where(c => c.Id == args.CoworkingId).FirstOrDefault();
            GlobalObjectsHelper.SelectedCoworking = coworking;
            Navigation.PushAsync(new CoworkingDetailPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DependencyService.Get<IActionBarChanger>().InitClick(OnPopup);
            UpdateTop();
        }

        void OnCoworkingList()
        {
            Navigation.InsertPageBefore(new CoworkingsListPage(), this);
            Navigation.PopAsync();        
        }

        void OnPopup(object sender, EventArgs e)
        {
            TopPopup.IsVisible = !TopPopup.IsVisible;
        }

        public void OnWhere(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LocationPage());
        }

        public void OnWhen(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DatePage());
        }

        public void OnTime(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TimePage());
        }

        public void OnApply(object sender, EventArgs e)
        {
            TopPopup.IsVisible = false;
            DBHelper.Instance.SaveOptions(GlobalObjectsHelper.CwSearchOptions);
            UpdateTop();
            InitMap();
            Task.Run(() => UpdateCoworkingList());
        }

        public void OnPlus(object sender, EventArgs e)
        {
            if (count < MAX_COUNT)
                count++;
            GlobalObjectsHelper.CwSearchOptions.FreeCount = count;
            SeatsNum.Text = count.ToString();
        }

        public void OnMinus(object sender, EventArgs e)
        {
            if (count > MIN_COUNT)
                count--;
            GlobalObjectsHelper.CwSearchOptions.FreeCount = count;
            SeatsNum.Text = count.ToString();
        }

        public void OnParking(object sender, EventArgs e)
        {
            var check = sender as CheckBoxImage;
            GlobalObjectsHelper.CwSearchOptions.Parking = check.IsChecked;
        }

        public void OnCoffee(object sender, EventArgs e)
        {
            var check = sender as CheckBoxImage;
            GlobalObjectsHelper.CwSearchOptions.Coffee = check.IsChecked;
        }


        public void OnFreeParking(object sender, EventArgs e)
        {
            var check = sender as CheckBoxImage;
            GlobalObjectsHelper.CwSearchOptions.FreeParking = check.IsChecked;
        }

        public void OnPrinting(object sender, EventArgs e)
        {
            var check = sender as CheckBoxImage;
            GlobalObjectsHelper.CwSearchOptions.Printing = check.IsChecked;
        }

        public void OnFreePrinting(object sender, EventArgs e)
        {
            var check = sender as CheckBoxImage;
            GlobalObjectsHelper.CwSearchOptions.FreePrinting = check.IsChecked;
        }

        public void OnFood(object sender, EventArgs e)
        {
            var check = sender as CheckBoxImage;
            GlobalObjectsHelper.CwSearchOptions.Food = check.IsChecked;
        }
    }
}