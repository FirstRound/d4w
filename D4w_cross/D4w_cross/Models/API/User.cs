using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.API
{
    class User : APIResponse
    {
        [PrimaryKey]
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public String Email { get; set; }

        [JsonProperty(PropertyName = "password", NullValueHandling = NullValueHandling.Ignore)]
        public String Password { get; set; }

        [JsonProperty(PropertyName = "old_password", NullValueHandling = NullValueHandling.Ignore)]
        public String OldPassword { get; set; }

        [JsonProperty(PropertyName = "first_name", NullValueHandling = NullValueHandling.Ignore)]
        public String FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name", NullValueHandling = NullValueHandling.Ignore)]
        public String LastName { get; set; }

        [JsonProperty(PropertyName = "address", NullValueHandling = NullValueHandling.Ignore)]
        public String Address { get; set; }

        [JsonProperty(PropertyName = "phone", NullValueHandling = NullValueHandling.Ignore)]
        public String Phone { get; set; }

        [JsonProperty(PropertyName = "image_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ImageId { get; set; }

        [JsonProperty(PropertyName = "image", NullValueHandling = NullValueHandling.Ignore)]
        public String Image { get; set; }

        [JsonProperty(PropertyName = "coworking_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? CoworkingId { get; set; }

        [JsonProperty(PropertyName = "created_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedAt { get; set; }
    }
}
