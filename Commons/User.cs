using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Commons
{
    public class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("Roles")]
        public IList<string> Roles { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        public string Fullname
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string Password { get; set; }
    }
}