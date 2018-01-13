using D4w_cross.Models.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace D4w_cross.Models.Views
{
    class AmentyImage : Xamarin.Forms.Image
    {
        public string Value { get; set; }
        public event EventHandler Clicked;

        public AmentyImage()
        {
            var imageTapGesture = new TapGestureRecognizer();
            imageTapGesture.Tapped += ImageTapGestureOnTapped;
            GestureRecognizers.Add(imageTapGesture);
        }

        private void ImageTapGestureOnTapped(object sender, EventArgs eventArgs)
        {
            Clicked.Invoke(sender, new AmentyEventArgs { Img = this });
        }
    }
}
