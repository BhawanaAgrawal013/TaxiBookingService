namespace TaxiBookingServiceUI.Models
{
    public class BookingRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Pickup Location")]
        [StringLength(100, ErrorMessage = "Pickup Location should not be greater than 100")]
        public string PickupLocation { get; set; }

        [Required]
        [DisplayName("Drop Location")]
        [StringLength(100, ErrorMessage = "Drop Location should not be greater than 100")]
        public string DropLocation { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Pickup Time")]
        public DateTime PickupTime { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Payment { get; set; }
    }
}
