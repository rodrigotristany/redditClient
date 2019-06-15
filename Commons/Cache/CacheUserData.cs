using BaseProject.Models.Auth;
using BaseProject.Models.Shopping;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace BaseProject.Commons.Cache
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
        private const string IdShoppingCart = "IdShoppingCart";

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

        public static ShoppingCartDto ShoppingCart
        {
            get
            {
                var result = AppSettings.GetValueOrDefault(IdShoppingCart, null);
                return result == null ? null : JsonConvert.DeserializeObject<ShoppingCartDto>(result);
            }
            set
            {
                var result = JsonConvert.SerializeObject(value);
                AppSettings.AddOrUpdateValue(IdShoppingCart, result);
            }
        }

        public static void DeleteUserDataDuringLogout()
        {
            Token = null;
            ShoppingCart = null;
        }

        #endregion

    }
}
