namespace TaxiBookingServiceUI.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Pickup Location")]
        [StringLength(100, ErrorMessage = "Pickup Location should not be greater than 100")]
        public string PickupLocation { get; set; }

        [Required]
        [DisplayName("Pickup Time")]
        public DateTime PickupTime { get; set; }

        [Required]
        [DisplayName("Drop Time")]
        public DateTime DropTime { get; set; }

        [Required]
        [DisplayName("Drop Location")]
        [StringLength(100, ErrorMessage = "Pickup Location should not be greater than 100")]
        public string DropLocation { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Payment { get; set; }

        [Required]
        [DisplayName("Payment Mode")]
        public string PaymentMode { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone number cannot be of length less or more than 10")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Vehicle Preference")]
        public string VehiclePreference { get; set; }

        public string Status { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string DriverEmail { get; set; }

        public string Verify { get; set; }
    }
}
