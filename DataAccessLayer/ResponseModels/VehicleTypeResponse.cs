namespace DataAccessLayer
{
    public class VehicleTypeResponse
    {
        public int Id {  get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string VehicleType { get; set; }
    }
}
