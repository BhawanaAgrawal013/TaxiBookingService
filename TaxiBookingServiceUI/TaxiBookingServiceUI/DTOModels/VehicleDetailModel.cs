namespace TaxiBookingServiceUI
{
    public class VehicleDetailModel
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [DisplayName("Vehicle Model")]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid VehicleModel")]
        public string VehicleModel { get; set; }

        [Required]
        [DisplayName("Vehicle Type")]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid VehicleType")]
        public string VehicleType { get; set; }

        [Required]
        [DisplayName("No. of Seats")]
        [Range(1, 5, ErrorMessage = "Seating should be less than 5 or more than 1")]
        public int Seating { get; set; }

        [Required]
        [DisplayName("Registration Number")]
        [StringLength(20)]
        public string RegistrationNumber { get; set; }

        [Required]
        [DisplayName("Registration Name")]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string RegistrationName { get; set; }

        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}
