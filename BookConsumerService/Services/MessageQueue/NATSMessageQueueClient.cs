using BookConsumerService.Services.Interfaces;
using BookConsumerService.Services.MessageQueue.Interfaces;
using Microsoft.Extensions.Logging;
using NATS.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace BookConsumerService.Services.MessageQueue
{
    [ExcludeFromCodeCoverage]
    public class NATSMessageQueueClient : IMessageQueueClient, IDisposable
    {
        //private readonly IBookService _bookService;
        private readonly ISaveData _saveData;
        private readonly string _connectionString;
        private readonly string _queueName;
        private  IConnection _client;
        private readonly ILogger _logger;
        public NATSMessageQueueClient(ISaveData saveData, string connectString, string queueName, ILogger logger)
        {
            _logger = logger;
            _saveData = saveData;
            _connectionString = connectString;
            _queueName = queueName;
        }

        public void Dispose()
        {
            _client.Drain();
            _client.Close();
            //throw new NotImplementedException();
        }

        public Task<bool> ReceiveMessageAsync()
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages of NATS.");
            Console.WriteLine("======================================================");
            // Create a new connection factory to create
            // a connection.
            ConnectionFactory cf = new ConnectionFactory();
            // Creates a live connection to the default
            // NATS Server running locally
            _client = cf.CreateConnection(_connectionString);
            // Setup an event handler to process incoming messages.
            // An anonymous delegate function is used for brevity.
            EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
            {
                // print the message
                //Console.WriteLine(args.Message);
                var messageBody = Encoding.UTF8.GetString(args.Message.Data);
                _saveData.ExecuteQuery(messageBody) ;
                // Here are some of the accessible properties from
                // the message:
                // args.Message.Data;
                // args.Message.Reply;
                // args.Message.Subject;
                // args.Message.ArrivalSubcription.Subject;
                // args.Message.ArrivalSubcription.QueuedMessageCount;
                // args.Message.ArrivalSubcription.Queue;

                // Unsubscribing from within the delegate function is supported.
                // args.Message.ArrivalSubcription.Unsubscribe();
            };
            // The simple way to create an asynchronous subscriber
            // is to simply pass the event in.  Messages will start
            // arriving immediately.
            IAsyncSubscription s = _client.SubscribeAsync(_queueName, h);
            Console.ReadLine();
            s.Unsubscribe();
            return Task.FromResult(true);
        }

    }
}
