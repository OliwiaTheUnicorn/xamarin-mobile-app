using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CrmMobileApp.Models
{
    public class Contract
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("contract_value")]
        public string ContractValue { get; set; }
        [JsonProperty("lead_id")]
        public string LeadId { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}
