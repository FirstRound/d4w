using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using TabbedPageDemo.iOS;
using D4w_cross;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace TabbedPageDemo.iOS
{
    public class ExtendedTabbedPageRenderer : TabbedRenderer
    {

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            foreach (var item in TabBar.Items)
            {
                item.Image = GetTabIcon(item.Title);
            }
        }

        private UIImage GetTabIcon(string title)
        {
            UITabBarItem item = null;

            switch (title)
            {
                case "PROFILE":
                    item = new UITabBarItem(UITabBarSystemItem.Contacts, 0);
                    break;
                case "HOLD ON":
                    item = new UITabBarItem(UITabBarSystemItem.Favorites, 0);
                    break;
                case "FIND":
                    item = new UITabBarItem(UITabBarSystemItem.Search, 0);
                    break;
            }

            var img = (item != null) ? UIImage.FromImage(item.SelectedImage.CGImage, item.SelectedImage.CurrentScale, item.SelectedImage.Orientation) : new UIImage();
            return img;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            // Set Text Font for unselected tab states
            UITextAttributes normalTextAttributes = new UITextAttributes();
            var font = UIFont.SystemFontOfSize(12);
            normalTextAttributes.Font = font; // unselected
            normalTextAttributes.TextColor = UIColor.LightGray;
            UITabBarItem.Appearance.SetTitleTextAttributes(normalTextAttributes, UIControlState.Normal);
        }

        public override UIViewController SelectedViewController
        {
            get
            {
                UITextAttributes selectedTextAttributes = new UITextAttributes();
                var font = UIFont.SystemFontOfSize(12);
                selectedTextAttributes.Font = font; // SELECTED
                selectedTextAttributes.TextColor = UIColor.Orange;


                if (base.SelectedViewController != null)
                {
                    base.SelectedViewController.TabBarItem.SetTitleTextAttributes(selectedTextAttributes, UIControlState.Normal);

                }
                return base.SelectedViewController;
            }
            set
            {
                base.SelectedViewController = value;
                foreach (UIViewController viewController in base.ViewControllers)
                {
                    UITextAttributes normalTextAttributes = new UITextAttributes();
                    var font = UIFont.SystemFontOfSize(12);
                    normalTextAttributes.Font = font; // unselected
                    normalTextAttributes.TextColor = UIColor.LightGray;
                    viewController.TabBarItem.SetTitleTextAttributes(normalTextAttributes, UIControlState.Normal);
                }
            }
        }
    }
}