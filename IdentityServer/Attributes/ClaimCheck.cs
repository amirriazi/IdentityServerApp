using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Attributes
{
    public static class ClaimCheck
    {
        public static bool HasClaim(AuthorizationFilterContext context, string claimType, string claimValue)
        {
            return context.HttpContext.User.Claims.Any(c => c.Type == claimType  && c.Value == claimValue);
        }
    }
}
