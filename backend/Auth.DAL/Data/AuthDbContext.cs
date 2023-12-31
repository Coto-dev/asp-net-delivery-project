﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Auth.DAL.Data.Entities;

namespace Auth.DAL.Data {
    public class AuthDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public  DbSet<Customer> Customers { get; set; }
        public DbSet<Cook> Cooks { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasOne(x => x.Cook)
                .WithOne(x => x.User)
                .HasForeignKey<Cook>().IsRequired();
            builder.Entity<User>()
                .HasOne(x => x.Courier)
                .WithOne(x => x.User)
                .HasForeignKey<Courier>().IsRequired();
            builder.Entity<User>()
               .HasOne(x => x.Manager)
               .WithOne(x => x.User)
               .HasForeignKey<Manager>().IsRequired();
            builder.Entity<User>()
               .HasOne(x => x.Customer)
               .WithOne(x => x.User)
               .HasForeignKey<Customer>().IsRequired();

            builder.Entity<User>(o => {
                o.ToTable("Users");
            });



            builder.Entity<Role>(o => {
                o.ToTable("Roles");
            });
            builder.Entity<UserRole>(o => {
                o.ToTable("UserRoles");
                o.HasOne(x => x.Role)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
                o.HasOne(x => x.User)
                    .WithMany(x => x.Roles)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
