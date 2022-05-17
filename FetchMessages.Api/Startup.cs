using Messages.Api.Services;
using Messages.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using Microsoft.Identity.Web;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace FetchMessages.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMessageDB, MessageDB>();
            // services.AddApplicationInsightsTelemetry("892a0d11-948c-4b06-890e-2fb2b0234d1c");
            services.AddLogging(confiure =>
            {
                confiure.AddApplicationInsights("fc1382d3-ee97-4b6e-bf0a-83a2ba23a15d");
                confiure.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information).
                AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Error);
            });
                // existing code which include services.AddApplicationInsightsTelemetry() to enable Application Insights.
            services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((module, o) => module.AuthenticationApiKey = "n662kpeem4ok2os71desvwnasfzi4kiidezijvbe");

            services.AddAzureAppConfiguration();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

     
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAzureAppConfiguration();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.Use(async (context, next) =>//created our own custom middleware
            //{
            //    if (!context.User.Identity?.IsAuthenticated ?? false)//checking if user is authanticated or not
            //    {
            //        //if not then 
            //        context.Response.StatusCode = 401;
            //        await context.Response.WriteAsync("Access Denied need Access Tocken ");
            //    }
            //    else await next();

            //});

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
