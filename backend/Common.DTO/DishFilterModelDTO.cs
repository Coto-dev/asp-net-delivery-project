using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
	public class DishFilterModelDTO {
        public List<Categories>? Categories { get; set; } = new List<Categories>((Categories[])Enum.GetValues(typeof(Categories)));

        public List<Guid>? MenusId { get; set; }
        public DishSorting Sorting { get; set; } = DishSorting.NameAsc;
        public bool Vegetarian { get; set; } = false;
        [Range(1, int.MaxValue, ErrorMessage = "Неверно указана страница")]
        public int Page { get; set; } = 1;


    }
}
