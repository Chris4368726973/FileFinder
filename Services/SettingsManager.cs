using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace FileFinder
{
    public static class SettingsManager
    {

        private const string SettingsPath = "settings.json";

        public static Settings LoadSettings()
        {
            if (!File.Exists(SettingsPath))
            {
                return new Settings();
            }

            var jsonString = File.ReadAllText(SettingsPath);

            return JsonSerializer.Deserialize<Settings>(jsonString);

        }

        public static void SaveSettings(Settings settings)
        {

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);

            File.WriteAllText(SettingsPath, jsonString);

        }

    }
}
