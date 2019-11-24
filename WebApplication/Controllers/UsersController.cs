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
using WebApplication.AuthModels;

namespace WebApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _service;
        private readonly AppSettings _appSettings;
        private IMapper _mapper;
        public UsersController(IUserService service, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _service = service;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await _service.Authenticate(model.Email, model.Password);

            if (user == null) return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);
            try
            {
                // create user
                await _service.Create(user, model.Password);
                return Ok();
            }
            catch (BLL.AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Users
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [AllowAnonymous]
        [HttpGet("{id}/GetBookings")]
        public async Task<ActionResult> GetBookings(int id)
        {
            return Ok(await _service.GetBookings(id));
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BLL.Models.User user)
        {
            //   return Ok(await _service.(user));
            return Ok();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BLL.Models.User user)
        {
            await _service.Update(user);
            return Ok();
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
