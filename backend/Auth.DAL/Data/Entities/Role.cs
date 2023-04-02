using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.DAL.Data.Entities {
    public class Role : IdentityRole<Guid> {
        public RoleType Type { get; set; }
        public ICollection<UserRole> Users { get; set; }
    }
}
