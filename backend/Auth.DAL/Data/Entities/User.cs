using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.BL.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Genders Gender { get; set; }
        public Courier Courier { get; set; }
        public Manager Manager { get; set; }
        public Cook Cook { get; set; }
        public Customer Customer { get; set; }
        public ICollection<UserRole> Roles { get; set; }
    }
}
