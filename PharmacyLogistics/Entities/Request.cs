using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Request
    {
        public Request()
        {
            Requestproducts = new HashSet<Requestproduct>();
        }

        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public DateOnly DateOfRequest { get; set; }
        public int StatusId { get; set; }
        public int? UserId { get; set; }

        public virtual Pharmacy Pharmacy { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual User? User { get; set; }
        public virtual ICollection<Requestproduct> Requestproducts { get; set; }
    }
}
