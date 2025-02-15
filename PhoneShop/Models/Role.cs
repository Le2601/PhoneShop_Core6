﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace PhoneShop.Models
{
    [Table("Role")]
    public class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }


    }
}
