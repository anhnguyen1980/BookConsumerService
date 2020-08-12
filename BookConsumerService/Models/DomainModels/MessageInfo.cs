using System;
using System.Collections.Generic;
using System.Text;
using BookConsumerService.Models.DTOs;

namespace BookConsumerService.Models.DomainModels
{
    public class MessageInfo
    {
        public string ActionType { get; set; }
        public BookDto BookDto { get; set; }

    }
}
