﻿// <auto-generated />
using System;
using Backend.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.DAL.Migrations
{
    [DbContext(typeof(BackendDbContext))]
    [Migration("20230420094340_changeOrder")]
    partial class changeOrder
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.DAL.Data.Entities.Cook", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RestarauntId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RestarauntId");

                    b.ToTable("Cook");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVagetarian")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.DishInCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DishId");

                    b.ToTable("CartDishes");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Manager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RestarauntId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RestarauntId");

                    b.ToTable("Manager");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RestarauntId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RestarauntId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DeliveryTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CookId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DishID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DishID");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Restaraunt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Restaraunts");
                });

            modelBuilder.Entity("DishMenu", b =>
                {
                    b.Property<Guid>("DishesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MenusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DishesId", "MenusId");

                    b.HasIndex("MenusId");

                    b.ToTable("DishMenu");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Cook", b =>
                {
                    b.HasOne("Backend.DAL.Data.Entities.Restaraunt", "Restaraunt")
                        .WithMany("Cooks")
                        .HasForeignKey("RestarauntId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaraunt");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.DishInCart", b =>
                {
                    b.HasOne("Backend.DAL.Data.Entities.Customer", "Customer")
                        .WithMany("DishInCart")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.DAL.Data.Entities.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Manager", b =>
                {
                    b.HasOne("Backend.DAL.Data.Entities.Restaraunt", "Restaraunt")
                        .WithMany("Managers")
                        .HasForeignKey("RestarauntId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaraunt");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Menu", b =>
                {
                    b.HasOne("Backend.DAL.Data.Entities.Restaraunt", "Restaraunt")
                        .WithMany("Menus")
                        .HasForeignKey("RestarauntId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaraunt");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Order", b =>
                {
                    b.HasOne("Backend.DAL.Data.Entities.Cook", "Cook")
                        .WithMany()
                        .HasForeignKey("CookId");

                    b.HasOne("Backend.DAL.Data.Entities.Customer", null)
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cook");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Rating", b =>
                {
                    b.HasOne("Backend.DAL.Data.Entities.Customer", "Customer")
                        .WithMany("Ratings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.DAL.Data.Entities.Dish", "Dish")
                        .WithMany("Ratings")
                        .HasForeignKey("DishID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("DishMenu", b =>
                {
                    b.HasOne("Backend.DAL.Data.Entities.Dish", null)
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.DAL.Data.Entities.Menu", null)
                        .WithMany()
                        .HasForeignKey("MenusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Customer", b =>
                {
                    b.Navigation("DishInCart");

                    b.Navigation("Orders");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Dish", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("Backend.DAL.Data.Entities.Restaraunt", b =>
                {
                    b.Navigation("Cooks");

                    b.Navigation("Managers");

                    b.Navigation("Menus");
                });
#pragma warning restore 612, 618
        }
    }
}
