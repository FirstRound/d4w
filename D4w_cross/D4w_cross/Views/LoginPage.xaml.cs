using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using D4w_cross.Helpers;
using D4w_cross.Helpers.API;

namespace D4w_cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        public async void OnLogin(object sender, EventArgs e)
        {
            if (Email.Text == null || Password.Text == null)
            {
                await DisplayAlert("Error!", "Please, enter email and password!", "Ok");
                return;
            }

            var res = new UsersHelper().Login(Email.Text, Password.Text);
            if (res.Status == System.Net.HttpStatusCode.OK)
            {
                DBHelper.Instance.SetToken(res);
                App.Current.MainPage = new MainPage();
            }
            else
            {
                await DisplayAlert("Error!", "Wrong email or password!", "Ok");
            }
        }

        public void OnSignUp(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        public void OnContinue(object sender, EventArgs e)
        {
            DBHelper.Instance.DeleteToken();
            App.Current.MainPage = new MainPage();
        }

        public void OnForgot(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }
    }
}