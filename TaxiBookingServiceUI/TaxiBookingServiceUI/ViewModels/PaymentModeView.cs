namespace TaxiBookingServiceUI.Models
{
    public class PaymentModeView
    {
        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Pyament Mode")]
        [DisplayName("Payment Mode")]
        public string PaymentMode { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string UserEmail { get; set; }
    }
}
