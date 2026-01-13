using System;
using System.Configuration;

namespace MyNotes.Common.Helpers
{
    public class ConfigHelper
    {
        public static T Get<T>(string key)
        {
            string value = ConfigurationManager.AppSettings[key];

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
