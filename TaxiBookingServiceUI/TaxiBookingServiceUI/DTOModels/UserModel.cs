namespace TaxiBookingServiceUI
{
    public class UserModel
    {
        [Required]
        [DisplayName("First Name")]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [StringLength(20)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue)]
        public decimal Wallet { get; set; }

        [Required]
        [StringLength(6)]
        public string Gender { get; set; }

        [Required]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string RoleName { get; set; }
    }
}
