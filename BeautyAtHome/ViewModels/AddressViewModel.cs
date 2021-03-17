using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class AddressCM
    {
        public int AccountId { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
    }

    public class AddressUM
    {

        public string Location { get; set; }
        public string LocationName { get; set; }
        public string Note { get; set; }


    }

    public class AddressVM
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public string Note { get; set; }

    }

   
}
