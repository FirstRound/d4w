using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Helpers.Other
{
    class IconGetter
    {

        private static Dictionary<string, string> amentyIcons = new Dictionary<string, string>
        {
            {"coffee", "CoffeeIcon.png" },
            {"parking", "ParkingIcon.png" },
            {"free_parking", "FreeParkingIcon.png" },
            {"printing", "PrintingIcon.png" },
            {"free_printing", "FreePrintingIcon.png" },
            {"food", "FoodIcon.png" }
        };

        public static String GetAmentyIcon(string amenty)
        {
            if (amentyIcons.ContainsKey(amenty))
            {
                return amentyIcons[amenty];
            }
            return null;
        }
    }
}
