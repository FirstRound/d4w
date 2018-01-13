using D4w_cross.Helpers.API;
using D4w_cross.Models.Containers;
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
	public partial class HistoryPage : ContentPage
	{
		public HistoryPage ()
		{
			InitializeComponent ();

            var bookings = new UsersHelper().GetMyBookings().Bookings.Where(b => b.Price != null).ToList();
            Bookings.ItemsSource = bookings;
            /*
            var list = new List<HistoryItemContainer>();
            foreach (var booking in bookings)
            {
                if (booking.Price != null)
                {
                    list.Add(new HistoryItemContainer
                    {
                        BeginDate = booking.VisitTime.Value,
                        EndDate = booking.FinishTime.Value,
                        Price = booking.Price,
                        CoworkingName = booking.CoworkingName
                    });
                }
            }*/
		}
	}
}