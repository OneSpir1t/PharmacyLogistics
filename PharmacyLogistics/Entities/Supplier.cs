using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public int Inn { get; set; }
        public string Name { get; set; } = null!;

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
