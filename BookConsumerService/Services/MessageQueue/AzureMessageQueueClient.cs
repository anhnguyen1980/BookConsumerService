using BookConsumerService.Services.Interfaces;
using BookConsumerService.Services.MessageQueue.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookConsumerService.Services.MessageQueue
{
    [ExcludeFromCodeCoverage]
    public class AzureMessageQueueClient : IMessageQueueClient
    {
        private readonly ISaveData _saveData;
        private IQueueClient queueClient;
        private readonly string _connectString;
        private readonly string _queueName;
        private readonly ILogger _logger;
        public AzureMessageQueueClient(ISaveData saveData, string connectString, string queueName, ILogger logger)
        {
            _saveData = saveData;
            _connectString = connectString;
            _queueName = queueName;
            _logger = logger;
        }
        public Task<bool> ReceiveMessageAsync()
        {
            MainAsync(_connectString, _queueName).GetAwaiter().GetResult();
            return Task.FromResult(true);
        }
        async System.Threading.Tasks.Task MainAsync(string connectString, string queueName)
        {
            //string ServiceBusConnectionString = @"Endpoint=sb://anhnguyen.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qTKznJRadfRhedcnYAR6NC9CVM9DPEoMbcF9PSIy27Q=";
            //const string QueueName = "myQueue";
            //string ServiceBusConnectionString = _configuration["AzureServiceBus:ConnectionString"];//; @"Endpoint=sb://anhnguyen.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qTKznJRadfRhedcnYAR6NC9CVM9DPEoMbcF9PSIy27Q=";
            //string QueueName = _configuration["AzureServiceBus:QueueName"];
            _logger.LogInformation($"Begin to receive the message queue of Azure Service Bus: {_connectString}");
            queueClient = new QueueClient(connectString, queueName);
            _logger.LogInformation($"Initial QueueClient with queue name: {_queueName}");
            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages of Azure Service Bus.");
            Console.WriteLine("======================================================");

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await queueClient.CloseAsync();
            _logger.LogInformation($"Finish receiving the message queue of Azure SB");
        }
        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        async System.Threading.Tasks.Task ProcessMessagesAsync(Message message, CancellationToken token)
        {

            string messageBody = Encoding.UTF8.GetString(message.Body);
            // Console.WriteLine(messageInfo.actionType.ToUpper() + " successful." + Environment.NewLine);
            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            //  _logger.LogInformation($"Saved message Number: {message.SystemProperties.SequenceNumber}");
            //Console.WriteLine($"Saved message Number: {message.SystemProperties.SequenceNumber}"+ Environment.NewLine);
            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
            // Process the message.
            _logger.LogInformation($"Start to process the message body: {messageBody}");
            await _saveData.ExecuteQuery(messageBody);
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
