﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("ProductQuestions")]
    public class ProductQuestions
    {
        [Key]       
        
        public int Id { get; set; }

        public int ProductId { get; set; }


        public string UserName { get; set; }

        public string UserEmail { get; set; } 

        public string  Content { get; set; }

       

        public DateTime CreateAt { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<FeedBackComment> FeedBackComments { get; set; }


    }
}
