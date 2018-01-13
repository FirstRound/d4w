using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace D4w_cross
{
	public partial class MainPage : TabbedPage
    {
		public MainPage()
		{
			InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            MessagingCenter.Send(this, "CurrentPageChanged");
        }
    }
}
