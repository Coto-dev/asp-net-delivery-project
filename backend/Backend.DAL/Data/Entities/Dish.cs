﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Common.Enums;

namespace Backend.DAL.Data.Entities
{
    public class Dish
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsVagetarian { get; set; }
        public string PhotoUrl { get; set; }
        public Categories Category { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public DateTime? DeletedTime { get; set; }

    }
}