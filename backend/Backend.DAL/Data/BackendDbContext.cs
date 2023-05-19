using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Backend.DAL.Data {
    public class BackendDbContext : DbContext {

        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) { }

        public virtual DbSet<Dish> Dishes { get; set; }
		public virtual DbSet<DishInOrder> OrderDishes { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Restaraunt> Restaraunts { get; set; }
        public virtual DbSet<DishInCart> CartDishes { get; set; }
    }


}
