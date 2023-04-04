using System;
using System.Collections.Generic;

namespace PharmacyLogistics.Entities
{
    public partial class User
    {
        public User()
        {
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }
        public int UserRoleId { get; set; }
        public int? PharmacyId { get; set; }
        public string Patryonomic { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual Pharmacy? Pharmacy { get; set; }
        public virtual Userrole UserRole { get; set; } = null!;
        public virtual ICollection<Request> Requests { get; set; }
    }
}
