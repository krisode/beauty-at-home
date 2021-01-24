using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class ServiceInCombo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ServiceComboId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }

        public virtual Service Service { get; set; }
        public virtual Service ServiceCombo { get; set; }
    }
}
