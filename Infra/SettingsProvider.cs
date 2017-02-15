using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OwnAspNetCore.Infra
{
    internal class SettingsProvider : ISettings
    {
        //Default name to save settings
        private static readonly string fileName = "settings.json";

        //Initialize a ISettings using the default params
        public SettingsProvider()
        : this(null) { }

        //Initialize a ISettings instance using a reference
        public SettingsProvider(ISettings s)
        {

            Port = s?.Port ?? 5000;

        }

        public int Port { get; set; }

        //Save settings to disk
        public async Task SaveAsync()
        {
            using (FileStream writeStream = File.OpenWrite(fileName))
            {

                var jsonString = JsonConvert.SerializeObject(this, typeof(ISettings), new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });

                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                await writeStream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
            }
        }

        //Load settings from disk
        public static async Task<SettingsProvider> LoadSettingsAsync()
        {
            //Try to load from disk
            if (File.Exists(fileName))
            {
                try
                {
                    ISettings settings;

                    using (FileStream reader = File.OpenRead(fileName))
                    {
                        var jsonBytes = new byte[reader.Length];
                        await reader.ReadAsync(jsonBytes, 0, jsonBytes.Length);

                        var jsonString = Encoding.UTF8.GetString(jsonBytes);
                        settings = JsonConvert.DeserializeObject<SettingsProvider>(jsonString);
                    }

                    var provider = new SettingsProvider(settings);

                    //Save updated fields
                    await provider.SaveAsync();

                    return provider;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error loading settings");
                    Console.WriteLine(e.Message);
                }
            }

            var newProvider = new SettingsProvider();
            await newProvider.SaveAsync();
            return newProvider;
        }

        public static SettingsProvider LoadSettings()
        {
            return LoadSettingsAsync().Result;
        }
    }
}