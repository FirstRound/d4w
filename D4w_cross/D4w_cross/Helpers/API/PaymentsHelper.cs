using D4w_cross.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace D4w_cross.Models.API
{
    class PaymentsHelper : APIHelper
    {
        const String GET_CLIENT_TOKEN = "payments/get_client_token";
        const String INIT_PAYMENTS = "payments/init_payments";
        const String PAY_FOR_BOOKING = "payments/pay_for_booking";

        public ClientToken GetClientToken()
        {
            var obj = new ClientToken();
            try
            {
                var result = SendGet(GET_CLIENT_TOKEN, new List<Tuple<string, string>>());
                obj = JsonConvert.DeserializeObject<ClientToken>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public APIResponse InitPayments(string nonce)
        {
            var obj = new APIResponse();
            try
            {
                var param = new Dictionary<String, String>
                {
                    {"nonce", nonce }
                };
                var result = SendPost(INIT_PAYMENTS, JsonConvert.SerializeObject(param));
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;

        }

        public APIResponse PayForBooking(string nonce, int bookingId)
        {
            var obj = new APIResponse();
            try
            {
                var param = new Dictionary<String, String>
                {
                    {"nonce", nonce },
                    {"booking_id", bookingId.ToString() }
                };
                var result = SendPost(PAY_FOR_BOOKING, JsonConvert.SerializeObject(param));
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
