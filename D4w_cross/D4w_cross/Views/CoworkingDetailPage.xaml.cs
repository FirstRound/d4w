using D4w_cross.Helpers;
using D4w_cross.Helpers.API;
using D4w_cross.Helpers.Other;
using D4w_cross.Models.API;
using D4w_cross.Models.CustomEventArgs;
using D4w_cross.Models.Views;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D4w_cross.Views
{

    public class Template : DataTemplateSelector
    {
        public DataTemplate ValidTemplate { get; set; }
        public DataTemplate InvalidTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ValidTemplate;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoworkingDetailPage : ContentPage
    {

        private const int WDAYS_COUNT = 4;

        private LocalStorage localStorage;

        private DateTime СorrectDate(DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year,
                                date.Month,
                                date.Day,
                                time.Hours,
                                time.Minutes,
                                time.Seconds);
        }

        public CoworkingDetailPage()
        {
            InitializeComponent();

            localStorage = new LocalStorage();

            //TODO: Refactor
            var cw = GlobalObjectsHelper.SelectedCoworking;
            var opt = GlobalObjectsHelper.CwSearchOptions;

            if (opt.BeginDate != null && opt.EndDate != null && opt.BeginWork != null && opt.BeginWork != null)
            {
                var freeSeats = new CoworkingsHelper().GetAvailableSeats(cw.Id, СorrectDate(opt.BeginDate.Value, opt.BeginWork.Value), СorrectDate(opt.EndDate.Value, opt.EndWork.Value));
                var seatsCount = 0;
                if (freeSeats.Status == System.Net.HttpStatusCode.OK)
                {
                    seatsCount = freeSeats.SeatsNum.Value;
                }
                GlobalObjectsHelper.SelectedCoworking.UpdateSeatsAvailable(seatsCount);
            }

            int count = cw.WorkingDays.Count;
            Hours1.ItemsSource = cw.WorkingDays.GetRange(0, Math.Min(WDAYS_COUNT, count));
            if (cw.WorkingDays.Count > WDAYS_COUNT)
                Hours2.ItemsSource = cw.WorkingDays.GetRange(WDAYS_COUNT, count - WDAYS_COUNT);

            foreach (var amenty in cw.Amenties)
            {
                var imgSource = IconGetter.GetAmentyIcon(amenty.RawName);
                if (imgSource != null)
                {

                    var stack = new StackLayout();
                    var label = new Label
                    {
                        Text = amenty.Name,
                        TextColor = Color.Black
                    };
                    var img = new AmentyImage
                    {
                        Source = ImageSource.FromFile(imgSource),
                        WidthRequest = 50,
                        HeightRequest = 50,
                        Value = amenty.Description
                    };

                    img.Clicked += OnAmenty;
                    stack.Children.Add(img);
                    stack.Children.Add(label);

                    Amenties.Children.Add(stack);
                }
            }

            Task.Run(()=> { LoadImages(cw); });

            BindingContext = cw;
        }

        private async void LoadImages(Coworking cw)
        {
            for (int i = 0; i < cw.Images.Count; i++)
            {
                var stack = new StackLayout();
                var img = new CachedImage
                {
                    WidthRequest = 300,
                    HeightRequest = 200,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.End,
                    Aspect = Aspect.AspectFit
                };
                Device.BeginInvokeOnMainThread(() =>
                {
                    stack.Children.Add(img);
                    Images.Children.Add(stack);
                });
                LoadImage(cw.Images[i], img);
            }
        }


        private void OnAmenty(object semder, EventArgs e)
        {
            AmentyDescription.Text = (e as AmentyEventArgs).Img.Value;
        }

        private void LoadImage(Models.API.Image img, CachedImage imgControl)
        {
            var image = localStorage.GetImage(img.Id);
            var regex = new Regex("data:image.*base64,");
            image.Base64 = regex.Replace(image.Base64, String.Empty);
            try
            {
                //var resized = ImageResizer.ResizeImage(image, 600, 400);
                var formsImage = Xamarin.Forms.ImageSource.FromStream(
                               () => { return new MemoryStream(Convert.FromBase64String(image.Base64)); });
                Device.BeginInvokeOnMainThread(() =>
                {
                    imgControl.Source = formsImage;
                });               
            }
            catch (Exception e)
            {

            }
        }

        void OnDay(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Hours1.SelectedItem = null;
                Hours2.SelectedItem = null;
            }
        }

        public async void OnHoldSeat(object sender, EventArgs e)
        {
            var opt = GlobalObjectsHelper.CwSearchOptions;
            var cw = GlobalObjectsHelper.SelectedCoworking;
            var cnt = opt.FreeCount;
            if (cnt == null)
                cnt = 1;
            if (opt.BeginDate == null || opt.EndWork == null || opt.BeginWork == null || opt.EndDate == null)
            {
                await DisplayAlert("Error!", "Please, select the date and time in menu above!", "Ok");
            }
            else if (cw.SeatsAvailable < cnt)
            {
                await DisplayAlert("Error!", "No seats available!", "Ok");
            }
            else
            {
                var booking = new Booking();
                booking.BeginDate = opt.BeginDate;
                booking.EndDate = opt.EndDate;
                booking.CorrectDates(opt.BeginWork.Value, opt.EndWork.Value);
                booking.VisitorsCount = cnt;
                booking.CoworkingId = cw.Id;

                var token = DBHelper.Instance.GetToken();
                if (token == null)
                {
                    App.Current.MainPage = new SignUpPage(booking);
                }
                else
                {
                    booking = new BookingsHelper().Create(booking);
                    if (booking.Status == System.Net.HttpStatusCode.OK)
                    {
                        DBHelper.Instance.AddBooking(booking);
                        //switch tab
                        var tabs = (MainPage)App.Current.MainPage;
                        tabs.CurrentPage = tabs.Children[1];
                        var bookingPage = (BookingPage)tabs.CurrentPage;
                        bookingPage.Start();
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Unknown error occured! Please, try again later.", "Ok");
                    }
                }
            }
        }
        
    }
}