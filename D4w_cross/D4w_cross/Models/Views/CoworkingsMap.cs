using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace D4w_cross.Models.Views
{
    class CoworkingsMap : Map
    {
        public event EventHandler PinsUpdated;
        public EventHandler CoworkingClicked;

        public void UpdatePins(object sender, EventArgs e)
        {
            if (PinsUpdated != null)
                PinsUpdated.Invoke(sender, e);
        }

        public void OnCoworking(object sender, EventArgs e)
        {
            CoworkingClicked.Invoke(sender, e);
        }
    }
}
