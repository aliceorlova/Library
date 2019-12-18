using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
