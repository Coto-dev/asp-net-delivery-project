using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Common.Enums;

namespace Backend.DAL.Data.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public double Price { get; set; }
        public string Address { get; set; }
        public Statuses Status { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Courier")]
        public Guid? CourierId { get; set; }
        public Courier? Courier { get; set; }
        [ForeignKey("Cook")]
        public Guid CookId { get; set; }
        public Cook Cook { get; set; }
       

    }
}
