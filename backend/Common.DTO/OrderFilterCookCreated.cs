using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
    public class OrderFilterCookCreated {
        public int Page { get; set; } = 1;
        public string OrderNumber { get; set; }
        /// <summary>
        /// Дата создания заказа
        /// </summary>
        public DateSorting? OrderDate { get; set; }
        /// <summary>
        /// Дата к которой будет доставлен заказ
        /// </summary>
        public DateSorting? DeliveryDate { get; set; }
    }
}
