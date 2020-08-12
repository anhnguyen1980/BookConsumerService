using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookConsumerService.Services.Interfaces
{
    public interface ISaveData
    {
        public Task<bool> ExecuteQuery(string MessageBody);
    }
}
