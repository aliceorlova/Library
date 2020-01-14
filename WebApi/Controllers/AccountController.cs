using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AuthModels;
using Microsoft.Extensions.Configuration;
using BLL.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        readonly IUserService _userService;
        IMapper _mapper;
        readonly IConfiguration _configuration;

        public AccountController(IMapper mapper,
            IUserService userService, IConfiguration configuration)
        {
            _mapper = mapper;
            _userService = userService;
            _configuration = configuration;
            //  _userService.CreateRoles();
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var user = new User { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, isBlocked = false, Password = model.Password };
                var result = await _userService.AddAsync(user);
                return Ok(_mapper.Map<UserModel>(result));
            }
            catch (BLL.AppException ex)
            {
                return BadRequest(ex.Message); ;
                ///  throw new HttpResponseException(ex.Message, HttpStatusCode.BadRequest);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/AssignRole")]
        public async Task<IActionResult> AssignRole(int id)
        {
            //   if (!User.IsInRole("Admin")) return Forbid();
            var result = await _userService.AssignRoleAsync(id);
            return Ok(_mapper.Map<UserModel>(result));
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]AuthenticateModel model)
        {

            var user = await _userService.LoginAsync(_mapper.Map<User>(model));
            var result = _mapper.Map<UserModel>(user);
            return Ok(result);
        }

        //  [Authorize(Roles = "Admin,Manager")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole("Admin") && !User.IsInRole("Manager")) return Forbid();
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            var model = _mapper.Map<UserModel>(user);
            if (model == null) return new NotFoundResult();
            else return Ok(model);
        }

        //[Authorize(Roles = "Admin,Manager")]

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            // if (!User.IsInRole("Admin") && !User.IsInRole("Manager")) return Forbid();
            var users = await _userService.GetAllAsync();
            var models = _mapper.Map<IEnumerable<UserModel>>(users);
            return Ok(models);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("GetCustomers")]
        public async Task<ActionResult> GetCustomers()
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Manager")) return Forbid();
            var users = await _userService.GetCustomersAsync();
            var models = _mapper.Map<IEnumerable<UserModel>>(users);
            return Ok(models);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetCustomersAndManagers")]
        public async Task<ActionResult> GetCustomersAndManagers()
        {
            if (!User.IsInRole("Admin")) return Forbid();
            var users = await _userService.GetCustomersAndManagersAsync();
            var models = _mapper.Map<IEnumerable<UserModel>>(users);
            return Ok(models);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateModel model)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId) return Forbid();

            var user = await _userService.GetByIdAsync(id);

            try
            {
                await _userService.UpdateAsync(user, model.Password);
                return Ok();
            }
            catch (BLL.AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/GetBookings")]
        public async Task<ActionResult> GetBookings(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole("Admin") && !User.IsInRole("Manager")) return Forbid();
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            var model = _mapper.Map<UserModel>(user);
            var bookings = await _userService.GetBookingsAsync(id);
            foreach (var b in bookings)
            {
                b.User = _mapper.Map<User>(model);
            }

            return Ok(bookings);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId) return Forbid();
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }
}

