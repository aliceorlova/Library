using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService service;
        public BooksController(IBookService service)
        {
            this.service = service;
        }
        // GET: api/Author
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await service.GetAllAsync());
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(int id)
        {
            var res = await service.GetByIdAsync(id);
            if (res == null) return new NotFoundResult();
            else return Ok(res);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Book book)
        {
            return Ok(await service.AddAsync(book));
        }

        [HttpPost("{id}/AddAuthor")]
        public async Task<IActionResult> AddAuthor(int id, [FromBody] BLL.Models.Author author)
        {
            await service.AddAuthorAsync(id, author);
            return Ok();
        }

        //[HttpPost("{id}/AddAuthors")]
        //public async Task<IActionResult> AddAuthors(int id, [FromBody] ICollection<BLL.Models.Author> authors)
        //{
        //    await service.AddAuthors(id, authors);
        //    return Ok();
        //}
        [HttpPost("{id}/AddGenre")]
        public async Task<IActionResult> AddGenre(int id, [FromBody] BLL.Models.Genre genre)
        {
            await service.AddGenreAsync(id, genre);
            return Ok();
        }

        //[HttpPost("{id}/AddGenres")]
        //public async Task<IActionResult> AddGenres(int id, [FromBody] ICollection<BLL.Models.Genre> genres)
        //{
        //    await service.AddGenres(id, genres);
        //    return Ok();
        //}
        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BLL.Models.Book book)
        {
            await service.UpdateAsync(id, book);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await service.Delete(id);
        //    return Ok();
        //}
    }
}
