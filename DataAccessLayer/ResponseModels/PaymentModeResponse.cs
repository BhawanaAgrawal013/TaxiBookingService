namespace DataAccessLayer
{
    public class PaymentModeResponse
    {
        public int  Id { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Pyament Mode")]
        public string PaymentMode { get; set; }
    }
}
