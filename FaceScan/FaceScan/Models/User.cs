using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace FaceScan.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Domain")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "First")]
        public string First { get; set; }

        [JsonProperty(PropertyName = "Last")]
        public string Last { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "UPN")]
        public string UPN { get; set; }

        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "UserPhoto")]
        public string UserPhoto { get; set; }

        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "PersonId")]
        public string PersonId { get; set; }

    }

}

