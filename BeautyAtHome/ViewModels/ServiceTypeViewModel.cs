using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class ServiceTypeCM
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ServiceTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ServiceVM> services { get; set; }
    }

    public class ServiceTypeSM
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ServiceTypeUM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ServiceTypePagingSM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


}
