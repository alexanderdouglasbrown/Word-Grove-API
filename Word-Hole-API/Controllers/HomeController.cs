using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models.DB;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly WordHoleDBContext _context;
        public HomeController(WordHoleDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public object Get()
        {
            var query = (from posts in _context.Posts
                         orderby posts.Createdon descending
                         select posts);

            var output = new List<object>();

            foreach (var post in query)
            {
                output.Add(new
                {
                    id = post.Id,
                    message = post.Post
                });
            }

            return output;
        }
    }
}