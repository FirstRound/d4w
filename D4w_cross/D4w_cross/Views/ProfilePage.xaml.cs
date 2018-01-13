using D4w_cross.Dependencies;
using D4w_cross.Helpers;
using D4w_cross.Helpers.API;
using D4w_cross.Models.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D4w_cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {

        private User user;
        private LocalStorage localStorage;
        public ProfilePage()
        {
            InitializeComponent();
            localStorage = new LocalStorage();
        }

        protected override void OnAppearing()
        {
            if (DBHelper.Instance.GetToken() != null)
            {
                user = new UsersHelper().GetMe();
                UserName.Text = user.FirstName + " " + user.LastName;
                if (user.ImageId != null)
                {
                    var image = localStorage.GetImage(user.ImageId.Value);
                    try
                    {
                        var formsImage = Xamarin.Forms.ImageSource.FromStream(
                                  () => { return new MemoryStream(Convert.FromBase64String(image.Base64)); });
                        UserImage.Source = formsImage;
                    }catch (Exception e)
                    {

                    }
                }
            }
            BindingContext = user;
        }

        public void OnPersonalInfo(object sender, EventArgs e)
        {
            if (DBHelper.Instance.GetToken() != null)
            {
                Navigation.PushAsync(new PersonalInfoPage());
            }
           
        }

        public void OnHistory(object sender, EventArgs e)
        {
            if (DBHelper.Instance.GetToken() != null)
            {
                Navigation.PushAsync(new HistoryPage());
            }

        }

        public void OnPayment(object sender, EventArgs e)
        {
            if (DBHelper.Instance.GetToken() != null)
            {
                var token = new PaymentsHelper().GetClientToken();
                if (token.Status == System.Net.HttpStatusCode.OK)
                {
                    DependencyService.Get<IPaymentDropIn>().ShowInit(token.Token);
                }
            }

        }


        public void OnLogout(object sender, EventArgs e)
        {
            DBHelper.Instance.DeleteToken();
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}