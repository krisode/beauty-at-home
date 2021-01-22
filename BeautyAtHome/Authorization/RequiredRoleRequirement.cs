using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BeautyAtHome
{
    public class RequiredRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; set; }

        public RequiredRoleRequirement(string role)
        {
            Role = role;
        }
    }
}
