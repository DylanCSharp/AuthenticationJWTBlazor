using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Client.Shared.Models;
using Test.Shared;

namespace Test.Client.Shared.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(RegisterModel registerUser);
        Task<ServiceResponse<string>> Login(LoginModel loginModel);
    }
}
