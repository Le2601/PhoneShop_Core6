﻿using PhoneShop.Models;
using System.ComponentModel.DataAnnotations;

namespace PhoneShop.ModelViews
{
    public class Product_ReviewModelView
    {
        
        [Key]

        public int Id { get; set; }

        public int ProductId { get; set; }


        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string Content { get; set; }

        public double Rate { get; set; }

        public DateTime CreateAt { get; set; }


        public virtual ProductViewModel ProductViewModel { get; set; }

    }
}
