using Azure.Identity;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace FetchMessages.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
           CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
              webBuilder.ConfigureAppConfiguration(config =>
              {
                  var settings = config.Build();
                  var cs = settings.GetSection("AppConfig").Value;
                  config.AddAzureAppConfiguration(options =>
                  {
                      options.Connect(cs).Select(KeyFilter.Any, "cosmoDemoApp")
                              .ConfigureKeyVault(kv =>
                              {
                                  kv.SetCredential(new DefaultAzureCredential());
                              });
                  });
              }).UseStartup<Startup>());
    }
}
