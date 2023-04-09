using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class OrderPagedList {
        public List<OrderDTO> Orders { get; set; }
        public PageInfoDTO PageInfo { get; set; }
    }
}
