using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using D4w_cross.Dependencies;
using Xamarin.Forms;
using D4w_cross.iOS.Dependencies;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ActionBarChangerImplementation))]
namespace D4w_cross.iOS.Dependencies
{
    class ActionBarChangerImplementation : IActionBarChanger
    {
        public event EventHandler Clicked;

        public void InitClick(EventHandler func)
        {
            var activity = App.Current.MainPage;
            var toolbarTitle = activity.FindByName<Page>("NavPage");

            if (toolbarTitle != null)
            {
                Clicked -= func;
                Clicked += func;

                EventHandler del = delegate (object sender, EventArgs e)
                {
                    Clicked.Invoke(sender, new EventArgs());
                };

                //toolbarTitle.Click -= del;
                //toolbarTitle.Click += del;
            }


        }

        public void ChangeTitle(string title)
        {
            var s = App.Current.MainPage;
            s.Title = "Some";
        }
    }
}