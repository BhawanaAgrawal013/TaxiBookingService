namespace TaxiBookingServiceUI.Models
{
    public class CancellationResonViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Cancellation reason should be less than 50")]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid reason")]
        public string Reason { get; set; }

        public bool IsCharged { get; set; }
    }
}
