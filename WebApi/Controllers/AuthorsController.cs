using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using BLL.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService service;
        public AuthorsController(IAuthorService service)
        {
            this.service = service;
        }
        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await service.GetAll());
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var res = await service.GetById(id);
            if (res == null) return new NotFoundResult();
            else return Ok(res);
        }

        // POST: api/Authors
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Author author)
        {
            return Ok(await service.Add(author));
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BLL.Models.Author author)
        {
            await service.Update(id, author);
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
