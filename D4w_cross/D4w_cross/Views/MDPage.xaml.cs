using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D4w_cross.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using D4w_cross.Dependencies;

namespace D4w_cross.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MDPage : MasterDetailPage
    {
        public MDPage()
        {
            InitializeComponent();
            IsGestureEnabled = false;
            Master.IsVisible = false;
            Master.Title = "";
        }  
    }
}