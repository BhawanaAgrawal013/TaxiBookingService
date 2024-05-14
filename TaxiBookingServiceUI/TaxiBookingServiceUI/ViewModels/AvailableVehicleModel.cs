namespace TaxiBookingServiceUI.Models
{
    public class AvailableVehicleModel
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Rating { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        [DisplayName("Vehicle Model")]
        public string VehicleModel { get; set; }

        [Required]
        [StringLength(20)]
        [DisplayName("Vehicle Type")]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string VehicleType { get; set; }

        [Required]
        [DisplayName("No. of Seats")]
        public int Seating { get; set; }

        [Required]
        [StringLength(20)]
        public string RegistrationNumber { get; set; }
    }
}
