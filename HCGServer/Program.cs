using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Server;
using System.IO;
using System;

namespace HCGServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Appsettings/Appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            
            var ServerConfig = Configuration.GetSection("Server");
            ServerConfig["Type"] = ServerConfig["Type"] ?? "Kestrel";

            var Host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseIISIntegration();

            if (string.Equals(ServerConfig["Type"], "Kestrel", StringComparison.OrdinalIgnoreCase)) {
                Host.UseKestrel(options => {
                    if (ServerConfig["ThreadCount"] != null) { options.ThreadCount = int.Parse(ServerConfig["ThreadCount"]); }
                });
            } else if (string.Equals(ServerConfig["Type"], "WebListener", StringComparison.OrdinalIgnoreCase)) {
                Host.UseWebListener(options => {
                    options.Listener.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.AllowAnonymous;
                });
            }
            
            Host.Build().Run();
        }
    }
}
