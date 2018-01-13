using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.API
{
    class BookingList : APIResponse
    {
        public List <Booking> Bookings { get; set; }
    }
}
