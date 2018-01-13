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
	public partial class ForgotConfirmPage : ContentPage
	{
		public ForgotConfirmPage ()
		{
			InitializeComponent ();
		}

        public void OnContinue(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            Navigation.PopAsync();
        }
    }
}