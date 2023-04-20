using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
	public class MenuDTO {
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime DeletedTime { get; set; }
		public List<DishDetailsDTO> DishDetails { get; set; } = new List<DishDetailsDTO>();
	}
}
