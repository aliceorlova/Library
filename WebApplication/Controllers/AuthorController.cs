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
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService service;
        public AuthorController(IAuthorService service)
        {
            this.service = service;
        }
        // GET: api/Author
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await service.GetAll());
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await service.GetById(id));
        }

        // POST: api/Author
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Author author)
        {
            return Ok(await service.Add(author));
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BLL.Models.Author author)
        {
            await service.Update(id,author);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.Delete(id);
            return Ok();
        }
    }
}
