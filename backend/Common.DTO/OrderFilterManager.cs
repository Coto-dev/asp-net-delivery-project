﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Common.Enums;
using Common.Enums;

namespace Common.DTO {
    public class OrderFilterManager {
        public int Page { get; set; } = 1;
        public string OrderNumber { get; set; }
        public List<Statuses> Statuses { get; set; }
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