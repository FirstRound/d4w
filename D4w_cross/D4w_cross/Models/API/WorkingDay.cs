using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.API
{
    class WorkingDay
    {
        [JsonProperty(PropertyName = "day")]
        public String Day { get; set; }

        [JsonProperty(PropertyName = "begin_work")]
        public TimeSpan? BeginWork { get; set; }

        [JsonProperty(PropertyName = "end_work")]
        public TimeSpan? EndWork { get; set; }
    }
}
