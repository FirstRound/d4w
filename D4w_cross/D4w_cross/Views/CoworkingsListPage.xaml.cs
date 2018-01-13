using D4w_cross.Dependencies;
using D4w_cross.Helpers;
using D4w_cross.Helpers.API;
using D4w_cross.Models.API;
using D4w_cross.Models.API.Search;
using D4w_cross.Models.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D4w_cross.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CoworkingsListPage : ContentPage
	{
        private const int MIN_COUNT = 1;
        private const int MAX_COUNT = 3;

        private int count = MIN_COUNT;
        private ObservableCollection<Coworking> coworkings = new ObservableCollection<Coworking>();
        private LocalStorage localStorage;


        public CoworkingsListPage()
        {
            InitializeComponent();

            TopPopup.IsVisible = false;

            localStorage = new LocalStorage();
            Coworkings.ItemsSource = coworkings;

            Task.Run(() => UpdateCoworkingList());
            ToolbarFilters.Command = new Command (() => { OnPopup(null, null ); });

            ToolbarMap.Command = new Command(() => { OnCoworkingMap(); });
        }

        private async void UpdatePictures()
        {

            //load pictures
                for (int i = 0; i < GlobalObjectsHelper.Coworkings.Count; i++)
                {
                    if (GlobalObjectsHelper.Coworkings[i].Images.Count > 0)
                    {                   
                        LoadImage(GlobalObjectsHelper.Coworkings[i]);
                    }
                    else
                    {
                        GlobalObjectsHelper.Coworkings[i].UpdateDisplayImage(ImageSource.FromFile("Noimage.png"));
                    }
                }
        }


        public async void UpdateCoworkingList()
        {
            Device.BeginInvokeOnMainThread (() => {
                coworkings.Clear();
            });
            GlobalObjectsHelper.CwSearchOptions.Limit = 10;
            GlobalObjectsHelper.CwSearchOptions.Offset = 0;
            GlobalObjectsHelper.CwSearchOptions.CorrectDates(new TimeSpan(00, 00, 00), new TimeSpan(23, 59, 59));
            GlobalObjectsHelper.UpdateCoworkings();
            
            foreach (var cw in GlobalObjectsHelper.Coworkings)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    coworkings.Add(cw);
                });
            }
            GlobalObjectsHelper.CwSearchOptions.Offset += GlobalObjectsHelper.Coworkings.Count;

            //TODO: Fix it
            //UpdatePictures();

            GlobalObjectsHelper.AllCoworkings = coworkings;
            Device.BeginInvokeOnMainThread(() =>
            {
                Running.IsRunning = false;
                Running.IsVisible = false;
                if (GlobalObjectsHelper.AllCoworkings.Count == 0)
                {
                    NoCoworkings.IsVisible = true;
                }
                else
                {
                    NoCoworkings.IsVisible = false;
                }
            });
        }

        private void LoadImage(Coworking coworking)
        {
            var image = localStorage.GetImage(coworking.Images[0].Id);
            coworking.UpdateDisplayImage(image);
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DependencyService.Get<IActionBarChanger>().InitClick(OnPopup);
            UpdateTop();
        }

        void OnCoworkingMap()
        {
            Navigation.InsertPageBefore(new CoworkingsMapPage(), this);
            Navigation.PopAsync();
        }

        void OnPopup(object sender, EventArgs e)
        {
            TopPopup.IsVisible = !TopPopup.IsVisible;
        }

        async void OnCoworking(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var coworking = e.SelectedItem as Coworking;
                GlobalObjectsHelper.SelectedCoworking = coworking;
                Coworkings.SelectedItem = null;
                Navigation.PushAsync(new CoworkingDetailPage());
            }
        }

        void OnCoworkingAppeared(object sender, ItemVisibilityEventArgs e)
        {
            if (e.Item != null && GlobalObjectsHelper.AllCoworkings.Count > 0)
            {
                var coworking = e.Item as Coworking;

                if (coworking.DisplayImage == null)
                {
                    if (coworking.Images.Count > 0)
                    {
                        Task.Run(()=>LoadImage(coworking));
                    }
                    else
                    {
                        coworking.UpdateDisplayImage(ImageSource.FromFile("Noimage.png"));
                    }
                }


                var index = GlobalObjectsHelper.AllCoworkings.IndexOf(coworking);
                if (index == GlobalObjectsHelper.AllCoworkings.Count - 1)
                {
                    GlobalObjectsHelper.CwSearchOptions.CorrectDates(new TimeSpan(00, 00, 00), new TimeSpan(23, 59, 59));
                    GlobalObjectsHelper.UpdateCoworkings();
                    foreach (var cw in GlobalObjectsHelper.Coworkings)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            coworkings.Add(cw);
                        });
                    }
                    GlobalObjectsHelper.CwSearchOptions.Offset += 10;//GlobalObjectsHelper.Coworkings.Count;
                    //UpdateIndexes();
                    //Task.Run(()=> UpdatePictures());
                    GlobalObjectsHelper.AllCoworkings = coworkings;
                }
            }

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (GlobalObjectsHelper.AllCoworkings.Count == 0)
                    {
                        NoCoworkings.IsVisible = true;
                    }
                    else
                    {
                        NoCoworkings.IsVisible = false;
                    }
                });
            
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
            DBHelper.Instance.SaveOptions(GlobalObjectsHelper.CwSearchOptions);
            TopPopup.IsVisible = false;
            var first = ((MainPage)App.Current.MainPage).Children[0] as MDPage;
            UpdateTop();

            Running.IsVisible = true;
            Running.IsRunning = true;
            var thr = new Thread(() =>
            {
                UpdateCoworkingList();
            });
            thr.Start();
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