namespace DataAccessLayer.Models
{
    public class PaymentModeDTO
    {
        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Pyament Mode")]
        public string PaymentMode { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}
