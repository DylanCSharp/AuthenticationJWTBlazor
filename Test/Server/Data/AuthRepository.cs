using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Test.Shared;

namespace Test.Server.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext datacontext, IConfiguration configuration)
        {
            _context = datacontext;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user is not null)
            {
                 if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                 {
                    return new ServiceResponse<string>() { Success = true, Data = CreateToken(user), Message = "Login Successful" };
                 }
            }
            return new ServiceResponse<string>() { Success = false };
        }

        public async Task<ServiceResponse<int>> Register(Users user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>() { Success = false, Message = "User already exists" };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<int>() { Data = user.Id, Success = true, Message = "Registration Successful" };
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using var hmac = new HMACSHA512(passwordsalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordhash[i])
                {
                    return false;
                }
            }
            return true;
        }

        private string CreateToken(Users user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
