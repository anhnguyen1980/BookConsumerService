using System;
using System.Collections.Generic;
using System.Text;

namespace BookConsumerService.Services.MessageQueue.Interfaces
{
   public interface IFactoryClient
    {
        public IMessageQueueClient CreateMessageQueue(string Type);
    }
}
