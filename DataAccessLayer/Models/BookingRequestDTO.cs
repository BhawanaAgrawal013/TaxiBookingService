namespace DataAccessLayer.Models
{
    public class BookingRequestDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Pickup Location should not be greater than 100")]
        public string PickupLocation { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Drop Location should not be greater than 100")]
        public string DropLocation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PickupTime { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "Payment out of range")]
        public decimal Payment { get; set; }
    }
}
