using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
namespace BLL.Services
{
    class UserService : IUserService
    {
        readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        readonly AppSettings _settings;
        readonly UserManager<DAL.Entities.AppUser> _userManager;
        readonly RoleManager<IdentityRole<int>> _roleManager;
        public UserService(IUnitOfWork uow, IMapper mapper, IOptions<AppSettings> appSettings, UserManager<DAL.Entities.AppUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _mapper = mapper;
            _unitOfWork = uow;
            _settings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> AddAsync(User user)
        {
            var u = _mapper.Map<DAL.Entities.AppUser>(user);
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
            if (!res.Succeeded) throw new AppException("User could not be created. Check all the fields.");
            var role = await _roleManager.FindByIdAsync(u.RoleId.ToString());
            if (role == null) throw new AppException("Somehow the role does not exist. This really should never ever get displayed.");
            u = await _userManager.FindByEmailAsync(u.Email);
            await _userManager.AddToRoleAsync(u, role.Name);

            var r = _mapper.Map<User>(u);
            r.Role = role.Name;
            r.UserId = u.Id;
            return r;
        }

        public async Task<User> AssignRoleAsync(int id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new AppException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.SingleOrDefault();

            if (currentRole == "Customer")
            {
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

            else throw new AppException("can't really fire admin :) ");
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveAsync();
            var result = _mapper.Map<User>(user);
            var r = await _roleManager.FindByIdAsync(user.RoleId.ToString());
            result.Role = r.Name;
            return result;
        }

        public async Task<User> LoginAsync(User model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var role = await _roleManager.FindByIdAsync(user.RoleId.ToString());

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
                var result = _mapper.Map<User>(user);
                result.Token = tokenHandler.WriteToken(token);
                result.Role = role.Name;
                result.UserId = user.Id;
                return result.WithoutPassword();
            }
            else throw new AppException("Could not login user.");
        }

        public async Task DeleteAsync(int id)
        {
            var user = _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                _unitOfWork.UserRepository.Delete(_mapper.Map<DAL.Entities.AppUser>(user));
                await _unitOfWork.SaveAsync();
            }
            else throw new AppException("Can`t delete user. Wrong id.");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();
            IList<User> result = new List<User>();
            foreach (var user in users)
            {
                var role = await _roleManager.FindByIdAsync(user.RoleId.ToString());
                var res = _mapper.Map<User>(user);
                res.Role = role.Name;
                res.UserId = user.Id;
                result.Add(res);
            }
            return result.WithoutPasswords();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return _mapper.Map<User>(await _unitOfWork.UserRepository.GetByIdAsync(id)).WithoutPassword();
        }
        public async Task<IEnumerable<User>> GetCustomersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();
            var role = await _roleManager.FindByNameAsync("Customer");
            var res = users.Where(x => x.RoleId == role.Id);
            IList<User> result = new List<User>();
            foreach (var user in res)
            {
                //  var _role = await _roleManager.FindByIdAsync(user.RoleId.ToString());
                var r = _mapper.Map<User>(user);
                r.Role = role.Name;
                r.UserId = user.Id;
                result.Add(r);
            }
            return result.WithoutPasswords();
        }
        public async Task UpdateAsync(User userParam, string password)
        {
            if (password == null) throw new AppException("Password can`t be empty.");
            var user = _mapper.Map<DAL.Entities.AppUser>(_userManager.FindByEmailAsync(userParam.Email));
            var result = await _userManager.ChangePasswordAsync(user, userParam.Password, password);
            if (!result.Succeeded) throw new AppException("Something went wrong. Try again.");
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync(int id)
        {
            var bookings = await _unitOfWork.BookingRepository.GetActiveBookingsAsync();
            if (bookings == null) throw new AppException("No Bookings For You.");
            var res = bookings.Where(b => b.User.Id == id);
            return _mapper.Map<IEnumerable<Booking>>(res);
        }

        public async Task<IEnumerable<User>> GetCustomersAndManagersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();
            var custRole = await _roleManager.FindByNameAsync("Customer");
            var managerRole = await _roleManager.FindByNameAsync("Manager");
            var res = users.Where(x => x.RoleId == custRole.Id || x.RoleId == managerRole.Id);
            IList<User> result = new List<User>();
            foreach (var user in res)
            {
                var role = await _roleManager.FindByIdAsync(user.RoleId.ToString());
                var r = _mapper.Map<User>(user);
                r.Role = role.Name;
                r.UserId = user.Id;
                result.Add(r);
            }
            return result.WithoutPasswords();
        }
    }
}
