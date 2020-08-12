using AutoMapper;
using BookConsumerService.Infrastructure;
using BookConsumerService.Repositories;
using BookConsumerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Serilog;
using BookConsumerService.Configurations;
using BookConsumerService.Services;
using System.Diagnostics.CodeAnalysis;

namespace BookConsumerService
{  [ExcludeFromCodeCoverage]
    class Program
    {
      
        static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    // var configuration = services.GetRequiredService<IConfiguration>();
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Consumer host created successful.");
                }
                host.Run();
            }
            catch (Exception ex)
            {

                Log.Fatal($"An error occurred in Main {ex.Message}");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureServices((hostcontext, services) =>
                {
                    services.ConfigureServices();
                    services.AddHostedService<ConsumerHostServices>();
                })
             //Cannot get value of the appsettings.Development.json
             //.ConfigureHostConfiguration(confighost =>
             //{
             //    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
             //    confighost.SetBasePath(System.IO.Path.Combine(AppContext.BaseDirectory))
             //   .AddJsonFile("appsettings.json", optional: true, true)
             //   .AddJsonFile($"appsettings.{environmentName}.json", optional: true,true) 
             //   .AddEnvironmentVariables()
             //   .AddCommandLine(args);
             //})
             .ConfigureAppConfiguration((context, builder) =>
             {
                 var env = context.HostingEnvironment;
                 var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                 builder.SetBasePath(AppContext.BaseDirectory)
                 .AddJsonFile("appsettings.json", optional: false)
                //  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, true)
                 // Override config by env, using like Logging:Level or Logging__Level
                 .AddEnvironmentVariables();

             })
            .UseSerilog();
    }
}

