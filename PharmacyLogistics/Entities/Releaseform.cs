using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Releaseform
    {
        public Releaseform()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
