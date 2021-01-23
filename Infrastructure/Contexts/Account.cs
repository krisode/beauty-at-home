using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Account
    {
        public Account()
        {
            Addresses = new HashSet<Address>();
            BookingCustomerAccounts = new HashSet<Booking>();
            BookingSalonMemberAccounts = new HashSet<Booking>();
        }

        public string Email { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public long SalonId { get; set; }
        public long? GalleryId { get; set; }

        public virtual Gallery Gallery { get; set; }
        public virtual Salon Salon { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Booking> BookingCustomerAccounts { get; set; }
        public virtual ICollection<Booking> BookingSalonMemberAccounts { get; set; }
    }
}
