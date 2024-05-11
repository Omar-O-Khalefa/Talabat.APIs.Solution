namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Address
    {
        public required string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
