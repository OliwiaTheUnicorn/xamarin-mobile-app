using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrmMobileApp.Models
{
    public class ReminderStatus
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
