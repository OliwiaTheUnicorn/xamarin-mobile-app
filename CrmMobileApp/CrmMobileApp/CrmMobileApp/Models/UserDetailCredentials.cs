using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrmMobileApp.Models
{
    public class UserDetailCredentials
    {
        [JsonProperty("user")]
        public User user { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
