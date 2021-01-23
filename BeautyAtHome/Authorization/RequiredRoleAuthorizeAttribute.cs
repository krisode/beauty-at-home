using Microsoft.AspNetCore.Authorization;
using static BeautyAtHome.Utils.Constants;

namespace BeautyAtHome.Authorization
{
    public class RequiredRoleAuthorizeAttribute : AuthorizeAttribute
    {
        public RequiredRoleAuthorizeAttribute(string role)
        {
            Role = role;
        }

        public string Role
        {
            get
            {
                return Policy.Substring(PrefixPolicy.REQUIRED_ROLE.Length);
            }
            set
            {
                Policy = $"{PrefixPolicy.REQUIRED_ROLE}{value}";
            }
        }
    }
}
