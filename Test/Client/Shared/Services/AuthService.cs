using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Test.Client.Shared.Models;
using Test.Shared;

namespace Test.Client.Shared.Services
{
    public class AuthService : IAuthService
    {
        private HttpClient _http;

        public AuthService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<ServiceResponse<string>> Login(LoginModel loginModel)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", loginModel);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(RegisterModel registerUser)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", registerUser);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
