using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace D4w_cross.Models.API
{
    class Amenty : APIResponse
    {

        private String _name;

        public String RawName { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name
        {
            set
            {
                RawName = value;
                _name = Application.Current.Resources[value].ToString();
            }
            get { return _name; }
        }

        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }
    }
}
