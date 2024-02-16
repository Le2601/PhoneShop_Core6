using System;

namespace PhoneShop.ModelViews
{
    public class RegisterAccountViewModel
    {
       

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Salt { get; set; }
       
        public string FullName { get; set; }
        public int? RoleId { get; set; }
    }
}
