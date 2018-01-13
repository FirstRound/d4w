using D4w_cross.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace D4w_cross.Models.API.Search
{
    public class CoworkingSearchOptions : SearchOptions
    {
       
        public CoworkingSearchOptions()
        {
           
        }

        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public String Address { get; set; }  
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? BeginWork { get; set; }
        public TimeSpan? EndWork { get; set; }
        public int? FreeCount { get; set; }
        //[Ignore]
        //public List<String> Amenties { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public bool Coffee { get; set; }
        public bool Printing { get; set; }
        public bool FreePrinting { get; set; }
        public bool Parking { get; set; }
        public bool FreeParking { get; set; }
        public bool Food { get; set; }
        public int? Radius { get; set; }

        private void AddHelper(ref List<Tuple<String, String>> res, String key, Object value)
        {
            if (value != null)
                res.Add(new Tuple<String, String>(key, value.ToString()));
        }

        public void CorrectDates(TimeSpan beginTime, TimeSpan endTime)
        {
            var beginWork = BeginWork;
            if (beginWork == null)
            {
                beginWork = beginTime;
            }
            var endWork = EndWork;
            if (endWork == null)
            {
                endWork = endTime;
            }
            if (BeginDate != null)
            {
                BeginDate = new DateTime(BeginDate.Value.Year,
                                            BeginDate.Value.Month,
                                            BeginDate.Value.Day,
                                            beginWork.Value.Hours,
                                            beginWork.Value.Minutes,
                                            beginWork.Value.Seconds);
            }

            if (EndDate != null)
            {
                EndDate = new DateTime(EndDate.Value.Year,
                                        EndDate.Value.Month,
                                        EndDate.Value.Day,
                                        endWork.Value.Hours,
                                        endWork.Value.Minutes,
                                        endWork.Value.Seconds);
            }
        }

        public void Update()
        {
            var time = DateTime.Now;//new DateTime(2017, 12, 12, 09, 30, 00);
            BeginDate = time;
            EndDate = time;
            BeginWork = new TimeSpan(time.TimeOfDay.Hours + 1, 0, 0);
            EndWork = new TimeSpan((time.TimeOfDay.Hours + 4) % 24, 0, 0);
            if (time.TimeOfDay.Hours + 4 >= 24 && time.TimeOfDay.Hours + 1 < 24)
            {
                EndWork = new TimeSpan(23, 59, 59);
            }
            Limit = null;
            Offset = null;
        }

        public List<Tuple<String, String>> FormOptions()
        {
            var res = new List<Tuple<String, String>>();
            if (Lat != null)
                AddHelper(ref res, "lat", Lat.Value.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));
            if (Lng != null)
                AddHelper(ref res, "lng", Lng.Value.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));
            AddHelper(ref res, "address", Address);
            if (BeginDate != null)
                res.Add(new Tuple<String, String>("begin_date", BeginDate.Value.ToString("yyyy-MM-dd HH:mm")));
            if (EndDate != null)
                res.Add(new Tuple<String, String>("end_date", EndDate.Value.ToString("yyyy-MM-dd HH:mm")));
            AddHelper(ref res, "begin_work", BeginWork);
            AddHelper(ref res, "end_work", EndWork);
            AddHelper(ref res, "free_count", FreeCount);

            var amenties = new List<string>();
            if (Coffee)
                amenties.Add("coffee");
            if (Printing)
                amenties.Add("printing");
            if (FreePrinting)
                amenties.Add("free_printing");
            if (Parking)
                amenties.Add("parking");
            if (FreeParking)
                amenties.Add("free_parking");
            if (Food)
                amenties.Add("food");

            if (amenties.Count > 0)
            {
                if (amenties.Count == 1)
                {
                    res.Add(new Tuple<String, String>("amenties[]", amenties[0]));
                }
                else
                {
                    res.Add(new Tuple<String, String>("amenties[]", String.Join("&amenties[]=", amenties)));
                }
            }

            AddHelper(ref res, "limit", Limit);
            AddHelper(ref res, "offset", Offset);

            AddHelper(ref res, "radius", "50");

            return res;
        }
    }
}
