using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using D4w_cross.Models.API.Search;
using D4w_cross.Helpers;

#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using Android.Widget;
#endif

namespace D4w_cross.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DatePage : ContentPage
	{

        public DatePage ()
		{
            InitializeComponent();
            Date.MinimumDate = DateTime.Now;
            Date.Date = GlobalObjectsHelper.CwSearchOptions.BeginDate.GetValueOrDefault();
        }

        public void OnNext(object sender, EventArgs e)
        {
            GlobalObjectsHelper.CwSearchOptions.BeginDate = Date.Date;
            GlobalObjectsHelper.CwSearchOptions.EndDate = Date.Date;
            Navigation.PopAsync();
        }
    }
}