using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Common.Enums;
using Common.Enums;

namespace Common.DTO {
    public class DishFilterModelDTO {
        public List<Categories>? Categories { get; set; } = new List<Categories>();
        public DishSorting sorting { get; set; }
        public bool Vegetarian { get; set; } = false;
        public int Page { get; set; } = 1;


    }
}
