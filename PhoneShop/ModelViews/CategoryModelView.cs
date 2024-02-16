using System.ComponentModel.DataAnnotations;

namespace PhoneShop.ModelViews
{
    public class CategoryModelView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }  
        public string Image { get; set; }
    }
}
