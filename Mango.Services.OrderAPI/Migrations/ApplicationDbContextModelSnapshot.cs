﻿// <auto-generated />
using System;
using Mango.Services.OrderAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Mango.Services.OrderAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.14");

            modelBuilder.Entity("Mango.Services.OrderAPI.Models.OrderDetails", b =>
                {
                    b.Property<int>("OrderDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderHeaderId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OrderDetailsId");

                    b.HasIndex("OrderHeaderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Mango.Services.OrderAPI.Models.OrderHeader", b =>
                {
                    b.Property<int>("OrderHeaderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CartTotalItems")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("DiscountTotal")
                        .HasColumnType("REAL");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExpiryMonthYear")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FisrtName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("TEXT");

                    b.Property<double>("OrderTotl")
                        .HasColumnType("REAL");

                    b.Property<bool>("PaymentStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PickupDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OrderHeaderId");

                    b.ToTable("OrderHeaders");
                });

            modelBuilder.Entity("Mango.Services.OrderAPI.Models.OrderDetails", b =>
                {
                    b.HasOne("Mango.Services.OrderAPI.Models.OrderHeader", "OrderHeader")
                        .WithMany("PrderDetails")
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderHeader");
                });

            modelBuilder.Entity("Mango.Services.OrderAPI.Models.OrderHeader", b =>
                {
                    b.Navigation("PrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
