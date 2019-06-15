using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using RedditClient.Model.Auth;

namespace Commons.Cache
{
    public static class CacheUserData
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string IdLimitHoursToReuseData = "IdMaxHoursToReuseData";
        private const string IdToken = "IdToken";

        #endregion

        #region Methods

        public static double LimitHoursToReuseData
        {
            get
            {
                var result = AppSettings.GetValueOrDefault(IdLimitHoursToReuseData, 3.0);
                return result;
            }
            set
            {
                AppSettings.AddOrUpdateValue(IdLimitHoursToReuseData, value);
            }
        }

        public static AuthToken Token
        {
            get
            {
                var result = AppSettings.GetValueOrDefault(IdToken, null);
                return result == null ? null : JsonConvert.DeserializeObject<AuthToken>(result);
            }
            set
            {
                var result = JsonConvert.SerializeObject(value);
                AppSettings.AddOrUpdateValue(IdToken, result);
            }
        }

        #endregion

    }
}
