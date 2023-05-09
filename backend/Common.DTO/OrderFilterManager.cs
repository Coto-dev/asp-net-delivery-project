using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
	public class OrderFilterManager {
        public int Page { get; set; } = 1;
        public string? OrderNumber { get; set; }
        public List<Statuses> Statuses { get; set; }
		/// <summary>
		/// Выбор по какой дате сортировать
		/// </summary>
		public DateSorting? SortingDate { get; set; }
	}
}
