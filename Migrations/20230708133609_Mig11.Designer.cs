﻿// <auto-generated />
using System;
using MenuWebapi.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
#nullable disable
namespace MenuWebapi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230708133609_Mig11")]
    partial class Mig11
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");
            modelBuilder.Entity("MenuWebapi.Models.Auth.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");
                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");
                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");
                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");
                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");
                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");
                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");
                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");
                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");
                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");
                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");
                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");
                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");
                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");
                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");
                    b.HasKey("Id");
                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");
                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");
                    b.ToTable("AspNetUsers", (string)null);
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");
                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");
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
                    b.Property<float>("Price")
                        .HasColumnType("REAL");
                    b.HasKey("Id");
                    b.HasIndex("CategoryId");
                    b.ToTable("Foods");
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.OrderState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");
                    b.Property<string>("CssBadgeClass")
                        .HasColumnType("TEXT");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.HasKey("Id");
                    b.ToTable("OrderStates");
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");
                    b.Property<int?>("OrderCreatedStateId")
                        .HasColumnType("INTEGER");
                    b.Property<int?>("OrderPaidStateId")
                        .HasColumnType("INTEGER");
                    b.Property<float?>("ShippingCosts")
                        .HasColumnType("REAL");
                    b.Property<string>("SiteName")
                        .HasColumnType("TEXT");
                    b.Property<string>("SiteSubtitle")
                        .HasColumnType("TEXT");
                    b.HasKey("Id");
                    b.HasIndex("OrderCreatedStateId");
                    b.HasIndex("OrderPaidStateId");
                    b.ToTable("Settings");
                });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");
                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");
                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");
                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.HasKey("Id");
                    b.HasIndex("UserId");
                    b.ToTable("AspNetUserClaims", (string)null);
                });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");
                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");
                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");
                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");
                    b.HasKey("LoginProvider", "ProviderKey");
                    b.HasIndex("UserId");
                    b.ToTable("AspNetUserLogins", (string)null);
                });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");
                    b.Property<string>("Value")
                        .HasColumnType("TEXT");
                    b.HasKey("UserId", "LoginProvider", "Name");
                    b.ToTable("AspNetUserTokens", (string)null);
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Food", b =>
                {
                    b.HasOne("MenuWebapi.Models.Entities.Category", "Category")
                        .WithMany("Foods")
                        .HasForeignKey("CategoryId");
                    b.Navigation("Category");
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Setting", b =>
                {
                    b.HasOne("MenuWebapi.Models.Entities.OrderState", "OrderCreatedState")
                        .WithMany()
                        .HasForeignKey("OrderCreatedStateId");
                    b.HasOne("MenuWebapi.Models.Entities.OrderState", "OrderPaidState")
                        .WithMany()
                        .HasForeignKey("OrderPaidStateId");
                    b.Navigation("OrderCreatedState");
                    b.Navigation("OrderPaidState");
                });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MenuWebapi.Models.Auth.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MenuWebapi.Models.Auth.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MenuWebapi.Models.Auth.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
            modelBuilder.Entity("MenuWebapi.Models.Entities.Category", b =>
                {
                    b.Navigation("Foods");
                });
#pragma warning restore 612, 618
        }
    }
}
