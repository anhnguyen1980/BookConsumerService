using BookConsumerService.Services.Interfaces;
using BookConsumerService.Services.MessageQueue.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace BookConsumerService.Services.MessageQueue
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQMessageQueueClient:IMessageQueueClient
    {
                private readonly ISaveData _saveData;
        private readonly string _connectString;
        private readonly string  _queueName;
        private readonly ILogger _logger;
        public RabbitMQMessageQueueClient(ISaveData saveData, string connectString, string queueName, ILogger logger)
        {
                 _saveData = saveData;
            _connectString = connectString;
            _queueName = queueName;
            _logger = logger;
        }
        public Task<bool> ReceiveMessageAsync()
        {
            try
            {
                _logger.LogInformation($"Begin to receive the message queue of Rabbit MQ: {_connectString}");
                var factory = new ConnectionFactory() { HostName = _connectString, DispatchConsumersAsync = true };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                Console.WriteLine("======================================================");
                Console.WriteLine("Press ENTER key to exit after receiving all the messages of RabbitMQ.");
                Console.WriteLine("======================================================");
                _logger.LogInformation($"Declare a new chanel with queue name {_queueName}");
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;//Maybe fix "Convert ReadOnlyMemory<byte> to byte[]": .ToArray()

                    var messageBody = Encoding.UTF8.GetString(body.Span);

                    //    await SaveData.Execute(messageBody, _bookService);
                    _logger.LogInformation($"Start to save the message body: {messageBody}");
                    await _saveData.ExecuteQuery(messageBody);
                    _logger.LogInformation($"Saved data successfully.");
                };
                channel.BasicConsume(queue: _queueName,
                                     autoAck: true,
                                     consumer: consumer);
                _logger.LogInformation($"End Rabbit MQ");
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred {ex.Message}");
                throw ex;
            }

            return Task.FromResult(true);
        }
    }
}

