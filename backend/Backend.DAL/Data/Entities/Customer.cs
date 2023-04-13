using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DAL.Data.Entities {
    public class Customer {
        [Key]
        public Guid Id { get; set; } 
        public List<DishInCart> DishInCart { get; set;} = new List<DishInCart>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
