using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
    public class OrderFilterCustomer {
        public int Page { get; set; } = 1; 
        public int OrderNumber { get; set; }
        /// <summary>
        /// Дата заказа
        /// </summary>
        public DateSorting OrderDate { get; set; }
    }
}
