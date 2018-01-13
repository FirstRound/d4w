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
    public partial class PersonalInfoPage : ContentPage
    {
        private User user;
        private User upd;
        private LocalStorage localStorage;

        public PersonalInfoPage()
        {
            InitializeComponent();
            localStorage = new LocalStorage();

            if (DBHelper.Instance.GetToken() != null)
            {
                user = new UsersHelper().GetMe();
                upd = new User();
                Email.Text = user.Email;
                FirstName.Text = user.FirstName;
                LastName.Text = user.LastName;
                Phone.Text = user.Phone;

                if (user.ImageId != null)
                {
                    var image = localStorage.GetImage(user.ImageId.Value);
                    try
                    {
                        var formsImage = Xamarin.Forms.ImageSource.FromStream(
                                  () => { return new MemoryStream(Convert.FromBase64String(image.Base64)); });
                        UserImage.Source = formsImage;
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

        }

        public async void OnSelectImage(object sender, EventArgs e)
        {
            Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                upd.Image = Convert.ToBase64String(bytes);

                stream.Seek(0, SeekOrigin.Begin);
                UserImage.Source = ImageSource.FromStream(() => stream);
            }
        }

        public async void OnSave(object sender, EventArgs e)
        {
            if (Email.Text != user.Email)
                upd.Email = Email.Text;
            if (OldPassword.Text != null)
                upd.OldPassword = OldPassword.Text;
            if (NewPassword.Text != null)
                upd.Password = NewPassword.Text;
            if (FirstName.Text != user.FirstName)
                upd.FirstName = FirstName.Text;
            if (LastName.Text != user.LastName)
                upd.LastName = LastName.Text;
            if (Phone.Text != user.Phone)
                upd.Phone = Phone.Text;

            user = new UsersHelper().UpdateMe(upd);
            if (user.Status == System.Net.HttpStatusCode.OK)
            {
            }
            else
            {
                await DisplayAlert("Error!", "Something goes wrong", "Ok");
            }
            Navigation.PopAsync();
        }
    }
}