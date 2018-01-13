using D4w_cross.Helpers.API;
using D4w_cross.Models.API;
using D4w_cross.Models.API.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace D4w_cross.Helpers
{
    class GlobalObjectsHelper
    {
        public static CoworkingSearchOptions CwSearchOptions { get; set; } = new CoworkingSearchOptions();

        public static ObservableCollection<Coworking> Coworkings { get; set; }
        public static ObservableCollection<Coworking> AllCoworkings { get; set; }

        public static Coworking SelectedCoworking { get; set; }

        public static ObservableCollection<ImageSource> CarouselImages { get; set; } = new ObservableCollection<ImageSource>();

        public static void UpdateCoworkings()
        {
            Coworkings = new LocalStorage().GetAllCoworkings(CwSearchOptions);
        }
    }
}
