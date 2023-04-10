using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class DishesPagedListDTO {
        public  List<DishDetailsDTO> Dishes { get; set; }
        /// <summary>
        /// pagination info
        /// </summary>
        public PageInfoDTO PageInfo { get; set; }
    }
}
