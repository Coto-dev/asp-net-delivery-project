using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.Data.Entities {
    public class Customer : User {
        public string Address { get; set; }
    }
}
