using D4w_cross.Models.API;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace D4w_cross.Models.Views
{
    class CoworkingPin : Pin
    {
        public int CoworkingId { get; set; }
        public Image Cover { get; set; }
        public String Distance { get; set; }
        public String Time { get; set; }
        public List <String> Amenties { get; set; }
    }
}
