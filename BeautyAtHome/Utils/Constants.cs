using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.Utils
{
    public class Constants
    {

        public static readonly int PAGE_SIZE = 50;

        public static readonly int MAXIMUM_PAGE_SIZE = 250;

        public static readonly string EXPIRES_IN_AN_HOUR = "3600";

        public static class Role
        {
            public const string ADMIN = "CUSTOMER";
        }

        public static class PrefixPolicy
        {
            public const string REQUIRED_ROLE = "RequiredRole";
        }

        public static class TokenClaims
        {
            public const string ROLE = "role";
        }

        public static class AccountStatus
        {
            public const string ACTIVE = "ACTIVE";
        }
    }
}
