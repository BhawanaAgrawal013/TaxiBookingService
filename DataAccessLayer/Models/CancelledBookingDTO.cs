namespace DataAccessLayer.Models
{
    public class CancelledBookingDTO
    {
        public int Id { get; set; }

        [Required]  
        public bool IsPending { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "Payment out of range")]
        public decimal Amount { get; set; }

        public bool IsCharged { get; set; }
    }
}
