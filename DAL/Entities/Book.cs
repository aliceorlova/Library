using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Name { get; set; }
        public int BookYear { get; set; }
        public int NumberAvailable { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
