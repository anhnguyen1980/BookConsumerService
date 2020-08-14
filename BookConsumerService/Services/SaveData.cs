using BookConsumerService.Models.DomainModels;
using BookConsumerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookConsumerService.Services
{
    public class SaveData : ISaveData
    {
        private readonly IBookService _bookService;
        private readonly ILogger<SaveData> _logger;
        public SaveData( IBookService bookService, ILogger<SaveData> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }
        public async Task<bool> ExecuteQuery(string messageBody)
        {
            bool res = false;
            try
            {
                MessageInfo messageInfo = JsonConvert.DeserializeObject<MessageInfo>(messageBody);
                _logger.LogInformation($"Process data with request {messageInfo.ActionType}");
                switch (messageInfo.ActionType.ToUpper())
                {
                    case "POST":
                    case "PUT":
                        res = await _bookService.SaveBook(messageInfo);
                        break;
                    case "DELETE":
                        res = await _bookService.DeleteBook(messageInfo.BookDto.Id);
                        break;
                    default:
                        break;
                }
                if (res)
                    _logger.LogInformation($"{messageInfo.ActionType } successfully.");
            }
            catch (Exception ex)
            {
               _logger.LogError($"An error occurred in SaveData class :{Environment.NewLine} {ex.Message}");
                throw ex;
            }
            return res;
        }
    }
}
