using System;
using System.IO;
using System.Text.Json;

namespace KeyboardLayoutSwitcher
{
    public class AppSettings
    {
        public bool EnableAI { get; set; } = true;
        public string OpenAIApiKey { get; set; } = string.Empty;
        public string AIModel { get; set; } = "gpt-4o-mini";
    }

    public static class ConfigManager
    {
        private static readonly string ConfigPath;
        public static AppSettings Settings { get; private set; }

        static ConfigManager()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appData, "KeyboardLayoutSwitcher");
            Directory.CreateDirectory(appFolder);
            ConfigPath = Path.Combine(appFolder, "config.json");

            LoadSettings();
        }

        public static void LoadSettings()
        {
            if (File.Exists(ConfigPath))
            {
                try
                {
                    string json = File.ReadAllText(ConfigPath);
                    Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
                catch
                {
                    Settings = new AppSettings();
                }
            }
            else
            {
                Settings = new AppSettings();
            }
        }

        public static void SaveSettings()
        {
            try
            {
                string json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                // Optionally log error
                Console.WriteLine("Failed to save settings: " + ex.Message);
            }
        }
    }
}
