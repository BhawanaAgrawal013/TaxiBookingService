namespace DataAccessLayer.Models
{
    public class CityDTO
    {
        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid City Name")]
        public string City { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid State Name")]
        public string State { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string UserEmail { get; set; }
    }
}
