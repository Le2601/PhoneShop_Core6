using System.ComponentModel.DataAnnotations;

namespace PhoneShop.ModelViews
{
    public class SpecificationsViewModel
    {

       
        public int Id { get; set; }

       
        public string Model { get; set; }
       
        public string Display { get; set; }
       
        public string OperatingSystem { get; set; }
        
        public string Processor { get; set; }
       
        public string InternalStorage { get; set; }
       
        public string RandomAccessMemory { get; set; }
       
        public string Camera { get; set; }
       
        public string Battery { get; set; }
       
        public string WaterResistance { get; set; }
       
        public string DimensionsAndeight { get; set; }
       
        public string Color { get; set; }
       
        public string Connectivity { get; set; }


        public int ProductId { get; set; }
    }
}
