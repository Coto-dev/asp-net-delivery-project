using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
	public class DishModelDTO {
		[Required]
		[MinLength(1)]
		public string Name { get; set; }
		[Required]
		public int Price { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public bool IsVagetarian { get; set; }
		[Required]
		public string PhotoUrl { get; set; }
		[Required]
		public Categories Category { get; set; }
	}
}
