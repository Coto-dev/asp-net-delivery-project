using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class OrderFilterCook {
        public int Page { get; set; } = 1;
        public string OrderNumber { get; set; }
    }
}
