using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using BookConsumerService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BookConsumerService.Models.Mappings;
using BookConsumerService.Repositories.Interfaces;
using BookConsumerService.Repositories;
using BookConsumerService.Services.Interfaces;
using BookConsumerService.Services;
using System.Diagnostics.CodeAnalysis;

namespace BookConsumerService.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();
            Log.Logger = new LoggerConfiguration()
                           .ReadFrom.Configuration(configuration)
                           .Enrich.WithProperty("ApplicationName", "ConsumerService")
                           .WriteTo.Console()                           
                           .CreateLogger();
           
            services.AddSingleton(provider => new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper());
            services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseMySql(
                          configuration.GetConnectionString("MariaDBConnectString"), options=>options.EnableRetryOnFailure(2)));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBookService, BookService>();
            ////IGenericRepository<Book> repository, IUnitOfWork unitOfWork, ITaskService taskService, ILogger< BookService > logger
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISaveData, SaveData>();

            //IServiceProvider services = serviceProvider.BuildServiceProvider();

            //var bookService = services.GetService<IBookService>();

            //var saveData = services.GetService<ISaveData>();
            ////var taskService = services.GetService<ITaskService>();
            ////var unitOfWork = services.GetService<IUnitOfWork>();
            //IClientReceiver clientReceiver = new ClientReceiver(configuration, bookService, saveData);
            //await clientReceiver.ReceiveMessageAsync();

            //Console.ReadKey();
            return services;
        }
    }
}
