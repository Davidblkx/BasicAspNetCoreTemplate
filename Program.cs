using System.IO;
using Microsoft.AspNetCore.Hosting;
using OwnAspNetCore.Infra;

namespace OwnAspNetCore
{
    public class Program
    {

        public static void Main(string[] args)
        {
            int port = SettingsProvider.LoadSettings().Port;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:" + port)
                .Build();

            host.Run();
        }
    }
}
