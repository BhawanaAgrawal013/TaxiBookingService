namespace DataAccessLayer
{
    public class BookingResponse
    {
        [Required]
        [StringLength(100, ErrorMessage = "Pickup Location should not be greater than 100")]
        public string PickupLocation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PickupTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DropTime { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Drop Location should not be greater than 100")]
        public string DropLocation { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Range(1, int.MaxValue, ErrorMessage = "Payment out of range")]
        public decimal Payment { get; set; }

        [Required]
        public string PaymentMode { get; set; }

        [Range(0,5, ErrorMessage = "Rating cannot be less than 0 or more than 5")]
        public decimal Rating { get; set; }

        [Range(0, 5, ErrorMessage = "Rating cannot be less than 0 or more than 5")]
        public decimal UserRating { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone number cannot be of length less or more than 10")]
        public string PhoneNumber { get; set; }

        public string Status { get; set; }

        [StringLength(40)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string UserName { get; set; }

        [StringLength(40)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string DriverName { get; set; }
    }
}
