using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models;
using Word_Hole_API.Models.DB;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly WordHoleDBContext _context;
        public RegisterController(WordHoleDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ProcessRegistration([FromBody] LoginRegister userInfo)
        {
            if (userInfo.Password.Count() < 8)
            {
                return Ok(new { valid = false, error = "Password must be at least 8 characters long" });
            }

            if (CheckUserAlreadyExists(userInfo.Username))
            {
                return Ok(new { valid = false, error = "Username in use" });
            }

            if (!AddUserToDB(userInfo))
            {
                return Ok(new { valid = false, error = "An error occurred while trying to register :(" });
            }
            else
            {
                return Ok(new { valid = true }); // :)
            }
        }

        private bool CheckUserAlreadyExists(string username)
        {
            var query = (from users in _context.Users
                         where users.Username.ToUpper() == username.ToUpper()
                         select users).ToArray();

            return query.Count() > 0;
        }

        private bool AddUserToDB(LoginRegister userInfo)
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