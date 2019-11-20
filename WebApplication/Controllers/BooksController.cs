using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Services;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult> GetAll()
        {
            return Ok(await service.GetAll());
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await service.GetById(id));
        }

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Book book)
        {
            return Ok(await service.Add(book));
        }

        [HttpPost("{id}/AddAuthor")]
        public async Task<IActionResult> AddAuthor(int id, [FromBody] BLL.Models.Author author)
        {
            await service.AddAuthor(id, author);
            return Ok();
        }

        [HttpPost("{id}/AddAuthors")]
        public async Task<IActionResult> AddAuthors(int id, [FromBody] ICollection<BLL.Models.Author> authors)
        {
            await service.AddAuthors(id, authors);
            return Ok();
        }
        [HttpPost("{id}/AddGenre")]
        public async Task<IActionResult> AddGenre(int id, [FromBody] BLL.Models.Genre genre)
        {
            await service.AddGenre(id, genre);
            return Ok();
        }

        [HttpPost("{id}/AddGenres")]
        public async Task<IActionResult> AddGenres(int id, [FromBody] ICollection<BLL.Models.Genre> genres)
        {
            await service.AddGenres(id, genres);
            return Ok();
        }
        // PUT: api/Books/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {   
            
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
