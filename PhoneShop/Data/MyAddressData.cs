namespace PhoneShop.Data
{
    public class MyAddressData
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string CityName { get; set; } = string.Empty;


        public string DistrictName { get; set; } = string.Empty;

        public string WardName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int AddressType { get; set; }

        public int IsDefault { get; set; }


        public int IdAccount { get; set; }
    }
}
