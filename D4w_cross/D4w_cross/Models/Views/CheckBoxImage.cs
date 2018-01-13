using D4w_cross.Models.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace D4w_cross.Models.Views
{
    class CheckBoxImage : Xamarin.Forms.Image
    {
        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked; 
            }
            set
            {
                isChecked = value;
                Update();
            }
        }
        public string Value { get; set; }
        public event EventHandler Clicked;

        public CheckBoxImage()
        {
            var imageTapGesture = new TapGestureRecognizer();
            imageTapGesture.Tapped += ImageTapGestureOnTapped;
            GestureRecognizers.Add(imageTapGesture);
            Source = "Unchecked.png";

        }

        private void Update()
        {
            if (IsChecked)
            {
                Source = "Checked.png";
            }
            else
            {
                Source = "Unchecked.png";
            }
        }

        private void ImageTapGestureOnTapped(object sender, EventArgs eventArgs)
        {
            if (IsEnabled)
            {
                IsChecked = !IsChecked;
            }
            Update();
            if (Clicked != null)
                Clicked.Invoke(sender, new CheckBoxEventArgs { CheckBox = this });
        }
    }
}
