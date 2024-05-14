namespace DataAccessLayer.ResponseModels
{
    public class PaymentVerificationModel
    {
        public bool IsVerify { get; set; }

        [Required]
        public string PaymentMode { get; set; }

        public string Remark { get; set; }
    }
}
