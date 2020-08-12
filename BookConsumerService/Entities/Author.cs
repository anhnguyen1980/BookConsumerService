using System;
using System.Collections.Generic;
using System.Text;

namespace BookConsumerService.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
