using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Commons
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string UserKey = "username_key";
		private static readonly string UserKeyDefault = string.Empty;

        private const string PasswordKey = "password_key";
        private static readonly string PasswordKeyDefault = string.Empty;
        
		private const string IsLoginKey = "is_login";
		private static readonly string IsLoginDefault = string.Empty;

		#endregion

		#region Methods

        public static bool IsLoggedIn
        {
			get => Settings.User != null;         
        }

        public static User User
        {
            get
            {
                var result = AppSettings.GetValueOrDefault(UserKey, null);
                return result == null ? null : JsonConvert.DeserializeObject<User>(result);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserKey, JsonConvert.SerializeObject(value));
            }
        }

		#endregion
	}
}