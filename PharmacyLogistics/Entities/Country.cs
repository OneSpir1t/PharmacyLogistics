using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Country
    {
        public Country()
        {
            Manufacturers = new HashSet<Manufacturer>();
            Suppliers = new HashSet<Supplier>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Manufacturer> Manufacturers { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
