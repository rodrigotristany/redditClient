namespace BaseProject.Commons.Environment
{
    public class Environments
    {
        public static Environment Current { set; get; }

        public Environment Dev {get; set; }
        public Environment Qa { get; set; }
        public Environment Uat { get; set; }
        public Environment Prod { get; set; }
    }

    public class Environment
    {
        public string ApiEndpoint { get; set; }
        public string KeyAnalytics { get; set; }
        public string AndroidAppSecret { get; set; }
        public string iOSAppSecret { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string GrantType { get; set; }
        public string GrantTypeRefreshToken { get; set; }
		public string Scope { get; set; }
		public string AuthEndpoint { get; set; }
		public string OriginUrl { get; set; }
    }
}