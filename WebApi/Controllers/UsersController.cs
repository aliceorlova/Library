/*
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.IServices;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.AuthModels;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _service;
      //  private readonly AppSettings _appSettings;
        private IMapper _mapper;
        public UsersController(IUserService service /* , IOptions<AppSettings> appSettings , IMapper mapper)
        {
            _service = service;
           // _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await _service.Authenticate(model.Email, model.Password);

            if (user == null) return BadRequest(new { message = "Username or password is incorrect" });
          
            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = user.Token,
                Role = user.Role
            });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            // map model to entity
         //   var user = _mapper.Map<User>(model);
            try
            {
                //     user.Role = Role.User;
                // create user
                //    var created = await _service.Create(user, model.Password);
                // return Ok(_mapper.Map<UserModel>(user));
                //   return Ok(_mapper.Map<UserModel>(created));
                return Ok();
            }
            catch (BLL.AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Users
    //    [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var users = await _service.GetAll();
            var models = _mapper.Map<IEnumerable<UserModel>>(users);
            return Ok(models);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
        //    if (id != currentUserId && !User.IsInRole(Role.Admin)) return Forbid();
            var user = await _service.GetById(id);
            if (user == null) return NotFound();
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }


        [HttpGet("{id}/GetBookings")]
        public async Task<ActionResult> GetBookings(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
       //     if( id != currentUserId && !User.IsInRole(Role.Admin)) return Forbid();
            var user = await _service.GetById(id);
            if (user == null) return NotFound();
            var model = _mapper.Map<UserModel>(user);
            var bookings = await _service.GetBookings(id);
            foreach (var b in bookings)
            {
                 b.User = _mapper.Map<User>(model);
            }
            
            return Ok(bookings);
        }

    
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateModel model)
        {
            var user = _mapper.Map<User>(model);
            user.UserId = id;
            try
            {
                // update user 
                await _service.Update(user, model.Password);
                return Ok();
            }
            catch (BLL.AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }

        }
        /*
        [Authorize(Roles = Role.Admin)]
        [HttpPatch]
        public async Task<IActionResult> AssignRole([FromBody]PatchUserRole user)
        {
            var existing = await _service.GetById(user.UserId);
            if(existing == null) return NotFound();
            if (user.Role == Role.User && existing.Role == Role.Manager) existing.Role = Role.User;
            else if (user.Role == Role.Manager && existing.Role == Role.User) existing.Role = Role.Manager;
            try
            {
                // update user 
                await _service.AssignRole(existing);
                return Ok(_mapper.Map<UserModel>(existing));
            }
            catch (BLL.AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
*/