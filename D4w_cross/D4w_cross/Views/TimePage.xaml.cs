using D4w_cross.Helpers;
using D4w_cross.Models.API.Search;
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
	public partial class TimePage : ContentPage
	{

        private CoworkingSearchOptions options;

        public TimePage ()
		{
			InitializeComponent ();
            Start.Time = GlobalObjectsHelper.CwSearchOptions.BeginWork.GetValueOrDefault();
            Finish.Time = GlobalObjectsHelper.CwSearchOptions.EndWork.GetValueOrDefault();
        }

        public void OnNext(object sender, EventArgs e)
        {
            GlobalObjectsHelper.CwSearchOptions.BeginWork = Start.Time;
            GlobalObjectsHelper.CwSearchOptions.EndWork = Finish.Time;
            Navigation.PopAsync();
        }
    }
}