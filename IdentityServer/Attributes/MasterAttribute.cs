using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Attributes
{
    public class IsMasterApiAttribute : TypeFilterAttribute
    {
        public IsMasterApiAttribute() : base(typeof(IsMasterApiFilter))
        {
            Arguments = new object[] { new Claim("IsMaster", "True") };
        }
    }

    public class IsMasterApiFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public IsMasterApiFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {


            var hasClaim =ClaimCheck.HasClaim(context , "IsMaster" ,"True");
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
