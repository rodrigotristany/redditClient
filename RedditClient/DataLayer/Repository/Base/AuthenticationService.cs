using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Commons;
using Commons.Cache;
using Commons.Environment;
using RedditClient.DataLayer.Base.Models.Base;
using RedditClient.Model.Auth;

namespace RedditClient.DataLayer.Repository.Base
{
    public class AuthenticationService : UtilHandlersHttpClient
    {
		private HttpClient client;
        
        public ApiResponse<AuthToken> Token { get; set; }

		private static AuthenticationService instance = null;

		public static AuthenticationService Instance
        {
            get
            {
                if (instance == null)
                {
					instance = new AuthenticationService();
                }
                return instance;
            }
        }

		public AuthenticationService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = new TimeSpan(0, 0, 0, 0, Constants.SERVICE_TIMEOUT);
            client.BaseAddress = new Uri(Environments.Current.AuthEndpoint);
            this.Token = new ApiResponse<AuthToken>();
		}

        public async Task GetToken()
        {
            var uri = new Uri(client.BaseAddress, "token");

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("client_id", Environments.Current.ClientId));
			param.Add(new KeyValuePair<string, string>("client_secret", Environments.Current.ClientSecret));
			param.Add(new KeyValuePair<string, string>("grant_type", Environments.Current.GrantType));
            param.Add(new KeyValuePair<string, string>("scope", Environments.Current.Scope));
			param.Add(new KeyValuePair<string, string>("username", Settings.User.Username));
            param.Add(new KeyValuePair<string, string>("password", Settings.User.Password));
            var content = new FormUrlEncodedContent(param);
			try
			{
				var result = await client.PostAsync(uri, content);
                this.Token = await CreateApiResponseByStatusCode<AuthToken>(result);
            }
            catch (Exception ex)
            {
                //this.Token = ExceptionHandler<AuthToken>(ex);
                throw ex;
            }

            if(!Token.HasError)
                CacheUserData.Token = this.Token.Response;
            
        }

        public async Task RefreshToken()
        {
            var uri = new Uri(client.BaseAddress, "token");

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("client_id", Environments.Current.ClientId));
            param.Add(new KeyValuePair<string, string>("client_secret", Environments.Current.ClientSecret));
            param.Add(new KeyValuePair<string, string>("grant_type", Environments.Current.GrantTypeRefreshToken));
            param.Add(new KeyValuePair<string, string>("scope", Environments.Current.Scope));
            param.Add(new KeyValuePair<string, string>("refresh_token", CacheUserData.Token.RefreshToken));
            var content = new FormUrlEncodedContent(param);
            try
            {
                var result = await client.PostAsync(uri, content);
                this.Token = await CreateApiResponseByStatusCode<AuthToken>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!Token.HasError)
                CacheUserData.Token = this.Token.Response;
        }

        public bool CheckTokenExpirated()
        {
            var tokenRequested = CacheUserData.Token.DateTimeRequested;
            var now = DateTime.Now.ToLocalTime();
            var expiresIn = CacheUserData.Token.ExpiresIn;
            return now.Subtract(tokenRequested).TotalSeconds > expiresIn;
        }

        public void DestroyToken()
        {
            Token = new ApiResponse<AuthToken>();
            CacheUserData.Token = null;
        }
    }
}
