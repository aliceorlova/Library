using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.IServices;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
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
            return Ok(await service.GetAllAsync());
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            BLL.Models.Genre res;
            try
            {
                res = await service.GetByIdAsync(id);
            }
            catch 
            {
                return new NotFoundResult();
            }
            if (res == null) return new NotFoundResult();
            else return Ok(res);
        }

        // POST: api/Genres
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Genre genre)
        {
            return Ok(await service.AddAsync(genre));
        }

        // PUT: api/Genres/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] BLL.Models.Genre genre)
        {
            await service.UpdateAsync(id, genre);
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
