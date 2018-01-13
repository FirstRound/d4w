using D4w_cross.Models.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace D4w_cross.Helpers.API
{
    class BookingsHelper : APIHelper
    {

        private const String CREATE = "bookings/create";
        private const String GET = "bookings/get/";
        private const String CANCEL_BOOKING = "bookings/cancel_booking";
        private const String LEAVE_COWORKING = "bookings/leave_coworking";
        private const String EXTEND_BOOKING = "bookings/extend_booking";

        public Booking Create(Booking booking)
        {
            var obj = new Booking();
            try
            {
                var result = SendPost(CREATE, JsonConvert.SerializeObject(booking));
                obj = JsonConvert.DeserializeObject<Booking>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public Booking Get(int id)
        {
            var obj = new Booking();
            try
            {
                var result = SendGet(GET + id.ToString(), new List<Tuple<string, string>>());
                obj = JsonConvert.DeserializeObject<Booking>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public Booking CancelBooking(int id)
        {
            var obj = new Booking();
            try
            {
                var param = new Dictionary<String, String> {
                        { "booking_id", id.ToString() }
                };
                var result = SendPost(CANCEL_BOOKING, JsonConvert.SerializeObject(param));
                obj = JsonConvert.DeserializeObject<Booking>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public Booking LeaveCoworking(int id)
        {
            var obj = new Booking();
            try
            {
                var param = new Dictionary<String, String> {
                        { "booking_id", id.ToString() }
                };
                var result = SendPost(LEAVE_COWORKING, JsonConvert.SerializeObject(param));
                obj = JsonConvert.DeserializeObject<Booking>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public APIResponse ExtendBooking(int id, DateTime time)
        {
            var obj = new Booking();
            try
            {
                var param = new Dictionary<String, String> {
                        { "booking_id", id.ToString() },
                        { "extend_time", time.ToString("HH:mm") }
                };
                var result = SendPost(EXTEND_BOOKING, JsonConvert.SerializeObject(param));
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }


    }
}
