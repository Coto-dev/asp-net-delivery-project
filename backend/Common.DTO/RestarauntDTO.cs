using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class RestarauntDTO {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Description { get; set; }
		public string? Address { get; set; }

	}
}
