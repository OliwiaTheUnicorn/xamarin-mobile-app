using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CrmMobileApp.Models
{
    public class Lead
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty("location_cooridinates")]
        public string LocationCooridinates { get; set; }
    }
}
