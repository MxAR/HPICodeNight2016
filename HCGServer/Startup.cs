using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCGServer.Services;
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
                .AddJsonFile("Appttings/Appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            dev_env = env.IsDevelopment();
        }

        public IConfigurationRoot Configuration { get; }
        private static bool dev_env { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // framework services.
            services.AddMvc();

            // user defined services
            services.AddTransient<Services.HexConverter.IHexConverter, Services.HexConverter.HexConverter>();
            services.AddTransient<Services.EmailSender.IEmailSender, Services.EmailSender.EmailSender>();

            // add Dummy helper
            if (dev_env) { services.AddTransient<Dummy>(); }
        }

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
