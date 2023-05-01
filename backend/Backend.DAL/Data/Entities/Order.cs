using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAL.Data.Entities {
	public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public double Price { get; set; }
        public string Address { get; set; }
        public Statuses Status { get; set; }
		public Customer Customer { get; set; }
		public Guid? CourId { get; set; }
		public Guid? CookerId { get; set; }

		public List<DishInOrder> Dishes { get; set; }
       

    }
}
