using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookConsumerService.Services.MessageQueue.Interfaces
{
    public interface IMessageQueueClient
    {
        public Task<bool> ReceiveMessageAsync();
    }
}
