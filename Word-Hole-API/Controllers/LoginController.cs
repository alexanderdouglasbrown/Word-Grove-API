using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Word_Grove_API.Models.Login;
using Word_Grove_API.Models.DB;

namespace Word_Grove_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly WordGroveDBContext _context;
        private readonly IConfiguration _config;

        public LoginController(WordGroveDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public IActionResult ProcessLogin([FromBody] LoginPost userInfo)
        {
            var userData = GetUserFromDB(userInfo.Username);

            if (userData == null || !BCrypt.Net.BCrypt.Verify(userInfo.Password, userData.Hash))
            {
                return BadRequest(new { error = "Incorrect username or password" });
            }

            // Success
            var jwt = new JWT(_context, _config, userInfo.Username).GetToken();
            return Ok(new { jwt });
        }

        private Users GetUserFromDB(string username)
        {
            var user = (from users in _context.Users
                        where users.Username.ToUpper() == username.ToUpper()
                        select users).FirstOrDefault();

            return user;
        }
    }
}