﻿using System.ComponentModel.DataAnnotations;
using System;
using PhoneShop.Models;
using System.Collections.Generic;

namespace PhoneShop.ModelViews
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            this.ImageProducts = new HashSet<ImageProduct>();

        }
      
        public int Id { get; set; }

        public int BoothId { get; set; }
      
        public string Title { get; set; }
      
        public string Alias { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsApproved { get; set; }
        public Boolean IsOutstanding { get; set; }


        public Boolean IsOld { get; set; }

        public decimal Price { get; set; }
        public decimal InputPrice { get; set; }

        public decimal Discount { get; set; }
        
        public int Quantity { get; set; }


        public string ImageDefaultName { get; set; }


        public string Description { get; set; }

        public int CategoryId { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public int? Quantity_Purchase { get; set; }

        public double Rating { get; set; }

        public int Create_Id { get; set; }

        public bool Status { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Evaluate_ProductViewModel> Evaluate_Product { get; set; }


        public virtual ICollection<ImageProduct> ImageProducts { get; set; }


        public virtual ICollection<Review_Product> Review_Product { get; set; }


        public virtual ICollection<Order_DetailsViewModel> Order_Details { get; set; }

    }
}
