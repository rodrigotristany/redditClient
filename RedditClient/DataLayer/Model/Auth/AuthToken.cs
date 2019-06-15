using System;
using Newtonsoft.Json;

namespace RedditClient.Model.Auth
{
    public class AuthToken
    {
        private string accesToken;

		[JsonProperty("access_token")]
		public string AccessToken 
        { 
            get
            {
                return accesToken;
            }
            set
            {
                accesToken = value;
                DateTimeRequested = DateTime.Now.ToLocalTime();
            }
        }

		[JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        public DateTime DateTimeRequested { get; set; }
    }
}
