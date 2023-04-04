using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Pharmacyproduct
    {
        public int PharmacyId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateOnly Expirydate { get; set; }

        public virtual Pharmacy Pharmacy { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
