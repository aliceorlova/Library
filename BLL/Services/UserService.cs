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

namespace BLL.Services
{
    class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        IMapper mapper;
        readonly AppSettings _settings;
        public UserService(IUnitOfWork uow, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this.mapper = mapper;
            _unitOfWork = uow;
            _settings = appSettings.Value;
        }
        public async Task<User> AssignRole(User user)
        {
            var existing = await _unitOfWork.UserRepository.GetById(user.UserId);
            if (existing == null) return null;
            existing.Role = user.Role;
            //   existing.Role = "Manager";
            _unitOfWork.UserRepository.Update(existing);
            await _unitOfWork.Save();
            return mapper.Map<User>(existing);
        }
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            var users = await _unitOfWork.UserRepository.GetAll();

            var user = mapper.Map<User>(users.SingleOrDefault(x => x.Email == username));

            // check if username exists
            if (user == null) return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            // authentication successful
            return user;
        }

        public async Task<User> Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.Email) && string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(user.FirstName) && string.IsNullOrWhiteSpace(user.LastName)) throw new AppException("Please fill in all the fields to register.");
            if (_unitOfWork.UserRepository.GetAll().Result.Any(u => u.Email == user.Email)) throw new AppException("Username '" + user.Email + "' is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var u = mapper.Map<DAL.Entities.User>(user);

            _unitOfWork.UserRepository.Create(u);
            await _unitOfWork.Save();

            return mapper.Map<User>(u);
        }

        public async Task Delete(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user != null)
            {
                _unitOfWork.UserRepository.Delete(mapper.Map<DAL.Entities.User>(user));
                await _unitOfWork.Save();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return mapper.Map<IEnumerable<User>>(await _unitOfWork.UserRepository.GetUsers()).WithoutPasswords();
        }

        public async Task<User> GetById(int id)
        {
            return mapper.Map<User>(await _unitOfWork.UserRepository.GetById(id)).WithoutPassword();
        }

        public async Task Update(User userParam, string password = null)
        {
            var user = await _unitOfWork.UserRepository.GetById(userParam.UserId);
            var users = await _unitOfWork.UserRepository.GetAll();
            if (user == null) throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // throw error if the new username is already taken
                if (users.Any(x => x.Email == userParam.Email)) throw new AppException("Username " + userParam.Email + " is already taken");

                user.Email = userParam.Email;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName)) user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName)) user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<IEnumerable<Booking>> GetBookings(int id)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBookings();
            if (bookings == null) throw new AppException("No Bookings For You.");
            var res = bookings.Where(b => b.User.UserId == id);
            return mapper.Map<IEnumerable<Booking>>(res);
        }

    }
}
