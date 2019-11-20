using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService service;
        public GenresController(IGenreService service)
        {
            this.service = service;
        }
        // GET: api/Author
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await service.GetAll());
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await service.GetById(id));
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Genre genre)
        {
            return Ok(await service.Add(genre));
        }

        // PUT: api/Genres/5
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
