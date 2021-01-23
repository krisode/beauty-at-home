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
        public string AccessToken { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Address> Addresses{ get; set; }
    }


}
