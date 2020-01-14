using System.Collections.Generic;


namespace BLL.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public int BookYear { get; set; }
        public int NumberAvailable { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
