using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;
        public BookingsController(IBookingService service)
        {
            _service = service;
        }
        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet("GetAllActive")]
        public async Task<ActionResult> GetAllActive()
        {
            return Ok(await _service.GetAllActiveAsync());
        }
        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null) return new NotFoundResult();
            else return Ok(res);
        }



        [Authorize(Roles = "Manager,Admin")]
        [HttpPut("{id}/FinishBooking")]
        public async Task<IActionResult> FinishBooking(int id)
        {
            return Ok(await _service.FinishBookingAsync(id));
        }

        // POST: api/Bookings
        [Authorize(Roles = "Manager,Admin")]
        //  [Authorize(Roles = Role.Manager)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BLL.Models.Booking booking)
        {
            return Ok(await _service.AddAsync(booking));
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BLL.Models.Booking booking)
        {
            await _service.UpdateAsync(id, booking);
            return Ok();
        }
        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _service.Delete(id);
        //    return Ok();
        //}
    }
}
