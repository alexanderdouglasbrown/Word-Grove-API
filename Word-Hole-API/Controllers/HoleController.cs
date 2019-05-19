using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoleController : ControllerBase
    {
        [HttpGet]
        public object Get()
        {
            using (var context = new WordHoleDBContext())
            {
                var query = (from posts in context.Posts
                             orderby posts.Createdon descending
                             select posts);

                var output = new List<object>();

                foreach (var post in query)
                {
                    output.Add(new
                    {
                        id = post.Id,
                        message = post.Message
                    });
                }

                return output;
            }
        }
    }
}