using D4w_cross.Helpers;
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
	public partial class LoadingPage : ContentPage
	{

        private LocalStorage localStorage;

        public LoadingPage ()
		{
			InitializeComponent ();
            localStorage = new LocalStorage();
            Task.Run(async () =>
            {
                UpdateCoworkingList();
            });
            
        }

        private void UpdateCoworkingList()
        {

            GlobalObjectsHelper.CwSearchOptions.CorrectDates(new TimeSpan(00, 00, 00), new TimeSpan(23, 59, 59));
            GlobalObjectsHelper.UpdateCoworkings();
            
            //load pictures
            for (int i = 0; i < GlobalObjectsHelper.Coworkings.Count; i++)
            {
                if (GlobalObjectsHelper.Coworkings[i].Images.Count > 0)
                {
                    var i1 = i;
                    Task.Run(async () =>
                    {
                        LoadImage(i1, GlobalObjectsHelper.Coworkings[i1].Images[0].Id);
                    });
                    /*new Thread(() =>
                    {
                        LoadImage(i1, GlobalObjectsHelper.Coworkings[i1].Images[0].Id);
                    }).Start();*/
                }
            }
            Navigation.PopAsync();
        }

        private void LoadImage(int index, int id)
        {
            var image = localStorage.GetImage(id);
            GlobalObjectsHelper.Coworkings[index].UpdateDisplayImage(image);
        }
    }
}