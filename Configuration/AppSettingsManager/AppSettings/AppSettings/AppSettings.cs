using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettingsManager
{
    public static class AppSettings<Ty> where Ty : class, new()
    {
        public static Ty Instance { get; set; } = new Ty();
        public static void Configure(string settingsPath = "app.json")
        {
            Instance = JsonConvert.DeserializeObject<Ty>(settingsPath) ?? new Ty();
        }
    }
}
