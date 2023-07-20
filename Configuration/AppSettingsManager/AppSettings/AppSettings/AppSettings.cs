using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppSettingsManager
{
    public static class AppSettings
    {
        public static string SettingsPath { get; set; } = "";
        public static bool GenerateIfNotFound { get; set; } = true;
        public static Ty Get<Ty>(string settingsPath) where Ty : class, new()
        {
            if (string.IsNullOrWhiteSpace(settingsPath)) throw new ArgumentNullException(nameof(settingsPath));
            if (GenerateIfNotFound && !File.Exists(settingsPath))
            {
                var data = new Ty();
                File.WriteAllText(settingsPath, JsonConvert.SerializeObject(data));
                return data;
            }
            return JsonConvert.DeserializeObject<Ty>(File.ReadAllText(settingsPath)) ?? new Ty();
        }

        public static Ty Get<Ty>() where Ty : class, new()
        {
            return Get<Ty>(SettingsPath);
        }

        public static void Save<Ty>(Ty settingsObject, string settingsPath) where Ty : class, new()
        {
            if (string.IsNullOrWhiteSpace(settingsPath)) throw new ArgumentNullException(nameof(settingsPath));
            File.WriteAllText(settingsPath, JsonConvert.SerializeObject(settingsObject));
        }

        public static void Save<Ty>(Ty settingsObject) where Ty : class, new()
        {
            Save(settingsObject, SettingsPath);
        }
    }
}
