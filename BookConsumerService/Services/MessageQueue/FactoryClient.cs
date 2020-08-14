using BookConsumerService.Services.Interfaces;
using BookConsumerService.Services.MessageQueue.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookConsumerService.Services.MessageQueue
{
    public class FactoryClient : IFactoryClient
    {
        private readonly IConfiguration _configuration;
        private readonly ISaveData _saveData;
        private readonly ILogger _logger;
        public FactoryClient(IConfiguration configuration, ISaveData saveData, ILogger logger)
        {
            _configuration = configuration;
            _saveData = saveData;
            _logger = logger;
        }
        public IMessageQueueClient CreateMessageQueue(string type)
        {
            _logger.LogInformation($"Message queue type is {type}");
            if (type == "RabbitMQ")
            {
                return new RabbitMQMessageQueueClient(_saveData, _configuration["RabbitMQ:ConnectionString"], _configuration["RabbitMQ:QueueName"], _logger);
            }
            else if (type == "NATS")
                return new NATSMessageQueueClient( _saveData, _configuration["NATS:ConnectionString"], _configuration["NATS:QueueName"], _logger);
            else
                return new AzureMessageQueueClient( _saveData, _configuration["AzureServiceBus:ConnectionString"], _configuration["AzureServiceBus:QueueName"], _logger);
        }
    }
}
