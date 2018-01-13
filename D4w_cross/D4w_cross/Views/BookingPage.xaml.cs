using D4w_cross.Dependencies;
using D4w_cross.Helpers;
using D4w_cross.Helpers.API;
using D4w_cross.Models;
using D4w_cross.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace D4w_cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookingPage : ContentPage
    {
        private HoldOnContainer container;
        private DBHelper dbHelper;
        private Booking booking;
        private Coworking coworking;

        public BookingPage()
        {
            InitializeComponent();

            dbHelper = DBHelper.Instance;

            Start();
        }


        public void Start()
        {

            BookingCard.IsVisible = false;
            BookingStartedCard.IsVisible = false;
            BookingFinishedCard.IsVisible = false;
            Empty.IsVisible = true;
            PayCard.IsVisible = false;

            BeforeStart.IsVisible = false;
            AfterStart.IsVisible = false;
            //add retrieve from server
            /*
            Booking currentBooking = new Booking();
            currentBooking.BeginDate = new DateTime(2017, 11, 18, 15, 25, 00);
            currentBooking.EndDate = new DateTime(2017, 11, 18, 16, 29, 00);
            currentBooking.VisitTime = new DateTime(2017, 11, 18, 15, 11, 00);
            currentBooking.IsVisitConfirmed = true;
            currentBooking.IsClosed = false;
            Booking futureBooking = null;*/
            if (DBHelper.Instance.GetToken() == null)
            {
                return;
            }

            var futureBooking = new UsersHelper().GetFutureBooking(DateTime.Now);
            var currentBooking = new UsersHelper().GetCurrentBooking(DateTime.Now);
            if (currentBooking.Status == System.Net.HttpStatusCode.Forbidden)
            {
                DBHelper.Instance.DeleteToken();
                Navigation.PopAsync();
            }

            if (currentBooking.Status == System.Net.HttpStatusCode.OK && currentBooking.Price != null && currentBooking.IsPayed == false && currentBooking.IsClosed == true && currentBooking.IsVisitConfirmed == true)
            {
                booking = currentBooking;
                PayCard.IsVisible = true;
                Empty.IsVisible = false;
                Price.Text = booking.Price;
                return;
            }

            if (currentBooking.Status == System.Net.HttpStatusCode.OK && CheckBooking(currentBooking))
            {
                if (currentBooking.IsVisitConfirmed == true && currentBooking.IsExtendPending == false)
                {
                    booking = currentBooking;
                    coworking = new CoworkingsHelper().Get(booking.CoworkingId);
                    
                    UpdateContainer(coworking);
                    StartWorkTimer();
                    UpdateMap(coworking);
                    futureBooking.Status = System.Net.HttpStatusCode.NotFound;
                }
                else
                {
                    futureBooking = currentBooking;
                }
            }

            if (futureBooking.Status == System.Net.HttpStatusCode.OK && CheckBooking(futureBooking))
            {
                booking = futureBooking;
                coworking = new CoworkingsHelper().Get(booking.CoworkingId);

                UpdateContainer(coworking);
                if (futureBooking.IsVisitConfirmed == true && currentBooking.IsExtendPending == false)
                {
                    StartWorkTimer();
                }
                else
                {
                    StartWaitTimer();
                }
                UpdateMap(coworking);

            }

        }

        private bool CheckBooking(Booking booking)
        {
            if (booking != null)
            {
                return !booking.IsClosed.Value && !booking.IsUserCanceling.Value && !booking.IsUserLeaving.Value;
            }
            return false;
        }

        private void UpdateContainer(Coworking coworking)
        {
            container = new HoldOnContainer();
            container.CurBooking = booking;
            container.CurCoworking = coworking;

            if (coworking.Images.Count > 0)
            {
                var image = new LocalStorage().GetImage(coworking.Images[0].Id);
                var compressed = ImageResizer.ResizeImage(image, 800, 600);
                coworking.UpdateDisplayImage(compressed);
            }
            else
            {
                coworking.UpdateDisplayImage(ImageSource.FromFile("Noimage.png"));
            }

            BindingContext = container;
        }

        private void UpdateMap(Coworking coworking)
        {
            var lat = 55.77;
            var lng = 49.19;
            if (coworking.Lat != null)
                lat = coworking.Lat.Value;
            if (coworking.Lng != null)
                lng = coworking.Lng.Value;
            var map = new Map(
                    MapSpan.FromCenterAndRadius(
                    new Position(lat, lng), Distance.FromMiles(2)))
            {
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var pin = new Pin();
            pin.Label = "";
            pin.Position = new Position(lat, lng);
            map.Pins.Add(pin);

            MapLayout.Children.Add(map, Constraint.Constant(0), Constraint.Constant(0),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height;
                }));
        }

        private void StartWorkTimer()
        {
            container.Stop();
            container.OnStop -= OnBeginStop;
            container.OnStop += OnEndStop;
            container.TimeFormat = @"hh\ \:\ mm\ \:\ ss";
            container.Start(booking.EndDate.Value);

            BookingCard.IsVisible = false;
            BookingStartedCard.IsVisible = true;
            BookingFinishedCard.IsVisible = false;
            Empty.IsVisible = false;
            PayCard.IsVisible = false;
        }

        private void StartWaitTimer()
        {
            container.Stop();
            container.TimeFormat = @"dd\:hh\:mm";
            container.OnStop += OnBeginStop;
            container.Start(booking.BeginDate.Value);

            BookingCard.IsVisible = true;
            BookingStartedCard.IsVisible = false;
            BookingFinishedCard.IsVisible = false;
            Empty.IsVisible = false;
            PayCard.IsVisible = false;

            BeforeStart.IsVisible = true;
            AfterStart.IsVisible = false;
        }

        void OnBeginStop(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var checkedBooking = new BookingsHelper().Get(booking.Id.Value);
                if (checkedBooking.IsVisitConfirmed.Value && !checkedBooking.IsClosed.Value && !checkedBooking.IsExtendPending.Value)
                {
                    StartWorkTimer();
                }
                else
                {
                    BeforeStart.IsVisible = false;
                    AfterStart.IsVisible = true;
                    container.OnStop -= OnBeginStop;
                }
            });
        }

        void OnEndStop(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                BookingStartedCard.IsVisible = false;
                BookingFinishedCard.IsVisible = true;
            });
        }

        async void OnCheckIn(object sender, EventArgs e)
        {
            var checkedBooking = new BookingsHelper().Get(booking.Id.Value);
            if (checkedBooking.IsVisitConfirmed.Value && !booking.IsExtendPending.Value)
            {
                container.CurBooking.VisitTime = checkedBooking.VisitTime;
                StartWorkTimer();
            }
            else
            {
                await DisplayAlert("Error!", "You are not checked in at administrator!", "Ok");
            }
        }

        async void OnCancel(object sender, EventArgs e)
        {
            var cancel = await DisplayAlert("Question?", "Are you sure you want to cancel booking?", "Yes", "No");
            if (cancel)
            {
                var bk = new BookingsHelper().CancelBooking(booking.Id.Value);
                if (bk.Status == System.Net.HttpStatusCode.OK)
                {
                    Start();
                }
            }
        }

        void OnTimePlus(object sender, EventArgs e)
        {
            container.AdditionalTime += new TimeSpan(0, 30, 0);
            container.UpdateAdditionalTime();
        }

        void OnTimeMinus(object sender, EventArgs e)
        {
            container.AdditionalTime -= new TimeSpan(0, 30, 0);
            container.UpdateAdditionalTime();
        }

        async void OnCheckOut(object sender, EventArgs e)
        {
            booking = new BookingsHelper().Get(booking.Id.Value);
            if (booking.Status == System.Net.HttpStatusCode.OK)
            {
                if (booking.IsClosed.Value == true && booking.Price != null)
                {
                    Price.Text = booking.Price;
                    BookingCard.IsVisible = false;
                    BookingStartedCard.IsVisible = false;
                    BookingFinishedCard.IsVisible = false;
                    Empty.IsVisible = false;
                    PayCard.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Error!", "Please, come to administrator and press this button after check out.", "Ok");
                }
            }

        }

        async void OnFinish(object sender, EventArgs e)
        {
            booking = new BookingsHelper().Get(booking.Id.Value);
            if (booking.Status == System.Net.HttpStatusCode.OK)
            {
                if (booking.IsClosed.Value == true && booking.Price != null)
                {
                    Price.Text = booking.Price;
                    BookingCard.IsVisible = false;
                    BookingStartedCard.IsVisible = false;
                    BookingFinishedCard.IsVisible = false;
                    Empty.IsVisible = false;
                    PayCard.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Error!", "Please, ensure that administrator finished your booking!", "Ok");
                }
            }
            
        }

        async void OnPay(object sender, EventArgs e)
        {
            var token = new PaymentsHelper().GetClientToken();
            if (token.Status == System.Net.HttpStatusCode.OK)
            {
                var res = await DependencyService.Get<IPaymentDropIn>().ShowPay(token.Token, booking.Id.Value);
                if (res)
                {
                    Start();
                }
            }
        }

        void OnAddTime(object sender, EventArgs e)
        {
            var res = new BookingsHelper().ExtendBooking(booking.Id.Value, container.AdditionalTime);
            if (res.Status == System.Net.HttpStatusCode.OK)
            {
                Start();
            }
        }

        void OnOpenMap(object sender, EventArgs e)
        {
            if (coworking.Lat != null && coworking.Lng != null)
            {
                DependencyService.Get<IApplicationOpener>().OpenMapApp(coworking.Lat.Value, coworking.Lng.Value);
            }
            else if (coworking.Address != null)
            {
                DependencyService.Get<IApplicationOpener>().OpenMapApp(coworking.Address);
            }
        }
    }
}