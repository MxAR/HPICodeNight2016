using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace HCGServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // framework services.
            services.AddMvc();

            // user defined services
            services.AddTransient<Services.HexConverter.IHexConverter, Services.HexConverter.HexConverter>();
            services.AddTransient<Services.ColorMath.IColorMath, Services.ColorMath.ColorMath>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Logging Setup
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // NLog Setup
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            // Log the Startup of the application
            Services.Logs.LogManagement.SetupLogManagement(loggerFactory, env);

            app.UseMvc();
        }
    }
}
