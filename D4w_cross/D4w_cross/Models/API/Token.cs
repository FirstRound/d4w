using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace D4w_cross.Models.API
{
    class Token : APIResponse
    {
        public Token() { }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "token")]
        public String TokenStr { get; set; }
    }    

}
