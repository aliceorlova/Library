using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        IMapper mapper;
        readonly AppSettings _settings;
        public readonly UserManager<DAL.Entities.AppUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public UserService(IUnitOfWork uow, IMapper mapper, IOptions<AppSettings> appSettings, UserManager<DAL.Entities.AppUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            this.mapper = mapper;
            _unitOfWork = uow;
            _settings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task CreateRoles()
        {
            string[] roleNames = { "Admin", "Manager", "Customer" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    Console.WriteLine("Creating role " + roleName);
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
                else Console.WriteLine("Role " + roleName + " already exists.");
            }
        }
        public async Task<User> Add(User user)
        {
            var u = mapper.Map<DAL.Entities.AppUser>(user);
            // var existing = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == user.Email);
            var existing = await _userManager.FindByEmailAsync(user.Email);
            if (existing != null) throw new AppException("User with such email already exists.");
            var custRole = await _roleManager.FindByNameAsync("Customer");
            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if (user.Email == "admin@gmail.com")
            {
                u.RoleId = adminRole.Id;
            }
            else u.RoleId = custRole.Id;
            u.UserName = user.Email;
            var res = await _userManager.CreateAsync(u, user.Password);
            if (!res.Succeeded) throw new AppException("Error creating user");
            var role = await _roleManager.FindByIdAsync(u.RoleId.ToString());
            if (role == null) throw new AppException("Somehow the role does not exist.");
            await _userManager.AddToRoleAsync(u, role.Name);
            _unitOfWork.UserRepository.Create(u);
            await _unitOfWork.Save();
            var result = mapper.Map<User>(u);
            result.Role = role.Name;
            return result;
        }

        public async Task<User> AssignRole(int id)
        {
            //   var user = _unitOfWork.UserRepository.GetById(id);
            //  var existingUser = await _unitOfWork.UserRepository.GetById(id);

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new AppException("User not found.");

            // var custRole = await _roleManager.FindByNameAsync("Customer");
            //var managerRole = await _roleManager.FindByNameAsync("Manager");

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.SingleOrDefault();
            if (currentRole == "Customer")
            {
                //  user.RoleId = managerRole.Id;
                await _userManager.RemoveFromRoleAsync(user, "Customer");
                await _userManager.AddToRoleAsync(user, "Manager");
                var role = await _roleManager.FindByNameAsync("Manager");
                user.RoleId = role.Id;
                await _userManager.UpdateAsync(user);

            }
            else if (currentRole == "Manager")
            {
                await _userManager.RemoveFromRoleAsync(user, "Manager");
                await _userManager.AddToRoleAsync(user, "Customer");
                var role = await _roleManager.FindByNameAsync("Customer");
                user.RoleId = role.Id;
                await _userManager.UpdateAsync(user);

            }

            else throw new AppException("can't really fire admin :)");
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
            var result = mapper.Map<User>(user);
            var r = await _roleManager.FindByIdAsync(user.RoleId.ToString());
            result.Role = r.Name;
            return result;
        }

        public async Task<User> Login(User model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var role = await _roleManager.FindByIdAsync(user.RoleId.ToString());

                //   var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes((_settings.Secret)));
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_settings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
              {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role.Name)
              }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var result = mapper.Map<User>(user);
                result.Token = tokenHandler.WriteToken(token);
                //   result.Token = tokenResult;
                result.Role = role.Name;
                return result.WithoutPassword();
            }
            else throw new AppException("Could not login user.");
        }

        public async Task Delete(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user != null)
            {
                _unitOfWork.UserRepository.Delete(mapper.Map<DAL.Entities.AppUser>(user));
                await _unitOfWork.Save();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetUsers();
            IList<User> result = new List<User>();
            foreach (var user in users)
            {
                var role = await _roleManager.FindByIdAsync(user.RoleId.ToString());
                var res = mapper.Map<User>(user);
                res.Role = role.Name;
                result.Add(res);
            }
            return result.WithoutPasswords();
        }

        public async Task<User> GetById(int id)
        {
            return mapper.Map<User>(await _unitOfWork.UserRepository.GetById(id)).WithoutPassword();
        }

        public async Task Update(User userParam, string password = null)
        {

        }


        public async Task<IEnumerable<Booking>> GetBookings(int id)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBookings();
            if (bookings == null) throw new AppException("No Bookings For You.");
            var res = bookings.Where(b => b.AppUser.Id == id);
            return mapper.Map<IEnumerable<Booking>>(res);
        }


    }
}
