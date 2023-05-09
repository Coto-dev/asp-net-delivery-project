using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
	public class OrderDTO {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public Statuses Status { get; set; }
        /// <summary>
        /// Дата к которой будетдоставлен
        /// </summary>
        public DateTime DeliveryTime { get; set; }
        /// <summary>
        /// Дата создания заказа
        /// </summary>
        public DateTime OrderTime { get; set;}
        public int Price { get; set; }
        public string Address { get; set; }
        public List<DishShortModelDTO> Dishes { get; set; } = new List<DishShortModelDTO>();

	}
}
