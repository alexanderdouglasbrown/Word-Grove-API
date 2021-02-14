using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult GetHi()
        {
            return Ok("Hi");
        }
    }
}
