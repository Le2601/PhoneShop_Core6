using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Areas.Admin.Data
{
    public class SpecificationsData
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Model { get; set; }
        [Display(Name = "Màn hình")]
        public string Display { get; set; }
        [Display(Name = "Hệ điều hành")]
        public string OperatingSystem { get; set; }
        [Display(Name = "CPU")]
        public string Processor { get; set; }
        [Display(Name = "Bộ nhớ trong")]
        public string InternalStorage { get; set; }
        [Display(Name = "RAM")]
        public string RandomAccessMemory { get; set; }
        [Display(Name = "Camera")]
        public string Camera { get; set; }
        [Display(Name = "Pin")]
        public string Battery { get; set; }
        [Display(Name = "Chống nước")]
        public string WaterResistance { get; set; }
        [Display(Name = "Kích thước và Trọng lượng")]
        public string DimensionsAndeight { get; set; }
        [Display(Name = "Màu sắc")]
        public string Color { get; set; }
        [Display(Name = "Kết nối")]
        public string Connectivity { get; set; }

        public int ProductId { get; set; }

    }
}
