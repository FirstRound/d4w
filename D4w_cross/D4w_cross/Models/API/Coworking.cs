using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using D4w_cross.Helpers;

namespace D4w_cross.Models.API
{
    class Coworking : APIResponse, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public String FullName { get; set; }

        [JsonProperty(PropertyName = "short_name")]
        public String ShortName { get; set; }

        [JsonProperty(PropertyName = "address")]
        public String Address { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double? Lat { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public double? Lng { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double? Distance { get; set; }

        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }

        [JsonProperty(PropertyName = "contacts")]
        public String Contacts { get; set; }

        [JsonProperty(PropertyName = "additional_info")]
        public String AdditionalInfo { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double? Price { get; set; }
        
        [JsonProperty(PropertyName = "capacity")]
        public int? Capacity { get; set; }
        
        [JsonProperty(PropertyName = "creator_id")]
        public String CreatorId { get; set; }
        
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "working_days")]
        public List <WorkingDay>  WorkingDays { get; set; }

        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; set; }

        [JsonProperty(PropertyName = "amenties")]
        public List<Amenty> Amenties { get; set; }

        [Ignore, JsonIgnore]
        public Xamarin.Forms.ImageSource DisplayImage { get; set;  }

        public void UpdateDisplayImage(Image image)
        {
            try
            {
                var regex = new Regex("data:image.*base64,");
                var base64 = regex.Replace(image.Base64, String.Empty);
                var formsImage = Xamarin.Forms.ImageSource.FromStream(
                               () => { return new MemoryStream(Convert.FromBase64String(base64)); });
                DisplayImage = formsImage;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayImage"));
            } catch (Exception e)
            {

            }
        }

        public TimeSpan? CurrentBeginWork
        {
            get {
                var filtered = DateTime.Now;
                if (GlobalObjectsHelper.CwSearchOptions.BeginDate != null)
                    filtered = GlobalObjectsHelper.CwSearchOptions.BeginDate.Value;
                var beg = WorkingDays.Where(wd => wd.Day == filtered.DayOfWeek.ToString()).FirstOrDefault();
                if (beg != null)
                    return beg.BeginWork;
                return null;
            }
        }

        public TimeSpan? CurrentEndWork
        {
            get
            {
                var filtered = DateTime.Now;
                if (GlobalObjectsHelper.CwSearchOptions.EndDate != null)
                    filtered = GlobalObjectsHelper.CwSearchOptions.EndDate.Value;
                var beg = WorkingDays.Where(wd => wd.Day == filtered.DayOfWeek.ToString()).FirstOrDefault();
                if (beg != null)
                    return beg.EndWork;
                return null;
            }
        }

        public string FormattedTime
        {
            get
            {
                var beg = CurrentBeginWork;
                var end = CurrentEndWork;
                if (beg != null && end != null)
                {
                    return beg.Value.ToString(@"hh\:mm") + " - " + end.Value.ToString(@"hh\:mm");
                }
                return "Not available";
            }
        }

        public string FormattedDistance
        {
            get
            {
                if (Distance != null)
                {
                    return ((int)(Distance.Value)).ToString() + " km";
                }
                return "";
            }
        }



        public void UpdateDisplayImage(Xamarin.Forms.ImageSource image)
        {
            try
            {          
                DisplayImage = image;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayImage"));
            }
            catch (Exception e)
            {

            }
        }

        [Ignore, JsonIgnore]
        public int? SeatsAvailable { get; set; }

        public void UpdateSeatsAvailable(int num)
        {
            SeatsAvailable = num;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("SeatsAvailable"));
        }


    }
}
