using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Product
    {
        public Product()
        {
            Pharmacyproducts = new HashSet<Pharmacyproduct>();
            Requestproducts = new HashSet<Requestproduct>();
        }

        public int Id { get; set; }
        public int ReleaseFormId { get; set; }
        public int SupplierId { get; set; }
        public string Article { get; set; } = null!;
        public string? Dose { get; set; }
        public int Cost { get; set; }
        public int Quantityinthepackage { get; set; }
        public string Name { get; set; } = null!;

        public virtual Releaseform ReleaseForm { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<Pharmacyproduct> Pharmacyproducts { get; set; }
        public virtual ICollection<Requestproduct> Requestproducts { get; set; }
    }
}
