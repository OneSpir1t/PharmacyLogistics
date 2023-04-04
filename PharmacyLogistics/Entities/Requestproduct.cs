using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Requestproduct
    {
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Request Request { get; set; } = null!;
    }
}
