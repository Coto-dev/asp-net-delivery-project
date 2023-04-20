using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
	public class DishModelDTO {
		public string Name { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public bool IsVagetarian { get; set; }
		public string PhotoUrl { get; set; }
		public Categories Category { get; set; }
	}
}
