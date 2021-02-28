using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models.DB;
using Word_Hole_API.Models.Settings;
using Word_Hole_API.Shared;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly WordHoleDBContext _context;

        public SettingsController(WordHoleDBContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPatch("password")]
        public IActionResult PatchPassword(UpdateProfilePatch parameters)
        {
            if (parameters.New.Count() < 8)
                return BadRequest(new { error = "Password must be at least 8 characters long" });

            if (parameters.New != parameters.Confirm)
                return BadRequest(new { error = "Passwords do not match" });

            var userID = JWTUtility.GetUserID(HttpContext);

            var user = (from users in _context.Users
                        where users.Id == userID
                        select users).Single();

            if (!BCrypt.Net.BCrypt.Verify(parameters.Current, user.Hash))
                return BadRequest(new { error = "Current password incorrect" });

            var newHash = BCrypt.Net.BCrypt.HashPassword(parameters.New);

            user.Hash = newHash;
            _context.SaveChanges();

            return Ok();
        }
    }
}