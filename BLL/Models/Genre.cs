using System.Collections.Generic;

namespace BLL.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
