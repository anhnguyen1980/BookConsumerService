using BookConsumerService.Services.Interfaces;
using BookConsumerService.Services.MessageQueue;
using BookConsumerService.Services.MessageQueue.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookConsumerService.Services
{
    [ExcludeFromCodeCoverage]
    public class ConsumerHostServices:IHostedService
    {

        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<ConsumerHostServices> _logger;
        private readonly IMessageQueueClient _client;

        public ConsumerHostServices(IConfiguration configuration, ISaveData saveData, IHostApplicationLifetime appLifetime, ILogger<ConsumerHostServices> logger)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            IFactoryClient factory = new FactoryClient(configuration, saveData, _logger);
            _client = factory.CreateMessageQueue(configuration["MessageQueue:Type"]);
        }
        public void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            _client.ReceiveMessageAsync();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
        }
        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
