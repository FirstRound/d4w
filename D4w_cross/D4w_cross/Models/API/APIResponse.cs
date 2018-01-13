using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace D4w_cross.Models.API
{
    public class APIResponse
    {
        [Ignore, JsonIgnore]
        public HttpStatusCode Status { get; set; }

        [Ignore, JsonIgnore]
        public Dictionary<String, List <String> > Errors { get; set; }
    }
}
