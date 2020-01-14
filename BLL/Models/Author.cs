using System.Collections.Generic;


namespace BLL.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
