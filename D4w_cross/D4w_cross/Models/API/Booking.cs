using D4w_cross.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace D4w_cross.Models.API
{
    
    public class Booking : APIResponse, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey]
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty(PropertyName = "begin_date")]
        public DateTime? BeginDate { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty(PropertyName = "end_date")]
        public DateTime? EndDate { get; set; }

        [JsonProperty(PropertyName = "coworking_id")]
        public int CoworkingId { get; set; }

        [JsonProperty(PropertyName = "coworking_name")]
        public String CoworkingName { get; set; }

        [JsonProperty(PropertyName = "visitors_count")]
        public int? VisitorsCount { get; set; }
        
        [JsonProperty(PropertyName = "is_visit_confirmed")]
        public bool? IsVisitConfirmed { get; set; }

        private DateTime? _visitTime;
        [JsonProperty(PropertyName = "visit_time")]
        public DateTime? VisitTime
        {
            get
            {
                return _visitTime;
            }
            set
            {
                _visitTime = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("VisitTime"));
            }
        }

        [JsonProperty(PropertyName = "leave_time")]
        public DateTime? LeaveTime { get; set; }

        [JsonProperty(PropertyName = "price")]
        public String Price { get; set; }

        [JsonProperty(PropertyName = "is_closed")]
        public bool? IsClosed { get; set; }

        [JsonProperty(PropertyName = "is_user_leaving")]
        public bool? IsUserLeaving { get; set; }

        [JsonProperty(PropertyName = "is_user_canceling")]
        public bool? IsUserCanceling { get; set; }

        [JsonProperty(PropertyName = "is_extend_pending")]
        public bool? IsExtendPending { get; set; }

        [JsonProperty(PropertyName = "is_payed")]
        public bool? IsPayed { get; set; }

        public void CorrectDates(TimeSpan beginTime, TimeSpan endTime)
        {
            if (BeginDate != null)
            {
                BeginDate = new DateTime(BeginDate.Value.Year,
                                            BeginDate.Value.Month,
                                            BeginDate.Value.Day,
                                            beginTime.Hours,
                                            beginTime.Minutes,
                                            beginTime.Seconds);
            }

            if (EndDate != null)
            {
                EndDate = new DateTime(EndDate.Value.Year,
                                        EndDate.Value.Month,
                                        EndDate.Value.Day,
                                        endTime.Hours,
                                        endTime.Minutes,
                                        endTime.Seconds);
            }
        }
    }
}
