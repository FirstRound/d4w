using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Helpers
{
    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm";
        }
    }
}
