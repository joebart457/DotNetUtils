using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettingsManager
{
    public static class AppSettings
    {
        public static Ty Get<Ty>(string settingsPath = "app.json") where Ty : class, new()
        {
            return JsonConvert.DeserializeObject<Ty>(File.ReadAllText(settingsPath)) ?? new Ty();
        }

        public static void Save<Ty>(Ty settingsObject, string settingsPath = "app.json") where Ty : class, new()
        {
            File.WriteAllText(settingsPath, JsonConvert.SerializeObject(settingsObject));
        }
    }
}
