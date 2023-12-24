﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MenuWebapi.Models.Data;
#nullable disable
namespace MenuWebapi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230517075900_Mig2")]
    partial class Mig2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");
            modelBuilder.Entity("MenuWebapi.Models.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");
                    b.HasKey("Id");
                    b.ToTable("Categories");
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");
                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");
                    b.Property<string>("Ingredients")
                        .HasColumnType("TEXT");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");
                    b.HasKey("Id");
                    b.HasIndex("CategoryId");
                    b.ToTable("Foods");
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");
                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.Property<string>("Value")
                        .HasColumnType("TEXT");
                    b.HasKey("Id");
                    b.ToTable("Settings");
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Food", b =>
                {
                    b.HasOne("MenuWebapi.Models.Entities.Category", "Category")
                        .WithMany("Foods")
                        .HasForeignKey("CategoryId");
                    b.Navigation("Category");
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Category", b =>
                {
                    b.Navigation("Foods");
                });
#pragma warning restore 612, 618
        }
    }
}
