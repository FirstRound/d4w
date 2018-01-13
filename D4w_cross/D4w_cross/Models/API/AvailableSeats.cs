using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.API
{
    class FreeSeats : APIResponse
    {
        [JsonProperty(PropertyName = "free_seats")]
        public int? SeatsNum { get; set; }
    }
}
