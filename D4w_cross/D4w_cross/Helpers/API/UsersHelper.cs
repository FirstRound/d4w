using D4w_cross.Models.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace D4w_cross.Helpers.API
{
    class UsersHelper : APIHelper
    {
        private const String LOGIN = "auth/login";
        private const String CREATE = "users/create";
        private const String GET_ME = "users/get_me";
        private const String UPDATE_ME = "users/update_me";
        private const String FORGOT_PASSWORD = "users/forgot_password";
        private const String GET_FUTURE_BOOKING = "users/get_future_booking";
        private const String GET_CURRENT_BOOKING = "users/get_current_booking";
        private const String GET_MY_BOOKINGS = "users/get_my_bookings";

        public Token Login(String email, String password)
        {
            var obj = new Token();
            try
            {
                var param = new Dictionary<String, String> {
                        { "email", email },
                        { "password", password }
                };
                var result = SendPost(LOGIN, JsonConvert.SerializeObject(param));
                obj = JsonConvert.DeserializeObject<Token>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }


        public User Create(User user)
        {
            var obj = new User();
            try
            {
                var result = SendPost(CREATE, JsonConvert.SerializeObject(user));
                obj = JsonConvert.DeserializeObject<User>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
               FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public User GetMe()
        {
            var obj = new User();
            try
            {
                var result = SendGet(GET_ME, new List<Tuple<string, string>>());
                obj = JsonConvert.DeserializeObject<User>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public User UpdateMe(User user)
        {
            var obj = new User();
            try
            {
                var result = SendPut(UPDATE_ME, JsonConvert.SerializeObject(user));
                obj = JsonConvert.DeserializeObject<User>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public APIResponse ForgotPassword(String email)
        {
            var obj = new APIResponse();
            try
            {
                var param = new Dictionary<String, String>
                {
                    {"email", email }
                };
                var result = SendPost(FORGOT_PASSWORD, JsonConvert.SerializeObject(param));
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public Booking GetCurrentBooking(DateTime date)
        {
            var obj = new Booking();
            try
            {
                var result = SendGet(GET_CURRENT_BOOKING, new List<Tuple<string, string>> {
                    new Tuple<string, string>("date", date.ToString("yyyy-MM-dd HH:mm"))
                });
                obj = JsonConvert.DeserializeObject<Booking>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public Booking GetFutureBooking(DateTime date)
        {
            var obj = new Booking();
            try
            {
                var result = SendGet(GET_FUTURE_BOOKING, new List<Tuple<string, string>> {
                    new Tuple<string, string>("date", date.ToString("yyyy-MM-dd HH:mm"))
                });
                obj = JsonConvert.DeserializeObject<Booking>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public BookingList GetMyBookings()
        {
            var obj = new BookingList();
            try
            {
                var result = SendGet(GET_MY_BOOKINGS, new List<Tuple<string, string>>());
                obj.Bookings = JsonConvert.DeserializeObject<List<Booking>>(result);
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
