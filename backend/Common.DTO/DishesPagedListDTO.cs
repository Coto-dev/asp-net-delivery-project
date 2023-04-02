using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class DishesPagedListDTO {
        List<DishDetailsDTO> Dishes { get; set; }
        /// <summary>
        /// pagination info
        /// </summary>
        public PageInfoDTO PageInfo { get; set; }
    }
}
