using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DAL.Data.Entities {
    public class DishInCart {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Count { get; set; }
        public Dish Dish  { get; set; }
        public Customer Customer { get; set; }
    }
}
