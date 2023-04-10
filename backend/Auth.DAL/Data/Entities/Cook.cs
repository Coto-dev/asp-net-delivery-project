using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.Data.Entities {
    public class Cook {
        public Guid Id { get; set; }
        public User User { get; set; }
    }
}
