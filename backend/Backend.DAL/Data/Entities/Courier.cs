using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DAL.Data.Entities {
    public class Courier {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
