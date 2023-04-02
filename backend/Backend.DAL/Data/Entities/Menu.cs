using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DAL.Data.Entities
{
    public class Menu
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        [ForeignKey("Restaraunt")]
        public Guid RestarauntId { get; set; }
        public Restaraunt Restaraunt { get; set; }
        public DateTime? DeletedTime { get; set; }

    }
}
