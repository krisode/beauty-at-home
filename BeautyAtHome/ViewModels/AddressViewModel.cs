using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class AddressCM
    {
        public string Location { get; set; }
        public int AccountId { get; set; }
        public string Note { get; set; }
        public string LocationName { get; set; }
    }

    public class AddressVM
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int AccountId { get; set; }
        public string Note { get; set; }
        public string LocationName { get; set; }
        public bool IsDefault { get; set; }
    }

    public class AddressSM
    {
        public string Location { get; set; }
        public int AccountId { get; set; }
        public string Note { get; set; }
        public string LocationName { get; set; }
        public bool IsDefault { get; set; }

    }

    public class AddressUM
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int AccountId { get; set; }
        public string Note { get; set; }
        public string LocationName { get; set; }
        public bool IsDefault { get; set; }
    }

    public class AddressPagingSM
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public bool IsDefault { get; set; }
    }
}
