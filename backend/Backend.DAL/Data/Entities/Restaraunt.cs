using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAL.Data.Entities
{
    public class Restaraunt
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public List<Cook> Cooks { get; set; }
        public List<Manager> Managers { get; set; } 
        public DateTime? DeletedTime { get; set; }

    }
    

}
