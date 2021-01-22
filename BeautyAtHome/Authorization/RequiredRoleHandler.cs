using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static BeautyAtHome.Utils.Constants;

namespace BeautyAtHome
{
    public class RequiredRoleHandler : AuthorizationHandler<RequiredRoleRequirement>
    {
        IHttpContextAccessor _httpContextAccessor = null;
        public RequiredRoleHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, RequiredRoleRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            string jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;

            try
            {
                var response = await auth.VerifyIdTokenAsync(jwtToken);
                string role = (string) response.Claims.GetValueOrDefault(TokenClaims.ROLE, null);
                if (role.Equals(requirement.Role))
                {
                    context.Succeed(requirement);
                }
            }
            catch (FirebaseAdmin.FirebaseException)
            {
            }
            return Task.CompletedTask;
        }
    }
}
