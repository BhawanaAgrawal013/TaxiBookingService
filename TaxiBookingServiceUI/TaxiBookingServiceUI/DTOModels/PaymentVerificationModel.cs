namespace TaxiBookingServiceUI
{
    public class PaymentVerificationModel
    {
        public bool IsVerify { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Pyament Mode")]
        public string PaymentMode { get; set; }

        [Required]
        public string Remark { get; set; }
    }
}
