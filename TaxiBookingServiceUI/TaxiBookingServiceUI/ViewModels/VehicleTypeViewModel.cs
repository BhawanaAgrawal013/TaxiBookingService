namespace TaxiBookingServiceUI.Models
{
    public class VehicleTypeViewModel
    {
        [Required]
        [DisplayName("Vehicle Type")]
        [StringLength(15)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string VehicleType { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}
