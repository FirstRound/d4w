using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.API
{
    class ClientToken : APIResponse
    {
        [JsonProperty(PropertyName = "client_token")]
        public String Token { get; set; }

    }
}
