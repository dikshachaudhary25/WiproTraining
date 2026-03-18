using System.Collections.Generic;

namespace BookStoreApi.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}