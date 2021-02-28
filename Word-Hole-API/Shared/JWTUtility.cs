using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Word_Hole_API.Shared
{
    public static class JWTUtility
    {
        public static int GetUserID(HttpContext httpContext)
        {
            return int.Parse(httpContext.User.Claims.Single(c => c.Type == "UserID").Value);
        }

        public static RoleType GetRole(HttpContext httpContext)
        {
            var roleString = httpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value.ToUpper();

            if (roleString == "ADMIN")
                return RoleType.Admin;
            else
                return RoleType.User;
        }
    }
}
