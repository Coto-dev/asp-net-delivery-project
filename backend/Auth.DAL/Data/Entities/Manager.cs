using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.Data.Entities {
    public class Manager {
        public Guid Id { get; set; }
        public User User { get; set; }

    }
}
