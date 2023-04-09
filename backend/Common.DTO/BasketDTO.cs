using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class BasketDTO {
        Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public int amount { get; set; }
        public string PhotoUrl { get; set; }
    }
}
