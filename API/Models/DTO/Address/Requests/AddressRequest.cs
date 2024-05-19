namespace API.Models.DTO.Address.Requests
{
    public class AddressRequest
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string ZipCode { get; set; }
    }
}
