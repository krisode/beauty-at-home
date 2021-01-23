using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class AccountInSalon
    {
        public int Id { get; set; }
        public int SalonOwnerId { get; set; }
        public int MemberId { get; set; }

        public virtual Account Member { get; set; }
        public virtual Account SalonOwner { get; set; }
    }
}
