using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

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

        public static string[] GetArray
        (
            string keyName,
            string defaulValue,
            bool   trim      = true,
            string separator = ";"
        )
        {
            string configValue = ConfigurationManager.AppSettings.Get(keyName);

            if (String.IsNullOrEmpty(configValue))
                return GetArray(defaulValue, trim, separator);
          
            return GetArray(configValue, trim, separator);
        }

        public static string[] GetArray
        (
            string listValues,
            bool   trim      = true,
            string separator = ";"
        )
        {
            if (String.IsNullOrEmpty(listValues))
                return new string[]{ };

            return listValues.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => (trim ? x.Trim() : x))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
        }

        /// <summary>
        /// Returns the specified application setting as a string dictionary object.
        /// </summary>
        /// <param name="keyName">
        /// Name of the key in the 'appSettings' configuration section.
        /// </param>
        /// <param name="defaultValue">
        /// The default value that will be returned if the specified key either does not exist or is empty.
        /// </param>
        /// <param name="trimNames">
        /// When set to true, names will be trimmed.
        /// </param>
        /// <param name="trimValues">
        /// When set to true, values will be trimmed.
        /// </param>
        /// <param name="nameValueSeparator">
        /// Character used to separate names (keys) from values in the string.
        /// </param>
        /// <param name="elementSeparator">
        /// Character used to separate name-value pairs in the string.
        /// </param>
        /// <returns>
        /// The configured or the default application setting converted to a string dictionary object.
        /// </returns>
        /// <remarks>
        /// An example of the name-value pair string would be: "a=b;c=d;e=f".
        /// </remarks>
        public static Dictionary<string, string> GetDictionary
        (
            string keyName,
            string defaulValue,
            bool   trimNames          = true,
            bool   trimValues         = false,        
            string nameValueSeparator = "=",
            string elementSeparator   = ";"
        )
        {
            string configValue = ConfigurationManager.AppSettings.Get(keyName);

            if (String.IsNullOrEmpty(configValue))
                return GetDictionary(defaulValue, trimNames, trimValues, nameValueSeparator, elementSeparator);
            
            return GetDictionary(configValue, trimNames, trimValues, nameValueSeparator, elementSeparator);
        }

        /// <summary>
        /// COnverts a name-value pair string to a string dictionary object.
        /// </summary>
        /// <param name="nameValuePairs">
        /// String holding name-value pairs.
        /// </param>
        /// <param name="trimNames">
        /// When set to true, names will be trimmed.
        /// </param>
        /// <param name="trimValues">
        /// When set to true, values will be trimmed.
        /// </param>
        /// <param name="nameValueSeparator">
        /// Character used to separate names (keys) from values in the string.
        /// </param>
        /// <param name="elementSeparator">
        /// Character used to separate name-value pairs in the string.
        /// </param>
        /// <returns>
        /// The configured or the default application setting converted to a string dictionary object.
        /// </returns>
        /// <remarks>
        /// An example of the name-value pair string would be: "a=b;c=d;e=f".
        /// </remarks>
        public static Dictionary<string, string> GetDictionary
        (
            string nameValuePairs,
            bool trimNames          = true,
            bool trimValues         = false,
            string nameValueSeparator = "=",
            string elementSeparator   = ";"
        )
        {
            if (String.IsNullOrEmpty(nameValuePairs))
                return new Dictionary<string, string>();

            // Convert tokenized string to map.
            // See https://stackoverflow.com/questions/4141208/convert-a-delimted-string-to-a-dictionarystring-string-in-c-sharp
            return nameValuePairs
                    .Split(new string[] { elementSeparator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(part  => part.Split(new string[] { nameValueSeparator }, StringSplitOptions.RemoveEmptyEntries))
                    .Where(part => part.Length == 2)
                    .ToDictionary(
                        sp => (trimNames  ? sp[0].Trim() : sp[0]), 
                        sp => (trimValues ? sp[1].Trim() : sp[1]));

        }

        // https://stackoverflow.com/questions/170665/helper-functions-for-safe-conversion-from-strings/170731#170731
        //private static T SafeConvert<T>
        //(
        //    string s
        //)
        //{
        //    if (string.IsNullOrEmpty(s))
        //        return default(T);

        //    return (T)Convert.ChangeType(s, typeof(T));
        //}
    }
}
