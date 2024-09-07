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


        public DbSet<ProductQuestions> ProductQuestions { get; set; }


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


        public DbSet<DeliveryProcess> DeliveryProcesses { get; set; }


        //nguoi ban hang


        public DbSet<Booth_Information> Booth_Information { get; set; }

        public DbSet<ShopAddress> ShopAddress { get; set; }

        public DbSet<Shipping_Method> ShopShipping_MethodAddress { get; set; }
        public DbSet<Bank_Account> Bank_Account { get; set; }

        public DbSet<WarehousedProducts> WarehousedProducts { get; set; }
        public DbSet<Booth_Tracking> Booth_Trackings { get; set; }

        public DbSet<Order_ProductPurchasePrice> Order_ProductPurchasePrices { get; set; }


        public DbSet<UserFollows> UserFollows { get; set; }

        public DbSet<PaymentResponse_MoMo> PaymentResponse_MoMos { get; set; }

        public DbSet<FeedBackComment> feedBackComments { get; set; }


        public DbSet<Review_Product> Review_Products { get; set; }


        public DbSet<Delete_Booth> Delete_Booths { get; set; }




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

            modelBuilder.Entity<Order>()
            .Property(p => p.Total_Order)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
             .Property(p => p.Profit)
             .HasColumnType("decimal(18, 2)");


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


            //modelBuilder.Entity<Evaluate_Product>()
            //     .HasOne(e => e.Product)
            //     .WithOne(p => p.Evaluate_Product)
            //     .HasForeignKey<Evaluate_Product>(e => e.ProductId);


          

            modelBuilder.Entity<Order_Details>()
              .HasOne(p => p.Order)
              .WithMany(p => p.Order_Details)
              .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Order_ProductPurchasePrice>()
             .HasOne(p => p.Order_Details)
             .WithMany(p => p.Order_ProductPurchasePrices)
             .HasForeignKey(p => p.OrderDetail_Id);

            modelBuilder.Entity<DeliveryProcess>()
             .HasOne(p => p.Order_Details)
             .WithMany(p => p.DeliveryProcesses)
             .HasForeignKey(p => p.Order_Detail_Id);



            modelBuilder.Entity<Order_Details>()
             .HasOne(p => p.Product)
             .WithMany(p => p.Order_Details)
             .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<PaymentResponse>()
             .HasOne(p => p.Order)
             .WithMany(p => p.PaymentResponses)
             .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<PaymentResponse_MoMo>()
            .HasOne(p => p.Order)
            .WithMany(p => p.PaymentResponse_MoMos)
            .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<ProductQuestions>()
            .HasOne(p => p.Product)
            .WithMany(p => p.ProductQuestions)
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

            //nguoi ban hang

            modelBuilder.Entity<ShopAddress>()
            .HasOne(p => p.Booth_Information)
            .WithMany(p => p.ShopAddresses)
            .HasForeignKey(p => p.BoothId);

            modelBuilder.Entity<Shipping_Method>()
           .HasOne(p => p.Booth_Information)
           .WithMany(p => p.Shipping_Methods)
           .HasForeignKey(p => p.BoothId);

            modelBuilder.Entity<Bank_Account>()
           .HasOne(p => p.Booth_Information)
           .WithMany(p => p.Bank_Accounts)
           .HasForeignKey(p => p.BoothId);

            modelBuilder.Entity<WarehousedProducts>()
          .HasOne(p => p.Product)
          .WithMany(p => p.WarehousedProducts)
          .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Booth_Tracking>()
         .HasOne(p => p.Booth_Information)
         .WithMany(p => p.Booth_Trackings)
         .HasForeignKey(p => p.BoothId);

            modelBuilder.Entity<Voucher>()
              .HasOne(p => p.Booth_Information)
              .WithMany(p => p.Vouchers)
              .HasForeignKey(p => p.BoothId);


            modelBuilder.Entity<Delete_Booth>()
              .HasOne(p => p.Booth_Information)
              .WithMany(p => p.Delete_Booths)
              .HasForeignKey(p => p.BoothId);

            modelBuilder.Entity<Product>()
       .HasOne(p => p.Booth_Information)
       .WithMany(p => p.Products)
       .HasForeignKey(p => p.Booth_InformationId);


            modelBuilder.Entity<UserFollows>()
              .HasOne(p => p.Account)
              .WithMany(p => p.UserFollows)
              .HasForeignKey(p => p.UserID);

            modelBuilder.Entity<UserFollows>()
              .HasOne(p => p.Booth_Information)
              .WithMany(p => p.UserFollows)
              .HasForeignKey(p => p.BoothID);


            modelBuilder.Entity<FeedBackComment>()
              .HasOne(p => p.ProductQuestions)
              .WithMany(p => p.FeedBackComments)
              .HasForeignKey(p => p.RwProductId);

            modelBuilder.Entity<Review_Product>()
              .HasOne(p => p.Product)
              .WithMany(p => p.review_Products)
              .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Review_Product>()
             .HasOne(p => p.Account)
             .WithMany(p => p.review_Products)
             .HasForeignKey(p => p.AccountId);


            modelBuilder.Entity<MyAddress>()
           .HasOne(p => p.Account)
           .WithMany(p => p.myAddresses)
           .HasForeignKey(p => p.IdAccount);





        }


       




        //fluent

        public DbSet<PhoneShop.ModelViews.BannerViewModel>? BannerViewModel { get; set; }




    }
}
