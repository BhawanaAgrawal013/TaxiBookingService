namespace DataAccessLayer
{
    public class VehicleDetailResponse
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string VehicleModel { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string VehicleType { get; set; }

        [Required]
        [Range(1,5, ErrorMessage = "Seating cannot be less than 5 or more than 1")]
        public int Seating { get; set; }
        
        [Required]
        [StringLength(20)]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Registration Name")]
        public string RegistrationName { get; set; }
    }
}
