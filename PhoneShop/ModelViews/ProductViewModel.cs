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
      
        public string Title { get; set; }
      
        public string Alias { get; set; }

      

        public decimal Price { get; set; }
       

        public decimal Discount { get; set; }
        
        public int Quantity { get; set; }


        public string ImageProductDefault { get; set; }


        public string Description { get; set; }

        public int CategoryId { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public virtual ICollection<ImageProduct> ImageProducts { get; set; }
    }
}
