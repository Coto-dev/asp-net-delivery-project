using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DAL.Data.Entities {

    public class Manager {
        [Key]
        public Guid Id { get; set; } 

        public Restaraunt Restaraunt { get; set; }
    }
}
