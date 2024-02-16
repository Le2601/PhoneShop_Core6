using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.DI.Category;
using PhoneShop.Models;

namespace PhoneShop.ModelViews
{
    public class CartItemViewModel
    {

        public List<CartItemModel> CartItems { get; set; }

        

        public decimal GrandTotal { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal Profit { get; set; }
    }
}
