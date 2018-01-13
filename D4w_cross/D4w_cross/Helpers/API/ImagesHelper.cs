using D4w_cross.Models.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace D4w_cross.Helpers.API
{
    class ImagesHelper : APIHelper
    {

        private const String GET = "images/get/";

        public Image Find(int id)
        {
            var obj = new Image();
            try
            {
                var result = SendGet(GET + id.ToString(), new List<Tuple<String, String>>());
                obj = JsonConvert.DeserializeObject<Image>(result);
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
