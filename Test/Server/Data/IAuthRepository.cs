using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Shared;

namespace Test.Server.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(Users user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<bool> UserExists(string email);
    }
}
