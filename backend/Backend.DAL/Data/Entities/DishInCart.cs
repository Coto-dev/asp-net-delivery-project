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
        public List<Dish> DishesCart { get; set; } = new List<Dish>();   
        public Customer Customer { get; set; }
    }
}
