using System;
using System.Collections.Specialized;
using System.Configuration;

namespace BasicAppSettings
{
    /// <summary>
    /// Implements helper methods for reading sensitive and non-sensitive application settings from the configuration file.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Returns the specified application setting value.
        /// </summary>
        /// <typeparam name="T">
        /// Data type of the application setting.
        /// </typeparam>
        /// <param name="keyName">
        /// Name of the key in the 'appSettings' configuration section.
        /// </param>
        /// <param name="defaultValue">
        /// The default value that will be returned if the specified key either does not exist or is empty.
        /// </param>
        /// <returns>
        /// The configured or the default application setting.
        /// </returns>
        public static T GetValue<T>
        (
            string keyName,
            T defaultValue
        )
        {
            string configValue = ConfigurationManager.AppSettings.Get(keyName);

            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return (T)Convert.ChangeType(configValue, typeof(T));
        }

        /// <summary>
        /// Returns a sensitive application setting value.
        /// </summary>
        /// <typeparam name="T">
        /// Data type of the sensitive application setting (most likely, string).
        /// </typeparam>
        /// <param name="keyName">
        /// Name of the key in the sensitive application configuration section.
        /// </param>
        /// <param name="secretSectionName">
        /// Name of the sensitive application configuration section.
        /// </param>
        /// <returns>
        /// The decrypted value of the specified sensitive application setting.
        /// </returns>
        public static T GetSecret<T>
        (
            string keyName,
            string secretSectionName = "secureAppSettings"
        )
        {
            string configValue = ConfigurationManager.AppSettings.Get(keyName);

            if (!String.IsNullOrEmpty(configValue))
                return (T)Convert.ChangeType(configValue, typeof(T));

            var section = ConfigurationManager.GetSection(secretSectionName)
                as NameValueCollection;

            if (section == null)
                throw new Exception(
                    String.Format("Cannot read section '{0}' from the configuration file.",
                        secretSectionName));

            configValue = section[keyName] as string;

            return (T)Convert.ChangeType(configValue, typeof(T));
        }
    }
}
