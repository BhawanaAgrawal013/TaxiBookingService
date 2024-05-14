namespace DataAccessLayer.Models
{
    public class VehicleDetailDTO
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Vehicle Model")]
        public string VehicleModel { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Vehicle Type")]
        public string VehicleType { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Seating should be less than 5 or more than 1")]
        public int Seating { get; set; }
        
        [Required]
        [StringLength(20)]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Registration Name")]
        public string RegistrationName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Email cannot be more than 20")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}
