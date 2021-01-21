using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class ServiceInCombo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ServiceComboId { get; set; }
        public long ServiceId { get; set; }
        public int Quantity { get; set; }

        public virtual Service Service { get; set; }
        public virtual Service ServiceCombo { get; set; }
    }
}
