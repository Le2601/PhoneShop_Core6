using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stripe.Radar;
using PhoneShop.ModelViews;
using Stripe;

namespace PhoneShop.Models
{
    public class ShopPhoneDbContext : DbContext
    {
        public ShopPhoneDbContext(DbContextOptions<ShopPhoneDbContext> options) : base(options)
        {

           
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<ImageProduct> ImageProducts { get; set; }

        public DbSet<Banner> BannerProducts { get; set; }

        public DbSet<Order> Orders { get; set; }


        public DbSet<Order_Details> Order_Details { get; set; }


        public DbSet<PaymentResponse> paymentResponses { get; set; }

        public DbSet<specifications> specifications { get; set; }


        public DbSet<Product_Review> product_Reviews { get; set; }


        public DbSet<SupportDirectory> Support_Directories { get; set; }

        public DbSet<SupportContent> Support_Contents { get; set; }


        public DbSet<Voucher> Vouchers { get; set; }


        //new update

        public DbSet<Introduce> Introduces { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<MyAddress> MyAddresses { get; set; }



        public DbSet<Evaluate_Product> Evaluate_Products { get; set; }









        //fluent

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity => {

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Title).HasMaxLength(100);



            });

            modelBuilder.Entity<Product>()
                .Property(p => p.Discount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Product>()
               .Property(p => p.Price)
               .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Product>()
              .Property(p => p.InputPrice)
              .HasColumnType("decimal(18, 2)");


            modelBuilder.Entity<Voucher>()
              .Property(p => p.DiscountAmount)
              .HasColumnType("decimal(18, 0)");

            //modelBuilder.Entity<Voucher>()
            // .Property(p => p.DiscountConditions)
            // .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Voucher>()
            .Property(p => p.DiscountConditions)
            .HasColumnType("decimal(18, 0)");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Product)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Account>()
               .HasOne(p => p.Role)
               .WithMany(p => p.Accounts)
               .HasForeignKey(p => p.RoleId);

            modelBuilder.Entity<ImageProduct>()
              .HasOne(p => p.Product)
              .WithMany(p => p.ImageProducts)
              .HasForeignKey(p => p.ProductId);

            //modelBuilder.Entity<Banner>()
            //  .HasOne(p => p.Product)
            //  .WithMany(p => p.Banners)
            //  .HasForeignKey(p => p.ProductId);


            modelBuilder.Entity<specifications>()
           .HasOne(p => p.Product)
           .WithMany(p => p.Specifications)
           .HasForeignKey(p => p.ProductId);


            modelBuilder.Entity<Evaluate_Product>()
                 .HasOne(e => e.Product)
                 .WithOne(p => p.Evaluate_Product)
                 .HasForeignKey<Evaluate_Product>(e => e.ProductId);


            modelBuilder.Entity<Order>()
              .Property(p => p.Total_Order)
              .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
             .Property(p => p.Profit)
             .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order_Details>()
              .HasOne(p => p.Order)
              .WithMany(p => p.Order_Details)
              .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Order_Details>()
             .HasOne(p => p.Product)
             .WithMany(p => p.Order_Details)
             .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<PaymentResponse>()
             .HasOne(p => p.Order)
             .WithMany(p => p.PaymentResponses)
             .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Product_Review>()
            .HasOne(p => p.Product)
            .WithMany(p => p.Product_Reviews)
            .HasForeignKey(p => p.ProductId);


            modelBuilder.Entity<SupportContent>()
           .HasOne(p => p.SupportDirectory)
           .WithMany(p => p.SupportContents)
           .HasForeignKey(p => p.IdSpDirectory);


            modelBuilder.Entity<District>()
             .HasOne(p => p.City)
             .WithMany(p => p.Districts)
             .HasForeignKey(p => p.IdCity);


            modelBuilder.Entity<Ward>()
             .HasOne(p => p.District)
             .WithMany(p => p.Wards)
             .HasForeignKey(p => p.IdDistrict);


        }


       




        //fluent

        public DbSet<PhoneShop.ModelViews.BannerViewModel>? BannerViewModel { get; set; }




    }
}
