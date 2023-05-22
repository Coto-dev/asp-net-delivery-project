using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
	public class DishRatingDTO {

		[Required]
		[Range(0, 5)]
		public double value { get; set;}
	}
}
