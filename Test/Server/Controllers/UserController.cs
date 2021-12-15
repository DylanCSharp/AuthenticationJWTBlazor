using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Server.Data;
using Test.Shared;
using System.Linq.Expressions;

namespace Test.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _congiguration;

        public UserController(DataContext dataContext, IConfiguration configuration)
        {
            _context = dataContext;
            _congiguration = configuration;
        }

        [HttpGet]
        public List<Users> GetAllUsers()
        {
            List<Users> users = new();
            foreach (var user in _context.Users)
            {
                users.Add(user);
            }

            return users;
        }

        public ActionResult<List<Users>> UpdateUsers(List<Users> updatedusers)
        {
            using SqlConnection conn = new(_congiguration.GetConnectionString("DB"));
            Console.WriteLine(_congiguration.GetConnectionString("DB"));
            conn.Open();
            foreach (var item in updatedusers)
            {
                using SqlCommand command = new("IF NOT EXISTS(SELECT * FROM USERS WHERE Email = '"+item.Email+"') INSERT INTO USERS (Email, Username) VALUES ('"+item.Email+"', '"+item.Username+"');", conn);
                command.ExecuteNonQuery();
            }
            return Ok();
        }
    }
}
