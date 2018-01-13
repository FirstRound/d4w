using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.API
{
    public class Image : APIResponse
    {
        [PrimaryKey]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "base64")]
        public String Base64 { get; set; }
    }
}
