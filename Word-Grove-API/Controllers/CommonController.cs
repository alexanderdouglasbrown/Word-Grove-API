using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Word_Grove_API.Models.DB;

namespace Word_Grove_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly WordGroveDBContext _context;

        public CommonController(WordGroveDBContext context)
        {
            _context = context;
        }

        [HttpGet("hello")]
        public IActionResult GetHi()
        {
            _context.Database.OpenConnection();
            return Ok("Hi");
        }
    }
}
