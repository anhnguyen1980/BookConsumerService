using System;
using System.Collections.Generic;
using System.Text;

namespace BookConsumerService.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public  BookGenre BookGenre { get; set; }
    }
}
