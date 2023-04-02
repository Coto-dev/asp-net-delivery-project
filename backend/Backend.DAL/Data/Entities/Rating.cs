using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DAL.Data.Entities
{

    public class Rating
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Dish")]
        public Guid DishID { get; set; }
        public Dish Dish { get; set; }
        public double Value { get; set; }
        public DateTime? DeletedTime { get; set; }


    }
}
