using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
	public class OrderChangeStatusMessage {
		public string orderId { get; set; }
		public string userId { get; set; }
		public string description { get; set; }
	}
}
