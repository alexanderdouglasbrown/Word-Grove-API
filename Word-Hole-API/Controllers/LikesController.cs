using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models.DB;
using Word_Hole_API.Models.Like;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly WordHoleDBContext _context;

        public LikesController(WordHoleDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPostLikes([FromQuery] LikesGet parameters)
        {
            var userID = GetNullableUserID();

            var likesQuery = from likes in _context.Likes
                             where likes.Postid == parameters.PostID
                             select likes;

            var totalLikes = likesQuery.Count();
            var isUserLiked = GetUserLikedPost(userID, likesQuery);

            return Ok(new { isUserLiked, totalLikes });
        }

        private int? GetNullableUserID()
        {
            var userIDRaw = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID");
            int? result = null;

            if (userIDRaw != null)
                result = int.Parse(userIDRaw.Value);

            return result;
        }

        private bool GetUserLikedPost(int? userID, IQueryable<Likes> likesQuery)
        {
            if (!userID.HasValue)
                return false;

            var getLike = (from likes in likesQuery
                           where likes.Userid == userID
                           select likes).FirstOrDefault();

            return getLike != null;
        }

        [Authorize]
        [HttpPut]
        public IActionResult AddLike(LikesPut parameters)
        {
            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);

            var queryCheckAlreadyLiked = (from likes in _context.Likes
                                          where likes.Userid == userID
                                          && likes.Postid == parameters.PostID
                                          select likes).FirstOrDefault();

            if (queryCheckAlreadyLiked != null)
                return Ok(); // Ignore. No need to throw error.

            var newLike = new Likes()
            {
                Userid = userID,
                Postid = parameters.PostID
            };

            _context.Likes.Add(newLike);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult RemoveLike(LikesDelete parameters)
        {
            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);

            var like = (from likes in _context.Likes
                             where likes.Userid == userID
                             && likes.Postid == parameters.PostID
                             select likes).FirstOrDefault();

            if (like == null)
                return Ok(); // Ignore. Probably a UI problem.

            _context.Likes.Remove(like);
            _context.SaveChanges();

            return Ok();
        }
    }
}