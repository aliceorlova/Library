using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(await service.GetAllAsync());
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var res = await service.GetByIdAsync(id);
            if (res == null) return new NotFoundResult();
            else return Ok(res);
        }

        // POST: api/Authors
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Author author)
        {
            return Ok(await service.AddAsync(author));
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BLL.Models.Author author)
        {
            await service.UpdateAsync(id, author);
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
