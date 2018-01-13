using D4w_cross.Models.API;
using D4w_cross.Models.API.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace D4w_cross.Helpers.API
{
    class CoworkingsHelper : APIHelper
    {
        const String GET = "coworkings/get/";
        const String GET_ALL = "coworkings/get_all";
        const String GET_FREE_SEATS = "coworkings/get_free_seats/";

        public Coworking Get(int id)
        {
            var obj = new Coworking();
            try
            {
                var result = SendGet(GET + id.ToString(), new List<Tuple<string, string>>());
                obj = JsonConvert.DeserializeObject<Coworking>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public CoworkingList GetAll(CoworkingSearchOptions options)
        {
            var obj = new CoworkingList();
            try
            {
                var result = SendGet(GET_ALL, options.FormOptions());
                obj.Coworkings = JsonConvert.DeserializeObject<List<Coworking>>(result);
                obj.Status = HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                FormErrorResponse(ex, obj);
            }
            return obj;
        }

        public FreeSeats GetAvailableSeats(int coworkingId, DateTime beginDate, DateTime endDate)
        {
            var obj = new FreeSeats();
            try
            {
                var options = new List<Tuple<String, String>>
                {
                    new Tuple<String, String>("begin_date", beginDate.ToString("yyyy-MM-dd HH:mm")),
                    new Tuple<String, String>("end_date", endDate.ToString("yyyy-MM-dd HH:mm"))
                };

                var result = SendGet(GET_FREE_SEATS + coworkingId.ToString(), options);
                obj = JsonConvert.DeserializeObject<FreeSeats>(result);
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
