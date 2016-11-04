using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HCGServer.Services.Logs 
{
    public static class LogManagement
    {
        private static ILogger _logger { get; set; }
        private static Timer FlushPeriod { get; set; }
        private static int ReservedLogSpace { get; set; }
        private static DateTime StartedAt { get; set; } 
        private static IHostingEnvironment ENV { get; set;}

        public static Task SetupLogManagement(ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            ENV = env;
            StartedAt = DateTime.Now;
            _logger = loggerFactory.CreateLogger("HCGServer.Services.Logs");
            LogStartup();

            FlushPeriod = new Timer(new TimerCallback(FlushLogs), null, new TimeSpan(0, 0, 0), new TimeSpan(7, 0, 0, 0));
            JObject _DBC = JObject.Parse(File.ReadAllText($"{Directory.GetCurrentDirectory()}/appsettings.json"));
            ReservedLogSpace = (int)_DBC["Logging"]["ReservedSpaceMB"];

            return Task.FromResult(0);
        }

        private static void LogStartup() 
        {
            string ph = "----------------------";
            string Time = StartedAt.ToString("D", new CultureInfo("de-DE"));
            for (int i = 0; i < 3; i++) { _logger.LogInformation(ph); }
            _logger.LogInformation("Application start went succesfully");
            _logger.LogInformation($">>>> Application : {ENV.ApplicationName}");
            _logger.LogInformation($">>>> Enviroment : {ENV.EnvironmentName}");
            _logger.LogInformation($">>>> root-path : {ENV.ContentRootPath}");
            _logger.LogInformation($">>>> Time : {Time}");
            for (int i = 0; i < 3; i++) { _logger.LogInformation(ph); }

            if (ENV.IsDevelopment()) {
                for (int i = 0; i < 3; i++) { _logger.LogDebug(ph); }
                _logger.LogDebug("Application start went succesfully");
                _logger.LogDebug($">>>> Application : {ENV.ApplicationName}");
                _logger.LogDebug($">>>> Enviroment : {ENV.EnvironmentName}");
                _logger.LogDebug($">>>> root-path : {ENV.ContentRootPath}");
                _logger.LogDebug($">>>> Time : {Time}");
                for (int i = 0; i < 3; i++) { _logger.LogDebug(ph); }
            }
        }

        public static void LogShutdown() 
        {
            string ph = "----------------------";
            string UpTime = (DateTime.Now - StartedAt).ToString("D", new CultureInfo("de-DE"));
            for (int i = 0; i < 3; i++) { _logger.LogInformation(ph); }
            _logger.LogInformation($"Application shutdown went succesfully");
            _logger.LogInformation($">>>> Application : {ENV.ApplicationName}");
            _logger.LogInformation($">>>> Enviroment : {ENV.EnvironmentName}");
            _logger.LogInformation($">>>> root-path : {ENV.ContentRootPath}");
            _logger.LogInformation($">>>> UpTime : {UpTime}");
            for (int i = 0; i < 3; i++) { _logger.LogInformation(ph); }

            if (ENV.IsDevelopment()) {
                for (int i = 0; i < 3; i++) { _logger.LogDebug(ph); }
                _logger.LogDebug($"Application shutdown went succesfully");
                _logger.LogDebug($">>>> Application : {ENV.ApplicationName}");
                _logger.LogDebug($">>>> Enviroment : {ENV.EnvironmentName}");
                _logger.LogDebug($">>>> root-path : {ENV.ContentRootPath}");
                _logger.LogDebug($">>>> UpTime : {UpTime}");
                for (int i = 0; i < 3; i++) { _logger.LogDebug(ph); }
            }
        }

        private static void FlushLogs(object k)
        {
            string LogDirectory = $"{AppContext.BaseDirectory}Logs/";
            try{
                if (Directory.Exists(LogDirectory)) {
                    if (GetDirectorySize(LogDirectory) >= ReservedLogSpace){
                        List<string> Files = Directory.GetFiles(LogDirectory).ToList();
                        List<string> SubDirectories = Directory.GetDirectories(LogDirectory).ToList();

                        #pragma warning disable 4014
                        if (Files.Any()){ Files.ForEach(e => File.Delete(e)); }
                        if (SubDirectories.Any()) { Parallel.ForEach(SubDirectories, e => FlushLogs(e)); }
                        #pragma warning restore 4014
                    }
                }
            } catch (IOException ex) { _logger.LogCritical(ex.Message); }
        }

        private static long GetDirectorySize(string DIRP)
        {
            string[] DIR = Directory.GetFiles(DIRP, "*.*");
            long size = 0;
            
            Parallel.ForEach(DIR, e => {
                FileInfo INFO = new FileInfo(e);
                size += INFO.Length;

            });

            return size;
        }
    }
}