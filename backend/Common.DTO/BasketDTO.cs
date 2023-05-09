using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
	public class BasketDTO {
		public List<DishShortModelDTO> Dishes { get; set; }
		public int BasketPrice { get; set;}
	}
}
