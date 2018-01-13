using D4w_cross.Helpers;
using D4w_cross.Helpers.API;
using D4w_cross.Models.API;
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
	public partial class SignUpPage : ContentPage
	{

        private Booking booking;

		public SignUpPage ()
		{
			InitializeComponent ();
		}

        public SignUpPage(Booking booking)
        {
            InitializeComponent();
            this.booking = booking;
        }

        public async void OnSignUp(object sender, EventArgs e)
        {
            var user = new User();
            user.Email = Email.Text;
            user.Password = Password.Text;
            user.FirstName = FirstName.Text;
            user.LastName = LastName.Text;
            user.Phone = Phone.Text;

            user = new UsersHelper().Create(user);
            if (user.Status == System.Net.HttpStatusCode.OK)
            {
                var token = new UsersHelper().Login(Email.Text, Password.Text);
                DBHelper.Instance.SetToken(token);
                if (booking != null)
                {
                    booking = new BookingsHelper().Create(booking);
                    if (booking.Status == System.Net.HttpStatusCode.OK)
                    {
                        //switch tab
                        var tabs = (MainPage)App.Current.MainPage;
                        tabs.CurrentPage = tabs.Children[1];
                        var bookingPage = (BookingPage)tabs.CurrentPage;
                        bookingPage.Start();
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Unknown error!", "Ok");

                        //await DisplayAlert(Application.Current.Resources["error"].ToString(), "KEK", "Ok");
                    }
                }
                else
                {
                    App.Current.MainPage = new MainPage();
                }
            }
            else
            {
                //var error = user.Errors.First();
                //var message = ErrorHelper.Translate(error.Key) + ErrorHelper.Translate(error.Value[0]);
                await DisplayAlert("Error!", "Email already taken or does not exists!", "Ok");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (booking != null)
            {
                App.Current.MainPage = new MainPage();
                return true;
            }
                
            return base.OnBackButtonPressed();
        }
    }
}