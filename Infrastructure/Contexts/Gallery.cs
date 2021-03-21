using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Gallery
    {
        public Gallery()
        {
            Accounts = new HashSet<Account>();
            FeedBacks = new HashSet<FeedBack>();
            Images = new HashSet<Image>();
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
