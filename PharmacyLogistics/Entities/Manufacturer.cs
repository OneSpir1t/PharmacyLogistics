using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class Manufacturer
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Country Country { get; set; } = null!;
    }
}
