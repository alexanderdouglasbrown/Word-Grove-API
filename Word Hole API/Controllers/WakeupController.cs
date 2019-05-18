using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WakeupController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Good morning!";
        }
    }
}