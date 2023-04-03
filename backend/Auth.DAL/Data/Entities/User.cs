using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.DAL.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Genders Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<UserRole> Roles { get; set; }
    }
}
