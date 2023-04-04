using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Pharmacy
    {
        public Pharmacy()
        {
            Pharmacyproducts = new HashSet<Pharmacyproduct>();
            Requests = new HashSet<Request>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int CityId { get; set; }
        public string? Address { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual ICollection<Pharmacyproduct> Pharmacyproducts { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
