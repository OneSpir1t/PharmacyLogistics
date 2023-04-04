using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class City
    {
        public City()
        {
            Pharmacies = new HashSet<Pharmacy>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Pharmacy> Pharmacies { get; set; }
    }
}
