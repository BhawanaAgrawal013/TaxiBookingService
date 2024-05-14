namespace TaxiBookingServiceUI.Models
{
    public class CancelledBookingView
    {
        public int Id { get; set; }

        [Required]
        public bool IsPending { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayName("Booking Payment")]
        public decimal Amount { get; set; }

        public bool IsCharged { get; set; }
    }
}
