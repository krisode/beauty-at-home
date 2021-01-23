using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class AuthCM
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }

    public class AuthVM
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string DisplayName { get; set; }
    }


}
