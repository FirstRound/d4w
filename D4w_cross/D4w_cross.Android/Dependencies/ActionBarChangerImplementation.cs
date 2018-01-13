using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using D4w_cross.Dependencies;
using Xamarin.Forms;
using D4w_cross.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency(typeof(ActionBarChangerImplementation))]
namespace D4w_cross.Droid.Dependencies
{
    class ActionBarChangerImplementation : IActionBarChanger
    {
        public event EventHandler Clicked;

        public void InitClick(EventHandler func)
        {
            var activity = Forms.Context as Activity;
            var toolbarTitle = activity.FindViewById<TextView>(Resource.Id.toolbar_title);

            if (toolbarTitle != null)
            {
                Clicked -= func;
                Clicked += func;

                EventHandler del = delegate (object sender, EventArgs e)
                {
                    Clicked.Invoke(sender, new EventArgs());
                };

                toolbarTitle.Click -= del;
                toolbarTitle.Click += del;
            }
        }

        public void ChangeTitle(string title)
        {
            var activity = Forms.Context as Activity;
            var toolbarTitle = activity.FindViewById<TextView>(Resource.Id.toolbar_title);
            if (toolbarTitle != null)
            {
                toolbarTitle.Text = title;
            }
        }
    }
}