using D4w_cross.Helpers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D4w_cross.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPasswordPage : ContentPage
	{
		public ForgotPasswordPage ()
		{
			InitializeComponent ();
		}

        public async void OnSend(object sender, EventArgs e)
        {
            var res = new UsersHelper().ForgotPassword(Email.Text);
            if (res.Status == HttpStatusCode.OK)
            {
                Navigation.PushAsync(new ForgotConfirmPage());
            }
            else
            {
                await DisplayAlert("Error", "User with this email doesn't exists!", "Ok");
            }
        }
    }
}
