using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
	public class AddUserViewModel {
		public Guid restarauntId { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
	}
}
