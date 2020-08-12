using System;
using System.Collections.Generic;
using System.Text;

namespace BookConsumerService.Entities
{
    public class Status
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public ICollection< TaskHistory> TaskHistory { get; set; }
    }
}
