using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Word_Grove_API.Models.DB;

namespace Word_Grove_API
{
    public class JWT
    {
        private readonly WordGroveDBContext _context;
        private readonly IConfiguration _config;

        private readonly string _token = null;

        public JWT(WordGroveDBContext context, IConfiguration config, string username)
        {
            _context = context;
            _config = config;

            var query = (from user in _context.Users
                         where user.Username.ToUpper() == username.ToUpper()
                         select user).FirstOrDefault();

            if (query != null)
                _token = CreateToken(query);
        }

        public string GetToken()
        {
            return _token;
        }

        private string CreateToken(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Access),
                new Claim("Username", user.Username),
                new Claim("UserID", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds);

            return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
        }
    }
}
