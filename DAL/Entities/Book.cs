using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    /// <summary>
    /// Create Linking table repositories
    /// </summary>
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
