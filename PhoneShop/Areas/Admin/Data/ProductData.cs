﻿using PhoneShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Areas.Admin.Data
{
    public class ProductData
    {
        //public ProductData()
        //{
        //    this.ImageProducts = new HashSet<ImageProduct>();

        //}
        [Key]
        public int Id { get; set; }

        public Boolean IsActive { get; set; }


        public Boolean IsApproved { get; set; }
        public Boolean IsOld { get; set; }


        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "Không thể bỏ trống")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Không thể bỏ trống")]
        [Display(Name = "Giá nhập")]

        public decimal InputPrice { get; set; }

        [Required(ErrorMessage = "Không thể bỏ trống")]
        [Display(Name = "Giá tiền")]

        public decimal Price { get; set; }
        [Display(Name = "Giảm giá")]
        //[Required(ErrorMessage = "Không thể bỏ trống")]

        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public int Quantity { get; set; }


        public string ImageDefaultName { get; set; }


        public string Description { get; set; }

        public int CategoryId { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public int Create_Id { get; set; }

        public int Booth_InformationId { get; set; }


        //public virtual Category Category { get; set; }


        //public virtual ICollection<ImageProduct> ImageProducts { get; set; }

        //public virtual ICollection<Banner> Banners { get; set; }

       

        //public virtual ICollection<specifications> Specifications { get; set; }

        //public virtual ICollection<Product_Review> Product_Reviews { get; set; }

    }
}
