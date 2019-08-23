using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Word_Hole_API.Models;
using Word_Hole_API.Models.DB;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly WordHoleDBContext _context;
        private readonly IConfiguration _config;
        public RegisterController(WordHoleDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public IActionResult ProcessRegistration([FromBody] LoginRegisterBody userInfo)
        {
            if (userInfo.Password.Count() < 8)
            {
                return Ok(new { error = "Password must be at least 8 characters long" });
            }

            if (CheckUserAlreadyExists(userInfo.Username))
            {
                return Ok(new { error = "Username already in use" });
            }

            if (!AddUserToDB(userInfo))
            {
                return Ok(new { error = "An error occurred while trying to register :(" });
            }
            else
            {
                // Success
                var jwt = new JWT(_context, _config, userInfo.Username).GetToken();
                return Ok(new { jwt });
            }
        }

        private bool CheckUserAlreadyExists(string username)
        {
            var query = (from users in _context.Users
                         where users.Username.ToUpper() == username.ToUpper()
                         select users).ToArray();

            return query.Count() > 0;
        }

        private bool AddUserToDB(LoginRegisterBody userInfo)
        {
            //BCrypt bundles its salt in the hash
            var hash = BCrypt.Net.BCrypt.HashPassword(userInfo.Password);

            var newUser = new Users
            {
                Username = userInfo.Username,
                Hash = hash,
                Access = "User"
            };

            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}