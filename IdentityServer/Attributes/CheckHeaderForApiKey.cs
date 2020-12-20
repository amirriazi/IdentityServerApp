using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace IdentityServer.Attributes
{
    public class CheckHeaderForApiKeyAttribute : TypeFilterAttribute
    {
        public CheckHeaderForApiKeyAttribute(): base(typeof(CheckHeaderForApiKeyAttribute))
        {
                
        }
        public class CheckHeaderForApiKeyFilter : IAuthorizationFilter
        {
            
            public CheckHeaderForApiKeyFilter()
            {
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var headers = context.HttpContext.Request.Headers;
                //Validate if any permissions are passed when using attribute at controller or action level
                if (!headers.ContainsKey("api-key"))
                {
                    //Validation cannot take place without any permissions so returning unauthorized
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }


    }
}

// www.craftedforeveryone.com
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  



