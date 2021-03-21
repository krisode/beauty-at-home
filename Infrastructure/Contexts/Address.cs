using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int AccountId { get; set; }
        public string Note { get; set; }
        public string LocationName { get; set; }
        public bool IsDefault { get; set; }
        public virtual Account Account { get; set; }
    }
}
